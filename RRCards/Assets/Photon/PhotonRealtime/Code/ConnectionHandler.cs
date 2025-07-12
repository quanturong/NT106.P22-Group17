

#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System;
    using System.Threading;
    using System.Diagnostics;
    using SupportClass = ExitGames.Client.Photon.SupportClass;

    #if SUPPORTED_UNITY
    using UnityEngine;
    #endif


    #if SUPPORTED_UNITY
    public class ConnectionHandler : MonoBehaviour
    #else
    public class ConnectionHandler
    #endif
    {
        public LoadBalancingClient Client { get; set; }
        public bool DisconnectAfterKeepAlive = false;
        public int KeepAliveInBackground = 60000;
        public int CountSendAcksOnly { get; private set; }
        public bool FallbackThreadRunning { get; private set; }
        public bool ApplyDontDestroyOnLoad = true;
        [NonSerialized]
        public static bool AppQuits;
        [NonSerialized]
        public static bool AppPause;
        [NonSerialized]
        public static bool AppPauseRecent;
        [NonSerialized]
        public static bool AppOutOfFocus;
        [NonSerialized]
        public static bool AppOutOfFocusRecent;


        private bool didSendAcks;
        private readonly Stopwatch backgroundStopwatch = new Stopwatch();

        private Timer stateTimer;

        #if SUPPORTED_UNITY

        #if UNITY_2019_4_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void StaticReset()
        {
            AppQuits = false;
            AppPause = false;
            AppPauseRecent = false;
            AppOutOfFocus = false;
            AppOutOfFocusRecent = false;
        }

        #endif
        protected virtual void Awake()
        {
            if (this.ApplyDontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        protected virtual void OnDisable()
        {
            this.StopFallbackSendAckThread();

            if (AppQuits)
            {
                if (this.Client != null && this.Client.IsConnected)
                {
                    this.Client.Disconnect(DisconnectCause.ApplicationQuit);
                    this.Client.LoadBalancingPeer.StopThread();
                }

                SupportClass.StopAllBackgroundCalls();
            }
        }
        public void OnApplicationQuit()
        {
            AppQuits = true;
        }
        public void OnApplicationPause(bool pause)
        {
            AppPause = pause;

            if (pause)
            {
                AppPauseRecent = true;
                this.CancelInvoke(nameof(this.ResetAppPauseRecent));
            }
            else
            {
                Invoke(nameof(this.ResetAppPauseRecent), 5f);
            }
        }

        private void ResetAppPauseRecent()
        {
            AppPauseRecent = false;
        }
        public void OnApplicationFocus(bool focus)
        {
            AppOutOfFocus = !focus;
            if (!focus)
            {
                AppOutOfFocusRecent = true;
                this.CancelInvoke(nameof(this.ResetAppOutOfFocusRecent));
            }
            else
            {
                this.Invoke(nameof(this.ResetAppOutOfFocusRecent), 5f);
            }
        }

        private void ResetAppOutOfFocusRecent()
        {
            AppOutOfFocusRecent = false;
        }


        #endif
        public static bool IsNetworkReachableUnity()
        {
            #if SUPPORTED_UNITY
            return Application.internetReachability != NetworkReachability.NotReachable;
            #else
            return true;
            #endif
        }
        public void StartFallbackSendAckThread()
        {
            #if UNITY_WEBGL
            if (!this.FallbackThreadRunning) this.InvokeRepeating(nameof(this.RealtimeFallbackInvoke), 0.05f, 0.05f);
            #else
            if (this.stateTimer != null)
            {
                return;
            }

            stateTimer = new Timer(this.RealtimeFallback, null, 50, 50);
            #endif

            this.FallbackThreadRunning = true;
        }
        public void StopFallbackSendAckThread()
        {
            #if UNITY_WEBGL
            if (this.FallbackThreadRunning) this.CancelInvoke(nameof(this.RealtimeFallbackInvoke));
            #else
            if (this.stateTimer != null)
            {
                this.stateTimer.Dispose();
                this.stateTimer = null;
            }
            #endif

            this.FallbackThreadRunning = false;
        }
        public void RealtimeFallbackInvoke()
        {
            this.RealtimeFallback();
        }
        public void RealtimeFallback(object state = null)
        {
            if (this.Client == null)
            {
                return;
            }

            if (this.Client.IsConnected && this.Client.LoadBalancingPeer.ConnectionTime - this.Client.LoadBalancingPeer.LastSendOutgoingTime > 100)
            {
                if (!this.didSendAcks)
                {
                    this.backgroundStopwatch.Reset();
                    this.backgroundStopwatch.Start();
                }
                if (this.backgroundStopwatch.ElapsedMilliseconds > this.KeepAliveInBackground)
                {
                    if (this.DisconnectAfterKeepAlive && this.Client.State != ClientState.Disconnecting)
                    {
                        this.Client.Disconnect();
                    }
                    return;
                }


                this.didSendAcks = true;
                this.CountSendAcksOnly++;

                this.Client.LoadBalancingPeer.SendAcksOnly();
            }
            else
            {
                if (this.backgroundStopwatch.IsRunning)
                {
                    this.backgroundStopwatch.Reset();
                }
                this.didSendAcks = false;
            }
        }
    }
}