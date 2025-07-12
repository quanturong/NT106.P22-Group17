
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif

#if UNITY_WEBGL
#define PING_VIA_COROUTINE
#endif


namespace Photon.Realtime
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ExitGames.Client.Photon;
    using System.Linq;

    #if SUPPORTED_UNITY
    using UnityEngine;
    using Debug = UnityEngine.Debug;
    #endif
    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class RegionHandler
    {
        public static Type PingImplementation;
        public List<Region> EnabledRegions { get; protected internal set; }

        private string availableRegionCodes;

        private Region bestRegionCache;
        public Region BestRegion
        {
            get
            {
                if (this.EnabledRegions == null)
                {
                    return null;
                }

                if (this.bestRegionCache != null)
                {
                    return this.bestRegionCache;
                }

                this.EnabledRegions.Sort((a, b) => a.Ping.CompareTo(b.Ping));
                int similarPingCutoff = (int)(this.EnabledRegions[0].Ping * pingSimilarityFactor);
                Region firstFromSimilar = this.EnabledRegions[0];
                foreach (Region region in this.EnabledRegions)
                {
                    if (region.Ping <= similarPingCutoff && region.Code.CompareTo(firstFromSimilar.Code) < 0)
                    {
                        firstFromSimilar = region;
                    }
                }

                this.bestRegionCache = firstFromSimilar;
                return this.bestRegionCache;
            }
        }
        public string SummaryToCache
        {
            get
            {
                if (this.BestRegion != null && this.BestRegion.Ping < RegionPinger.MaxMillisecondsPerPing)
                {
                    return this.BestRegion.Code + ";" + this.BestRegion.Ping + ";" + this.availableRegionCodes;
                }

                return this.availableRegionCodes;
            }
        }
        public string GetResults()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Region Pinging Result: {0}\n", this.BestRegion.ToString());
            foreach (RegionPinger region in this.pingerList)
            {
                sb.AppendLine(region.GetResults());
            }

            sb.AppendFormat("Previous summary: {0}", this.previousSummaryProvided);

            return sb.ToString();
        }
        public void SetRegions(OperationResponse opGetRegions, LoadBalancingClient loadBalancingClient = null)
        {
            if (opGetRegions.OperationCode != OperationCode.GetRegions)
            {
                return;
            }

            if (opGetRegions.ReturnCode != ErrorCode.Ok)
            {
                return;
            }

            string[] regions = opGetRegions[ParameterCode.Region] as string[];
            string[] servers = opGetRegions[ParameterCode.Address] as string[];
            if (regions == null || servers == null || regions.Length != servers.Length)
            {
                if (loadBalancingClient != null)
                {
                    loadBalancingClient.DebugReturn(DebugLevel.ERROR, "RegionHandler.SetRegions() failed. Received regions and servers must be non null and of equal length. Could not read regions.");
                }
                return;
            }

            this.bestRegionCache = null;
            this.EnabledRegions = new List<Region>(regions.Length);

            for (int i = 0; i < regions.Length; i++)
            {
                string server = servers[i];
                if (PortToPingOverride != 0)
                {
                    server = LoadBalancingClient.ReplacePortWithAlternative(servers[i], PortToPingOverride);
                }

                if (loadBalancingClient != null && loadBalancingClient.AddressRewriter != null)
                {
                    server = loadBalancingClient.AddressRewriter(server, ServerConnection.MasterServer);
                }

                Region tmp = new Region(regions[i], server);
                if (string.IsNullOrEmpty(tmp.Code))
                {
                    continue;
                }

                this.EnabledRegions.Add(tmp);
            }

            Array.Sort(regions);
            this.availableRegionCodes = string.Join(",", regions);
        }

        private readonly List<RegionPinger> pingerList = new List<RegionPinger>();
        private Action<RegionHandler> onCompleteCall;
        private int previousPing;
        private string previousSummaryProvided;
        protected internal static ushort PortToPingOverride;
        private float rePingFactor = 1.2f;
        private float pingSimilarityFactor = 1.2f;
        public int BestRegionSummaryPingLimit = 90;
        public bool IsPinging { get; private set; }
        public bool Aborted { get; private set; }
        #if SUPPORTED_UNITY
        private MonoBehaviourEmpty emptyMonoBehavior;
        #endif

        #if PHOTON_LOCATION
        internal Location Location = new Location();
        #endif
        public RegionHandler(ushort masterServerPortOverride = 0)
        {
            PortToPingOverride = masterServerPortOverride;
        }
        public bool PingMinimumOfRegions(Action<RegionHandler> onCompleteCallback, string previousSummary)
        {
            if (this.EnabledRegions == null || this.EnabledRegions.Count == 0)
            {
                return false;
            }

            if (this.IsPinging)
            {
                return false;
            }

            this.Aborted = false;
            this.IsPinging = true;
            this.previousSummaryProvided = previousSummary;

            #if SUPPORTED_UNITY
            if (this.emptyMonoBehavior != null)
            {
                this.emptyMonoBehavior.SelfDestroy();
            }
            this.emptyMonoBehavior = MonoBehaviourEmpty.BuildInstance(nameof(RegionHandler));
            this.emptyMonoBehavior.onCompleteCall = onCompleteCallback;
            this.onCompleteCall = emptyMonoBehavior.CompleteOnMainThread;
            #else
            this.onCompleteCall = onCompleteCallback;
            #endif

            #if PHOTON_LOCATION
            #if SUPPORTED_UNITY
            this.Location.FetchLocation(this.emptyMonoBehavior, null);
            #else
            this.Location.FetchLocation();
            #endif
            #endif


            if (string.IsNullOrEmpty(previousSummary))
            {
                return this.PingEnabledRegions();
            }

            string[] values = previousSummary.Split(';');
            if (values.Length < 3)
            {
                return this.PingEnabledRegions();
            }

            int prevBestRegionPing;
            bool secondValueIsInt = Int32.TryParse(values[1], out prevBestRegionPing);
            if (!secondValueIsInt)
            {
                return this.PingEnabledRegions();
            }

            string prevBestRegionCode = values[0];
            string prevAvailableRegionCodes = values[2];


            if (string.IsNullOrEmpty(prevBestRegionCode))
            {
                return this.PingEnabledRegions();
            }
            if (string.IsNullOrEmpty(prevAvailableRegionCodes))
            {
                return this.PingEnabledRegions();
            }
            if (!this.availableRegionCodes.Equals(prevAvailableRegionCodes) || !this.availableRegionCodes.Contains(prevBestRegionCode))
            {
                return this.PingEnabledRegions();
            }
            if (prevBestRegionPing >= RegionPinger.PingWhenFailed)
            {
                return this.PingEnabledRegions();
            }
            this.previousPing = prevBestRegionPing;


            Region preferred = this.EnabledRegions.Find(r => r.Code.Equals(prevBestRegionCode));
            RegionPinger singlePinger = new RegionPinger(preferred, this.OnPreferredRegionPinged);

            lock (this.pingerList)
            {
                this.pingerList.Clear();
                this.pingerList.Add(singlePinger);
            }

            singlePinger.Start();
            return true;
        }
        public void Abort()
        {
            if (this.Aborted)
            {
                return;
            }

            this.Aborted = true;
            lock (this.pingerList)
            {
                foreach (RegionPinger pinger in this.pingerList)
                {
                    pinger.Abort();
                }
            }

            #if SUPPORTED_UNITY
            if (this.emptyMonoBehavior != null)
            {
                this.emptyMonoBehavior.SelfDestroy();
            }
            #endif
        }

        private void OnPreferredRegionPinged(Region preferredRegion)
        {
            if (preferredRegion.Ping > this.BestRegionSummaryPingLimit || preferredRegion.Ping > this.previousPing * this.rePingFactor)
            {
                this.PingEnabledRegions();
            }
            else
            {
                this.IsPinging = false;
                this.onCompleteCall(this);
            }
        }
        private bool PingEnabledRegions()
        {
            if (this.EnabledRegions == null || this.EnabledRegions.Count == 0)
            {
                return false;
            }

            lock (this.pingerList)
            {
                this.pingerList.Clear();

                foreach (Region region in this.EnabledRegions)
                {
                    RegionPinger rp = new RegionPinger(region, this.OnRegionDone);
                    this.pingerList.Add(rp);
                    rp.Start(); // TODO: check return value
                }
            }

            return true;
        }

        private void OnRegionDone(Region region)
        {
            lock (this.pingerList)
            {
                if (this.IsPinging == false)
                {
                    return;
                }

                this.bestRegionCache = null;
                foreach (RegionPinger pinger in this.pingerList)
                {
                    if (!pinger.Done)
                    {
                        return;
                    }
                }

                this.IsPinging = false;
            }

            if (!this.Aborted)
            {
                this.onCompleteCall(this);
            }
        }
    }
    public class RegionPinger
    {
        public static int Attempts = 5;
        public static int MaxMillisecondsPerPing = 800; // enter a value you're sure some server can beat (have a lower rtt)
        public static int PingWhenFailed = Attempts * MaxMillisecondsPerPing;
        public int CurrentAttempt = 0;
        public bool Done { get; private set; }
        public bool Aborted { get; internal set; }


        private Action<Region> onDoneCall;
        private PhotonPing ping;
        private List<int> rttResults;
        private Region region;
        private string regionAddress;
        public RegionPinger(Region region, Action<Region> onDoneCallback)
        {
            this.region = region;
            this.region.Ping = PingWhenFailed;
            this.Done = false;
            this.onDoneCall = onDoneCallback;
        }
        private PhotonPing GetPingImplementation()
        {
            PhotonPing ping = null;

            #if !UNITY_EDITOR && NETFX_CORE
            if (RegionHandler.PingImplementation == null || RegionHandler.PingImplementation == typeof(PingWindowsStore))
            {
                ping = new PingWindowsStore();
            }
            #elif NATIVE_SOCKETS || NO_SOCKET
            if (RegionHandler.PingImplementation == null || RegionHandler.PingImplementation == typeof(PingNativeDynamic))
            {
                ping = new PingNativeDynamic();
            }
            #elif UNITY_WEBGL
            if (RegionHandler.PingImplementation == null || RegionHandler.PingImplementation == typeof(PingHttp))
            {
                ping = new PingHttp();
            }
            #else
            if (RegionHandler.PingImplementation == null || RegionHandler.PingImplementation == typeof(PingMono))
            {
                ping = new PingMono();
            }
            #endif

            if (ping == null)
            {
                if (RegionHandler.PingImplementation != null)
                {
                    ping = (PhotonPing)Activator.CreateInstance(RegionHandler.PingImplementation);
                }
            }

            return ping;
        }
        public bool Start()
        {
            this.ping = this.GetPingImplementation();

            this.Done = false;
            this.CurrentAttempt = 0;
            this.rttResults = new List<int>(Attempts);

            if (this.Aborted)
            {
                return false;
            }

            #if PING_VIA_COROUTINE
            MonoBehaviourEmpty.BuildInstance("RegionPing_" + this.region.Code).StartCoroutineAndDestroy(this.RegionPingCoroutine());
            #else
            bool queued = false;
            #if !NETFX_CORE
            try
            {
                queued = ThreadPool.QueueUserWorkItem(o => this.RegionPingThreaded());
            }
            catch
            {
                queued = false;
            }
            #endif
            if (!queued)
            {
                SupportClass.StartBackgroundCalls(this.RegionPingThreaded, 0, "RegionPing_" + this.region.Code + "_" + this.region.Cluster);
            }
            #endif


            return true;
        }
        protected internal void Abort()
        {
            this.Aborted = true;
            if (this.ping != null)
            {
                this.ping.Dispose();
            }
        }
        protected internal bool RegionPingThreaded()
        {
            this.region.Ping = PingWhenFailed;

            int rttSum = 0;
            int replyCount = 0;
            Stopwatch sw = new Stopwatch();

            try
            {
                string address = this.region.HostAndPort;
                int indexOfColon = address.LastIndexOf(':');
                if (indexOfColon > 1)
                {
                    address = address.Substring(0, indexOfColon);
                }

                sw.Start();
                this.regionAddress = ResolveHost(address);
                sw.Stop();
                if (sw.ElapsedMilliseconds > 100)
                {
                    System.Diagnostics.Debug.WriteLine($"RegionPingThreaded.ResolveHost() took: {sw.ElapsedMilliseconds}ms");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"RegionPingThreaded ResolveHost failed for {this.region}. Caught: {e}");
                this.Aborted = true;
            }

            for (this.CurrentAttempt = 0; this.CurrentAttempt < Attempts; this.CurrentAttempt++)
            {
                if (this.Aborted)
                {
                    break;
                }

                sw.Reset();
                sw.Start();

                try
                {
                    this.ping.StartPing(this.regionAddress);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("RegionPinger.RegionPingThreaded() caught exception for ping.StartPing(). Exception: " + e + " Source: " + e.Source + " Message: " + e.Message);
                    break;
                }


                while (!this.ping.Done())
                {
                    if (sw.ElapsedMilliseconds >= MaxMillisecondsPerPing)
                    {
                        break;
                    }
                    #if !NETFX_CORE
                    System.Threading.Thread.Sleep(1);
                    #endif
                }


                sw.Stop();
                int rtt = this.ping.Successful ? (int)sw.ElapsedMilliseconds : MaxMillisecondsPerPing;   // if the reply didn't match the sent ping
                this.rttResults.Add(rtt);

                rttSum += rtt;
                replyCount++;
                this.region.Ping = (int)((rttSum) / replyCount);

                #if !NETFX_CORE
                int i = 4;
                while (!this.ping.Done() && i > 0)
                {
                    i--;
                    System.Threading.Thread.Sleep(100);
                }
                System.Threading.Thread.Sleep(10);
                #endif
            }
            this.Done = true;
            this.ping.Dispose();

            if (this.rttResults.Count > 1 && replyCount > 0)
            {
                int bestRtt = this.rttResults.Min();
                int worstRtt = this.rttResults.Max();
                int weighedRttSum = rttSum - worstRtt + bestRtt;
                this.region.Ping = (int)(weighedRttSum / replyCount); // now, we can create a weighted ping value
            }

            this.onDoneCall(this.region);
            return false;
        }


        #if SUPPORTED_UNITY
        protected internal IEnumerator RegionPingCoroutine()
        {
            this.region.Ping = PingWhenFailed;

            int rttSum = 0;
            int replyCount = 0;
            Stopwatch sw = new Stopwatch();

            try
            {
                string address = this.region.HostAndPort;
                int indexOfColon = address.LastIndexOf(':');
                if (indexOfColon > 1)
                {
                    address = address.Substring(0, indexOfColon);
                }

                sw.Start();
                this.regionAddress = ResolveHost(address);
                sw.Stop();
                if (sw.ElapsedMilliseconds > 100)
                {
                    Debug.Log($"RegionPingCoroutine.ResolveHost() took: {sw.ElapsedMilliseconds}ms");
                }
            }
            catch (Exception e)
            {
                Debug.Log($"RegionPingCoroutine ResolveHost failed for {this.region}. Caught: {e}");
                this.Aborted = true;
            }

            for (this.CurrentAttempt = 0; this.CurrentAttempt < Attempts; this.CurrentAttempt++)
            {
                if (this.Aborted)
                {
                    yield return null;
                }

                sw.Reset();
                sw.Start();

                try
                {
                    this.ping.StartPing(this.regionAddress);
                }
                catch (Exception e)
                {
                    Debug.Log("RegionPinger.RegionPingCoroutine() caught exception for ping.StartPing(). Exception: " + e + " Source: " + e.Source + " Message: " + e.Message);
                    break;
                }


                while (!this.ping.Done())
                {
                    if (sw.ElapsedMilliseconds >= MaxMillisecondsPerPing)
                    {
                        break;
                    }

                    yield return new WaitForSecondsRealtime(0.01f); // keep this loop tight, to avoid adding local lag to rtt.
                }


                sw.Stop();
                int rtt = this.ping.Successful ? (int)sw.ElapsedMilliseconds : MaxMillisecondsPerPing; // if the reply didn't match the sent ping
                this.rttResults.Add(rtt);


                rttSum += rtt;
                replyCount++;
                this.region.Ping = (int)((rttSum) / replyCount);

                int i = 4;
                while (!this.ping.Done() && i > 0)
                {
                    i--;
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.1f);
            }
            this.Done = true;
            this.ping.Dispose();

            if (this.rttResults.Count > 1 && replyCount > 0)
            {
                int bestRtt = this.rttResults.Min();
                int worstRtt = this.rttResults.Max();
                int weighedRttSum = rttSum - worstRtt + bestRtt;
                this.region.Ping = (int)(weighedRttSum / replyCount); // now, we can create a weighted ping value
            }

            this.onDoneCall(this.region);
            yield return null;
        }
        #endif
        public string GetResults()
        {
            return string.Format("{0}: {1} ({2})", this.region.Code, this.region.Ping, this.rttResults.ToStringFull());
        }
        public static string ResolveHost(string hostName)
        {

			if (hostName.StartsWith("wss://"))
			{
				hostName = hostName.Substring(6);
			}
			if (hostName.StartsWith("ws://"))
			{
				hostName = hostName.Substring(5);
			}

            string ipv4Address = string.Empty;

            try
            {
                #if UNITY_WSA || NETFX_CORE || UNITY_WEBGL
                return hostName;
                #else

                IPAddress[] address = Dns.GetHostAddresses(hostName);
                if (address.Length == 1)
                {
                    return address[0].ToString();
                }
                for (int index = 0; index < address.Length; index++)
                {
                    IPAddress ipAddress = address[index];
                    if (ipAddress != null)
                    {
                        if (ipAddress.ToString().Contains(":"))
                        {
                            return ipAddress.ToString();
                        }
                        if (string.IsNullOrEmpty(ipv4Address))
                        {
                            ipv4Address = address.ToString();
                        }
                    }
                }
                #endif
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("RegionPinger.ResolveHost() caught an exception for Dns.GetHostAddresses(). Exception: " + e + " Source: " + e.Source + " Message: " + e.Message);
            }

            return ipv4Address;
        }
    }

    #if SUPPORTED_UNITY
    internal class MonoBehaviourEmpty : MonoBehaviour
    {
        internal Action<RegionHandler> onCompleteCall;
        private RegionHandler obj;

        public static MonoBehaviourEmpty BuildInstance(string id = null)
        {
            GameObject go = new GameObject(id ?? nameof(MonoBehaviourEmpty));
            DontDestroyOnLoad(go);

            return go.AddComponent<MonoBehaviourEmpty>();
        }

        public void SelfDestroy()
        {
            Destroy(this.gameObject);
        }

        void Update()
        {
            if (this.obj != null)
            {
                this.onCompleteCall(obj);
                this.obj = null;
                this.onCompleteCall = null;
                this.SelfDestroy();
            }
        }

        public void CompleteOnMainThread(RegionHandler obj)
        {
            this.obj = obj;
        }

        public void StartCoroutineAndDestroy(IEnumerator coroutine)
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return coroutine;
                this.SelfDestroy();
            }
        }
    }
    #endif
}
