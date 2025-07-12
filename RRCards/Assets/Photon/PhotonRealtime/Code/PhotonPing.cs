

namespace Photon.Realtime
{
    using System;
    using System.Collections;
    using System.Threading;

    #if NETFX_CORE
    using System.Diagnostics;
    using Windows.Foundation;
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    #endif

    #if !NO_SOCKET && !NETFX_CORE
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Sockets;
    #endif

    #if UNITY_WEBGL
    using UnityEngine.Networking;
    #endif
    public abstract class PhotonPing : IDisposable
    {
        public string DebugString = "";
        public bool Successful;
        protected internal bool GotResult;
        protected internal int PingLength = 13;
        protected internal byte[] PingBytes = new byte[] { 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x7d, 0x00 };
        protected internal byte PingId;

        private static readonly System.Random RandomIdProvider = new System.Random();
        public virtual bool StartPing(string ip)
        {
            throw new NotImplementedException();
        }
        public virtual bool Done()
        {
            throw new NotImplementedException();
        }
        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
        protected internal void Init()
        {
            this.GotResult = false;
            this.Successful = false;
            this.PingId = (byte)(RandomIdProvider.Next(255));
        }
    }


    #if !NETFX_CORE && !NO_SOCKET
    public class PingMono : PhotonPing
    {
        private Socket sock;
        public override bool StartPing(string ip)
        {
            this.Init();

            try
            {
                if (this.sock == null)
                {
                    if (ip.Contains("."))
                    {
                        this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    }
                    else
                    {
                        this.sock = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
                    }

                    this.sock.ReceiveTimeout = 5000;
                    int port = (RegionHandler.PortToPingOverride != 0) ? RegionHandler.PortToPingOverride : 5055;
                    this.sock.Connect(ip, port);
                }


                this.PingBytes[this.PingBytes.Length - 1] = this.PingId;
                this.sock.Send(this.PingBytes);
                this.PingBytes[this.PingBytes.Length - 1] = (byte)(this.PingId+1);  // this buffer is re-used for the result/receive. invalidate the result now.
            }
            catch (Exception e)
            {
                this.sock = null;
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw;
            }

            return false;
        }
        public override bool Done()
        {
            if (this.GotResult || this.sock == null)
            {
                return true;    // this just indicates the ping is no longer waiting. this.Successful value defines if the roundtrip completed
            }

            int read = 0;
            try
            {
                if (!this.sock.Poll(0, SelectMode.SelectRead))
                {
                    return false;
                }

                read = this.sock.Receive(this.PingBytes, SocketFlags.None);
            }
            catch (Exception ex)
            {
                if (this.sock != null)
                {
                    this.sock.Close();
                    this.sock = null;
                }
                this.DebugString += " Exception of socket! " + ex.GetType() + " ";
                return true;    // this just indicates the ping is no longer waiting. this.Successful value defines if the roundtrip completed
            }

            bool replyMatch = this.PingBytes[this.PingBytes.Length - 1] == this.PingId && read == this.PingLength;
            if (!replyMatch)
            {
                this.DebugString += " ReplyMatch is false! ";
            }


            this.Successful = replyMatch;
            this.GotResult = true;
            return true;
        }
        public override void Dispose()
        {
            if (this.sock == null) { return; }

            try
            {
                this.sock.Close();
            }
            catch
            {
            }

            this.sock = null;
        }

    }
    #endif


    #if NETFX_CORE
    public class PingWindowsStore : PhotonPing
    {
        private DatagramSocket sock;
        private readonly object syncer = new object();

        public override bool StartPing(string host)
        {
            lock (this.syncer)
            {
                this.Init();

                int port = (RegionHandler.PortToPingOverride != 0) ? RegionHandler.PortToPingOverride : 5055;
                EndpointPair endPoint = new EndpointPair(null, string.Empty, new HostName(host), port.ToString());
                this.sock = new DatagramSocket();
                this.sock.MessageReceived += this.OnMessageReceived;

                IAsyncAction result = this.sock.ConnectAsync(endPoint);
                result.Completed = this.OnConnected;
                this.DebugString += " End StartPing";
                return true;
            }
        }
        public override bool Done()
        {
            lock (this.syncer)
            {
                return this.GotResult || this.sock == null; // this just indicates the ping is no longer waiting. this.Successful value defines if the roundtrip completed
            }
        }
        public override void Dispose()
        {
            lock (this.syncer)
            {
                this.sock = null;
            }
        }

        private void OnConnected(IAsyncAction asyncinfo, AsyncStatus asyncstatus)
        {
            lock (this.syncer)
            {
                if (asyncinfo.AsTask().IsCompleted && !asyncinfo.AsTask().IsFaulted && this.sock != null && this.sock.Information.RemoteAddress != null)
                {
                    this.PingBytes[this.PingBytes.Length - 1] = this.PingId;

                    DataWriter writer;
                    writer = new DataWriter(this.sock.OutputStream);
                    writer.WriteBytes(this.PingBytes);
                    DataWriterStoreOperation res = writer.StoreAsync();
                    res.AsTask().Wait(100);

                    this.PingBytes[this.PingBytes.Length - 1] = (byte)(this.PingId + 1); // this buffer is re-used for the result/receive. invalidate the result now.

                    writer.DetachStream();
                    writer.Dispose();
                }
                else
                {
                    this.sock = null; // will cause Done() to return true but this.Successful defines if the roundtrip completed
                }
            }
        }

