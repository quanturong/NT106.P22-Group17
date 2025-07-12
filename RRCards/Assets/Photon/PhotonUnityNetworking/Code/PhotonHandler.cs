


namespace Photon.Pun
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ExitGames.Client.Photon;
    using Photon.Realtime;
    using UnityEngine;
    using UnityEngine.Profiling;

    using Debug = UnityEngine.Debug;
    public class PhotonHandler : ConnectionHandler, IInRoomCallbacks, IMatchmakingCallbacks
    {

        private static PhotonHandler instance;
        internal static PhotonHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    #if UNITY_6000_0_OR_NEWER
                    instance = FindFirstObjectByType<PhotonHandler>();
                    #else
                    instance = FindObjectOfType<PhotonHandler>();
                    #endif
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "PhotonMono";
                        instance = obj.AddComponent<PhotonHandler>();
                    }
                }

                return instance;
            }
        }
        public static int MaxDatagrams = 10;
        public static bool SendAsap;
        private const int SerializeRateFrameCorrection = 8;

        protected internal int UpdateInterval; // time [ms] between consecutive SendOutgoingCommands calls

        protected internal int UpdateIntervalOnSerialize; // time [ms] between consecutive RunViewUpdate calls (sending syncs, etc)


        private readonly Stopwatch swSendOutgoing = new Stopwatch();

        private readonly Stopwatch swViewUpdate = new Stopwatch();

        private SupportLogger supportLoggerComponent;


        protected override void Awake()
        {
            this.swSendOutgoing.Start();
            this.swViewUpdate.Start();

            if (instance == null || ReferenceEquals(this, instance))
            {
                instance = this;
                base.Awake();
            }
            else
            {
                Destroy(this);
            }
        }

        protected virtual void OnEnable()
        {
            if (Instance != this)
            {
                Debug.LogError("PhotonHandler is a singleton but there are multiple instances. this != Instance.");
                return;
            }

            this.Client = PhotonNetwork.NetworkingClient;

            if (PhotonNetwork.PhotonServerSettings.EnableSupportLogger)
            {
                SupportLogger supportLogger = this.gameObject.GetComponent<SupportLogger>();
                if (supportLogger == null)
                {
                    supportLogger = this.gameObject.AddComponent<SupportLogger>();
                }
                if (this.supportLoggerComponent != null)
                {
                    if (supportLogger.GetInstanceID() != this.supportLoggerComponent.GetInstanceID())
                    {
                        Debug.LogWarningFormat("Cached SupportLogger component is different from the one attached to PhotonMono GameObject");
                    }
                }
                this.supportLoggerComponent = supportLogger;
                this.supportLoggerComponent.Client = PhotonNetwork.NetworkingClient;
            }

            this.UpdateInterval = 1000 / PhotonNetwork.SendRate;
            this.UpdateIntervalOnSerialize = 1000 / PhotonNetwork.SerializationRate;

            PhotonNetwork.AddCallbackTarget(this);
            this.StartFallbackSendAckThread();  // this is not done in the base class
        }

        protected void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                PhotonNetwork.NewSceneLoaded();
            };
        }

        protected override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            base.OnDisable();
        }
        protected void FixedUpdate()
        {
            #if PUN_DISPATCH_IN_FIXEDUPDATE
            this.Dispatch();
            #elif PUN_DISPATCH_IN_LATEUPDATE
            #else
            if (Time.timeScale > PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate)
            {
                this.Dispatch();
            }
            #endif
        }
        protected void LateUpdate()
        {
            #if PUN_DISPATCH_IN_LATEUPDATE
            this.Dispatch();
            #elif PUN_DISPATCH_IN_FIXEDUPDATE
            #else
            if (Time.timeScale <= PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate)
            {
                this.Dispatch();
            }
            #endif

            if (PhotonNetwork.IsMessageQueueRunning && this.swViewUpdate.ElapsedMilliseconds >= this.UpdateIntervalOnSerialize - SerializeRateFrameCorrection)
            {
                PhotonNetwork.RunViewUpdate();
                this.swViewUpdate.Restart();
                SendAsap = true; // immediately send when synchronization code was running
            }

            
            if (SendAsap || this.swSendOutgoing.ElapsedMilliseconds >= this.UpdateInterval)
            {
                SendAsap = false;
                bool doSend = true;
                int sendCounter = 0;
                while (PhotonNetwork.IsMessageQueueRunning && doSend && sendCounter < MaxDatagrams)
                {
                    Profiler.BeginSample("SendOutgoingCommands");
                    doSend = PhotonNetwork.NetworkingClient.LoadBalancingPeer.SendOutgoingCommands();
                    sendCounter++;
                    Profiler.EndSample();
                }
                if (sendCounter >= MaxDatagrams)
                {
                    SendAsap = true;
                }

                this.swSendOutgoing.Restart();
            }
        }
        protected void Dispatch()
        {
            if (PhotonNetwork.NetworkingClient == null)
            {
                Debug.LogError("NetworkPeer broke!");
                return;
            }


            bool doDispatch = true;
            Exception ex = null;
            int exceptionCount = 0;
            while (PhotonNetwork.IsMessageQueueRunning && doDispatch)
            {
                Profiler.BeginSample("DispatchIncomingCommands");
                try
                {
                    doDispatch = PhotonNetwork.NetworkingClient.LoadBalancingPeer.DispatchIncomingCommands();
                }
                catch (Exception e)
                {
                    exceptionCount++;
                    if (ex == null)
                    {
                        ex = e;
                    }
                }

                Profiler.EndSample();
            }

            if (ex != null)
            {
                throw new AggregateException("Caught " + exceptionCount + " exception(s) in methods called by DispatchIncomingCommands(). Rethrowing first only (see above).", ex);
            }
        }


        public void OnCreatedRoom()
        {
            PhotonNetwork.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            PhotonNetwork.LoadLevelIfSynced();
        }


        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            var views = PhotonNetwork.PhotonViewCollection;
            foreach (var view in views)
            {
                if (view.IsRoomView)
                {
                    view.OwnerActorNr= newMasterClient.ActorNumber;
                    view.ControllerActorNr = newMasterClient.ActorNumber;
                }
            }
        }

        public void OnFriendListUpdate(System.Collections.Generic.List<FriendInfo> friendList) { }

        public void OnCreateRoomFailed(short returnCode, string message) { }

        public void OnJoinRoomFailed(short returnCode, string message) { }

        public void OnJoinRandomFailed(short returnCode, string message) { }

        protected List<int> reusableIntList = new List<int>();

        public void OnJoinedRoom()
        {

            if (PhotonNetwork.ViewCount == 0)
                return;

            var views = PhotonNetwork.PhotonViewCollection;

            bool amMasterClient = PhotonNetwork.IsMasterClient;
            bool amRejoiningMaster = amMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1;

            if (amRejoiningMaster)
                reusableIntList.Clear();
            foreach (var view in views)
            {
                int viewOwnerId = view.OwnerActorNr;
                int viewCreatorId = view.CreatorActorNr;
                    view.RebuildControllerCache();
                if (amRejoiningMaster)
                    if (viewOwnerId != viewCreatorId)
                    {
                        reusableIntList.Add(view.ViewID);
                        reusableIntList.Add(viewOwnerId);
                    }
            }

            if (amRejoiningMaster && reusableIntList.Count > 0)
            {
                PhotonNetwork.OwnershipUpdate(reusableIntList.ToArray());
            }
        }

        public void OnLeftRoom()
        {
        }


        public void OnPlayerEnteredRoom(Player newPlayer)
        {

            bool amMasterClient = PhotonNetwork.IsMasterClient;

            var views = PhotonNetwork.PhotonViewCollection;
            if (amMasterClient)
            {
                reusableIntList.Clear();
            }

            foreach (var view in views)
            {
                view.RebuildControllerCache();  // all clients will potentially have to clean up owner and controller, if someone re-joins
                if (amMasterClient)
                {
                    int viewOwnerId = view.OwnerActorNr;
                    if (viewOwnerId != view.CreatorActorNr)
                    {
                        reusableIntList.Add(view.ViewID);
                        reusableIntList.Add(viewOwnerId);
                    }
                }
            }
            if (amMasterClient && reusableIntList.Count > 0)
            {
                PhotonNetwork.OwnershipUpdate(reusableIntList.ToArray(), newPlayer.ActorNumber);
            }

        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            var views = PhotonNetwork.PhotonViewCollection;

            int leavingPlayerId = otherPlayer.ActorNumber;
            bool isInactive = otherPlayer.IsInactive;
            if (isInactive)
            {
                foreach (var view in views)
                {
                    if (view.ControllerActorNr == leavingPlayerId)
                        view.ControllerActorNr = PhotonNetwork.MasterClient.ActorNumber;
                }

            }
            else
            {
                bool autocleanup = PhotonNetwork.CurrentRoom.AutoCleanUp;

                foreach (var view in views)
                {
                    if (autocleanup && view.CreatorActorNr == leavingPlayerId)
                        continue;
                    if (view.OwnerActorNr == leavingPlayerId || view.ControllerActorNr == leavingPlayerId)
                    {
                        view.OwnerActorNr = 0;
                        view.ControllerActorNr = PhotonNetwork.MasterClient.ActorNumber;
                    }
                }
            }
        }
    }
}