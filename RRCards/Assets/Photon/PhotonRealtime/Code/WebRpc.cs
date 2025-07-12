
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System.Collections.Generic;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class WebRpcResponse
    {
        public string Name { get; private set; }
        public int ResultCode { get; private set; }
        [System.Obsolete("Use ResultCode instead")]
        public int ReturnCode
        {
            get { return ResultCode; }
        }
        public string Message { get; private set; }
        [System.Obsolete("Use Message instead")]
        public string DebugMessage
        {
            get { return Message; }
        }
        public Dictionary<string, object> Parameters { get; private set; }
        public WebRpcResponse(OperationResponse response)
        {
            object value;
            if (response.Parameters.TryGetValue(ParameterCode.UriPath, out value))
            {
                this.Name = value as string;
            }

            this.ResultCode = -1;
            if (response.Parameters.TryGetValue(ParameterCode.WebRpcReturnCode, out value))
            {
                this.ResultCode = (byte)value;
            }

            if (response.Parameters.TryGetValue(ParameterCode.WebRpcParameters, out value))
            {
                this.Parameters = value as Dictionary<string, object>;
            }

            if (response.Parameters.TryGetValue(ParameterCode.WebRpcReturnMessage, out value))
            {
                this.Message = value as string;
            }
        }
        public string ToStringFull()
        {
            return string.Format("{0}={2}: {1} \"{3}\"", this.Name, SupportClass.DictionaryToString(this.Parameters), this.ResultCode, this.Message);
        }
    }
    public class WebFlags
    {

        public readonly static WebFlags Default = new WebFlags(0);
        public byte WebhookFlags;
        public bool HttpForward
        {
            get { return (WebhookFlags & HttpForwardConst) != 0; }
            set {
                if (value)
                {
                    WebhookFlags |= HttpForwardConst;
                }
                else
                {
                    WebhookFlags = (byte) (WebhookFlags & ~(1 << 0));
                }
            }
        }
        public const byte HttpForwardConst = 0x01;
        public bool SendAuthCookie
        {
            get { return (WebhookFlags & SendAuthCookieConst) != 0; }
            set {
                if (value)
                {
                    WebhookFlags |= SendAuthCookieConst;
                }
                else
                {
                    WebhookFlags = (byte)(WebhookFlags & ~(1 << 1));
                }
            }
        }
        public const byte SendAuthCookieConst = 0x02;
        public bool SendSync
        {
            get { return (WebhookFlags & SendSyncConst) != 0; }
            set {
                if (value)
                {
                    WebhookFlags |= SendSyncConst;
                }
                else
                {
                    WebhookFlags = (byte)(WebhookFlags & ~(1 << 2));
                }
            }
        }
        public const byte SendSyncConst = 0x04;
        public bool SendState
        {
            get { return (WebhookFlags & SendStateConst) != 0; }
            set {
                if (value)
                {
                    WebhookFlags |= SendStateConst;
                }
                else
                {
                    WebhookFlags = (byte)(WebhookFlags & ~(1 << 3));
                }
            }
        }
        public const byte SendStateConst = 0x08;

        public WebFlags(byte webhookFlags)
        {
            WebhookFlags = webhookFlags;
        }
    }

}