        private void OnMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            lock (this.syncer)
            {
                DataReader reader = null;
                try
                {
                    reader = args.GetDataReader();
                    uint receivedByteCount = reader.UnconsumedBufferLength;
                    if (receivedByteCount > 0)
                    {
                        byte[] resultBytes = new byte[receivedByteCount];
                        reader.ReadBytes(resultBytes);


                        this.Successful = receivedByteCount == this.PingLength && resultBytes[resultBytes.Length - 1] == this.PingId;
                        this.GotResult = true;
                    }
                }
                catch
                {
                }
            }
        }
    }
    #endif


    #if NATIVE_SOCKETS
    public abstract class PingNative : PhotonPing
    {
        protected enum NativeSocketState : byte
        {
            Disconnected = 0,
            Connecting = 1,
            Connected = 2,
            ConnectionError = 3,
            SendError = 4,
            ReceiveError = 5,
            Disconnecting = 6
        }

        protected IntPtr pConnectionHandler = IntPtr.Zero;

        ~PingNative()
        {
            Dispose();
        }
    }
    public class PingNativeDynamic : PingNative
    {
        public override bool StartPing(string ip)
        {
            lock (SocketUdpNativeDynamic.syncer)
            {
                base.Init();

                if(pConnectionHandler == IntPtr.Zero)
                {
                    pConnectionHandler = SocketUdpNativeDynamic.egconnect(ip);
                    SocketUdpNativeDynamic.egservice(pConnectionHandler);
                    byte state = SocketUdpNativeDynamic.eggetState(pConnectionHandler);
                    while (state == (byte) NativeSocketState.Connecting)
                    {
                        SocketUdpNativeDynamic.egservice(pConnectionHandler);
                        state = SocketUdpNativeDynamic.eggetState(pConnectionHandler);
                    }
                }

                PingBytes[PingBytes.Length - 1] = PingId;
                SocketUdpNativeDynamic.egsend(pConnectionHandler, PingBytes, PingBytes.Length);
                SocketUdpNativeDynamic.egservice(pConnectionHandler);

                PingBytes[PingBytes.Length - 1] = (byte) (PingId - 1);
                return true;
            }
        }

        public override bool Done()
        {
            lock (SocketUdpNativeDynamic.syncer)
            {
                if (this.GotResult || pConnectionHandler == IntPtr.Zero)
                {
                    return true;
                }

                int available = SocketUdpNativeDynamic.egservice(pConnectionHandler);
                if (available < PingLength)
                {
                    return false;
                }

                int pingBytesLength = PingBytes.Length;
                int bytesInRemainginDatagrams = SocketUdpNativeDynamic.egread(pConnectionHandler, PingBytes, ref pingBytesLength);
                this.Successful = (PingBytes != null && PingBytes[PingBytes.Length - 1] == PingId);

                this.GotResult = true;
                return true;
            }
        }

        public override void Dispose()
        {
            lock (SocketUdpNativeDynamic.syncer)
            {
                if (this.pConnectionHandler != IntPtr.Zero)
                    SocketUdpNativeDynamic.egdisconnect(this.pConnectionHandler);
                this.pConnectionHandler = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
    }

    #if NATIVE_SOCKETS && NATIVE_SOCKETS_STATIC
    public class PingNativeStatic : PingNative
    {
        public override bool StartPing(string ip)
        {
            base.Init();

            lock (SocketUdpNativeStatic.syncer)
            {
                if(pConnectionHandler == IntPtr.Zero)
                {
                    pConnectionHandler = SocketUdpNativeStatic.egconnect(ip);
                    SocketUdpNativeStatic.egservice(pConnectionHandler);
                    byte state = SocketUdpNativeStatic.eggetState(pConnectionHandler);
                    while (state == (byte) NativeSocketState.Connecting)
                    {
                        SocketUdpNativeStatic.egservice(pConnectionHandler);
                        state = SocketUdpNativeStatic.eggetState(pConnectionHandler);
                        Thread.Sleep(0); // suspending execution for a moment is critical on Switch for the OS to update the socket
                    }
                }

                PingBytes[PingBytes.Length - 1] = PingId;
                SocketUdpNativeStatic.egsend(pConnectionHandler, PingBytes, PingBytes.Length);
                SocketUdpNativeStatic.egservice(pConnectionHandler);

                PingBytes[PingBytes.Length - 1] = (byte) (PingId - 1);
                return true;
            }
        }

        public override bool Done()
        {
            lock (SocketUdpNativeStatic.syncer)
            {
                if (this.GotResult || pConnectionHandler == IntPtr.Zero)
                {
                    return true;
                }

                int available = SocketUdpNativeStatic.egservice(pConnectionHandler);
                if (available < PingLength)
                {
                    return false;
                }

                int pingBytesLength = PingBytes.Length;
                int bytesInRemainginDatagrams = SocketUdpNativeStatic.egread(pConnectionHandler, PingBytes, ref pingBytesLength);
                this.Successful = (PingBytes != null && PingBytes[PingBytes.Length - 1] == PingId);

                this.GotResult = true;
                return true;
            }
        }

        public override void Dispose()
        {
            lock (SocketUdpNativeStatic.syncer)
            {
                if (pConnectionHandler != IntPtr.Zero)
                    SocketUdpNativeStatic.egdisconnect(pConnectionHandler);
                pConnectionHandler = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
    }
    #endif
    #endif


    #if UNITY_WEBGL
    public class PingHttp : PhotonPing
    {
        private UnityWebRequest webRequest;

        public override bool StartPing(string address)
        {
            base.Init();
            string scheme = UnityEngine.Application.isEditor ? "http://" : "https://";
            address = $"{scheme}{address}/photon/m/?ping&r={UnityEngine.Random.Range(0, 10000)}";

            this.webRequest = UnityWebRequest.Get(address);
            this.webRequest.SendWebRequest();
            return true;
        }

        public override bool Done()
        {
            if (this.webRequest.isDone)
            {
                Successful = true;
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
            this.webRequest.Dispose();
        }
    }
    #endif
}