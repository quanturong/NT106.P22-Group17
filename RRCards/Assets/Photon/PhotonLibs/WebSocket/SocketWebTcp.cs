#if UNITY_WEBGL || WEBSOCKET || WEBSOCKET_PROXYCONFIG


namespace ExitGames.Client.Photon
{
    using System;

    #if UNITY_2019_3_OR_NEWER
    using UnityEngine.Scripting;
    #endif
    [Preserve]
    public class SocketWebTcp : IPhotonSocket, IDisposable
    {
        private WebSocket sock;

        private readonly object syncer = new object();

        [Preserve]
        public SocketWebTcp(PeerBase npeer) : base(npeer)
        {
            this.ServerAddress = npeer.ServerAddress;
            this.ProxyServerAddress = npeer.ProxyServerAddress;
            if (this.ReportDebugOfLevel(DebugLevel.INFO))
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "SocketWebTcp() "+ WebSocket.Implementation+". Server: " + this.ServerAddress + (String.IsNullOrEmpty(this.ProxyServerAddress) ? "" : ", Proxy: " + this.ProxyServerAddress));
            }

            this.PollReceive = false;
        }

        public void Dispose()
        {
            this.State = PhotonSocketState.Disconnecting;

            if (this.sock != null)
            {
                try
                {
                    if (this.sock.Connected)
                    {
                        this.sock.Close();
                    }
                }
                catch (Exception ex)
                {
                    this.EnqueueDebugReturn(DebugLevel.INFO, "Exception in SocketWebTcp.Dispose(): " + ex);
                }
            }

            this.sock = null;
            this.State = PhotonSocketState.Disconnected;
        }


        public override bool Connect()
        {
            this.State = PhotonSocketState.Connecting;


            if (!this.ConnectAddress.Contains("IPv6"))
            {
                this.ConnectAddress += "&IPv6"; // this makes the Photon Server return a host name for the next server (NS points to MS and MS points to GS)
            }

            string proxyServerAddress;
            if (!this.ReadProxyConfigScheme(this.ProxyServerAddress, this.ServerAddress, out proxyServerAddress))
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "ReadProxyConfigScheme() failed. Using no proxy.");
            }

            try
            {
                this.sock = new WebSocket(new Uri(this.ConnectAddress), proxyServerAddress, this.OpenCallback, this.ReceiveCallback, this.ErrorCallback, this.CloseCallback, this.SerializationProtocol);
                this.sock.DebugReturn = (DebugLevel l, string s) =>
                                        {
                                            if (this.State != PhotonSocketState.Disconnected)
                                            {
                                                this.Listener.DebugReturn(l, this.State + " " + s);
                                            }
                                        };

                this.sock.Connect();
                return true;
            }
            catch (Exception e)
            {
                this.Listener.DebugReturn(DebugLevel.ERROR, "SocketWebTcp.Connect() caught exception: " + e);
                return false;
            }
        }

        private void CloseCallback(int code, string reason)
        {
            if (this.State == PhotonSocketState.Connecting)
            {
                this.HandleException(StatusCode.ExceptionOnConnect); // sets state to Disconnecting
                return;
            }
            if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
            {
                this.Listener.DebugReturn(DebugLevel.ERROR, "SocketWebTcp.CloseCallback(). Going to disconnect. Server: " + this.ServerAddress + " Error: " + code + " Reason: " + reason);
                this.HandleException(StatusCode.DisconnectByServerReasonUnknown); // sets state to Disconnecting
            }
        }
        private void ErrorCallback(int code, string message)
        {
            if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
            {
                this.Listener.DebugReturn(DebugLevel.ERROR, "SocketWebTcp.ErrorCallback(). Going to disconnect. Server: " + this.ServerAddress + " Error: " + code + " Message: " + message);
                this.HandleException(this.State != PhotonSocketState.Connected ? StatusCode.ExceptionOnConnect : StatusCode.ExceptionOnReceive); // sets state to Disconnecting
            }
        }

        private void OpenCallback()
        {
            if (State == PhotonSocketState.Connecting)
            {
                this.State = PhotonSocketState.Connected;
                this.peerBase.OnConnect();
            }
        }
        private bool ReadProxyConfigScheme(string proxyAddress, string url, out string proxyUrl)
        {
            proxyUrl = null;

            #if !WEBSOCKET_PROXYCONFIG

            if (!string.IsNullOrEmpty(proxyAddress))
            {
                if (proxyAddress.StartsWith("auto:") || proxyAddress.StartsWith("pac:") || proxyAddress.StartsWith("system:"))
                {
                    this.Listener.DebugReturn(DebugLevel.WARNING, "Proxy configuration via auto, pac or system is only supported with the WEBSOCKET_PROXYCONFIG define. Using no proxy instead.");
                    return true;
                }
                proxyUrl = proxyAddress;
            }

            return true;

            #else

            if (!string.IsNullOrEmpty(proxyAddress))
            {
                var httpUrl = url.ToString().Replace("ws://", "http://").Replace("wss://", "https://"); // http(s) schema required in GetProxyForUrlUsingPac call
                bool auto = proxyAddress.StartsWith("auto:", StringComparison.InvariantCultureIgnoreCase);
                bool pac = proxyAddress.StartsWith("pac:", StringComparison.InvariantCultureIgnoreCase);

                if (auto || pac)
                {
                    string pacUrl = "";
                    if (pac)
                    {
                        pacUrl = proxyAddress.Substring(4);
                        if (pacUrl.IndexOf("://") == -1)
                        {
                            pacUrl = "http://" + pacUrl; //default to http
                        }
                    }

                    string processTypeStr = auto ? "auto detect" : "pac url " + pacUrl;

                    this.Listener.DebugReturn(DebugLevel.INFO, "WebSocket Proxy: " + url + " " + processTypeStr);

                    string errDescr = "";
                    var err = ProxyAutoConfig.GetProxyForUrlUsingPac(httpUrl, pacUrl, out proxyUrl, out errDescr);

                    if (err != 0)
                    {
                        this.Listener.DebugReturn(DebugLevel.ERROR, "WebSocket Proxy: " + url + " " + processTypeStr + " ProxyAutoConfig.GetProxyForUrlUsingPac() error: " + err + " (" + errDescr + ")");
                        return false;
                    }
                }
                else if (proxyAddress.StartsWith("system:", StringComparison.InvariantCultureIgnoreCase))
                {
                    this.Listener.DebugReturn(DebugLevel.INFO, "WebSocket Proxy: " + url + " system settings");
                    string proxyAutoConfigPacUrl;
                    var err = ProxySystemSettings.GetProxy(out proxyUrl, out proxyAutoConfigPacUrl);
                    if (err != 0)
                    {
                        this.Listener.DebugReturn(DebugLevel.ERROR, "WebSocket Proxy: " + url + " system settings ProxySystemSettings.GetProxy() error: " + err);
                        return false;
                    }
                    if (proxyAutoConfigPacUrl != null)
                    {
                        if (proxyAutoConfigPacUrl.IndexOf("://") == -1)
                        {
                            proxyAutoConfigPacUrl = "http://" + proxyAutoConfigPacUrl; //default to http
                        }
                        this.Listener.DebugReturn(DebugLevel.INFO, "WebSocket Proxy: " + url + " system settings AutoConfigURL: " + proxyAutoConfigPacUrl);
                        string errDescr = "";
                        err = ProxyAutoConfig.GetProxyForUrlUsingPac(httpUrl, proxyAutoConfigPacUrl, out proxyUrl, out errDescr);

                        if (err != 0)
                        {
                            this.Listener.DebugReturn(DebugLevel.ERROR, "WebSocket Proxy: " + url + " system settings AutoConfigURLerror: " + err + " (" + errDescr + ")");
                            return false;
                        }
                    }
                }
                else
                {
                    proxyUrl = proxyAddress;
                }

                this.Listener.DebugReturn(DebugLevel.INFO, "WebSocket Proxy: " + url + " -> " + (string.IsNullOrEmpty(proxyUrl) ? "DIRECT" : "PROXY " + proxyUrl));
            }

            return true;
            #endif
        }


        public override bool Disconnect()
        {
            if (this.ReportDebugOfLevel(DebugLevel.INFO))
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "SocketWebTcp.Disconnect()");
            }

            this.State = PhotonSocketState.Disconnecting;

            lock (this.syncer)
            {
                if (this.sock != null)
                {
                    try
                    {
                        this.sock.Close();
                    }
                    catch (Exception ex)
                    {
                        this.Listener.DebugReturn(DebugLevel.ERROR, "Exception in SocketWebTcp.Disconnect(): " + ex);
                    }

                    this.sock = null;
                }
            }

            this.State = PhotonSocketState.Disconnected;
            return true;
        }
        public override PhotonSocketError Send(byte[] data, int length)
        {
            if (this.State != PhotonSocketState.Connected)
            {
                return PhotonSocketError.Skipped;
            }

            try
            {
                if (data.Length > length)
                {
                    byte[] trimmedData = new byte[length];
                    Buffer.BlockCopy(data, 0, trimmedData, 0, length);
                    data = trimmedData;
                }

                if (this.sock != null)
                {
                    this.sock.Send(data);
                }
            }
            catch (Exception e)
            {
                this.Listener.DebugReturn(DebugLevel.ERROR, "Cannot send to: " + this.ServerAddress + ". " + e.Message);

                this.HandleException(StatusCode.Exception);
                return PhotonSocketError.Exception;
            }

            return PhotonSocketError.Success;
        }


        public override PhotonSocketError Receive(out byte[] data)
        {
            data = null;
            return PhotonSocketError.NoData;
        }

        public void ReceiveCallback(byte[] buf, int len)
        {
            if (State == PhotonSocketState.Disconnecting || State == PhotonSocketState.Disconnected)
            {
                return;
            }

            try
            {
                this.HandleReceivedDatagram(buf, len, false);
            }
            catch (Exception e)
            {
                if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                {
                    if (this.ReportDebugOfLevel(DebugLevel.ERROR))
                    {
                        this.EnqueueDebugReturn(DebugLevel.ERROR, "SocketWebTcp.ReceiveCallback() caught exception. Going to disconnect. State: " + this.State + ". Server: '" + this.ServerAddress + "' Exception: " + e);
                    }

                    this.HandleException(StatusCode.ExceptionOnReceive);
                }
            }
        }
    }
}

#endif