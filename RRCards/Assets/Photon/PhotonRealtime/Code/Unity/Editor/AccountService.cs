
#if UNITY_2017_4_OR_NEWER
#define SUPPORTED_UNITY
#endif


#if UNITY_EDITOR

namespace Photon.Realtime
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using ExitGames.Client.Photon;
    public class AccountService
    {
        private const string ServiceUrl = "https://partner.photonengine.com/api/{0}/User/RegisterEx";

        private readonly Dictionary<string, string> RequestHeaders = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-functions-key", "" }
        };

        private const string DefaultContext = "Unity";

        private const string DefaultToken = "VQ920wVUieLHT9c3v1ZCbytaLXpXbktUztKb3iYLCdiRKjUagcl6eg==";
        public string CustomContext = null;     // "PartnerCode" on the server
        public string CustomToken = null;
        public bool RequestPendingResult = false;
        public bool RegisterByEmail(string email, List<ServiceTypes> serviceTypes, Action<AccountServiceResponse> callback = null, Action<string> errorCallback = null, string origin = null)
        {
            if (this.RequestPendingResult)
            {
                Debug.LogError("Registration request pending result. Not sending another.");
                return false;
            }

            if (!IsValidEmail(email))
            {
                Debug.LogErrorFormat("Email \"{0}\" is not valid", email);
                return false;
            }

            string serviceTypeString = GetServiceTypesFromList(serviceTypes);
            if (string.IsNullOrEmpty(serviceTypeString))
            {
                Debug.LogError("serviceTypes string is null or empty");
                return false;
            }

            string fullUrl = GetUrlWithQueryStringEscaped(email, serviceTypeString, origin);

            RequestHeaders["x-functions-key"] = string.IsNullOrEmpty(CustomToken) ? DefaultToken : CustomToken;


            this.RequestPendingResult = true;

            PhotonEditorUtils.StartCoroutine(
                PhotonEditorUtils.HttpPost(fullUrl,
                    RequestHeaders,
                    null,
                    s =>
                    {
                        this.RequestPendingResult = false;
                        if (string.IsNullOrEmpty(s))
                        {
                            if (errorCallback != null)
                            {
                                errorCallback("Server's response was empty. Please register through account website during this service interruption.");
                            }
                        }
                        else
                        {
                            AccountServiceResponse ase = this.ParseResult(s);
                            if (ase == null)
                            {
                                if (errorCallback != null)
                                {
                                    errorCallback("Error parsing registration response. Please try registering from account website");
                                }
                            }
                            else if (callback != null)
                            {
                                callback(ase);
                            }
                        }
                    },
                    e =>
                    {
                        this.RequestPendingResult = false;
                        if (errorCallback != null)
                        {
                            errorCallback(e);
                        }
                    })
            );
            return true;
        }


        private string GetUrlWithQueryStringEscaped(string email, string serviceTypes, string originAv)
        {
            string emailEscaped = UnityEngine.Networking.UnityWebRequest.EscapeURL(email);
            string st = UnityEngine.Networking.UnityWebRequest.EscapeURL(serviceTypes);
            string uv = UnityEngine.Networking.UnityWebRequest.EscapeURL(Application.unityVersion);
            string serviceUrl = string.Format(ServiceUrl, string.IsNullOrEmpty(CustomContext) ? DefaultContext : CustomContext );

            return string.Format("{0}?email={1}&st={2}&uv={3}&av={4}", serviceUrl, emailEscaped, st, uv, originAv);
        }
        private AccountServiceResponse ParseResult(string result)
        {
            try
            {
                AccountServiceResponse res = JsonUtility.FromJson<AccountServiceResponse>(result);
                if (res.ReturnCode == AccountServiceReturnCodes.Success)
                {
                    string[] parts = result.Split(new[] { "\"ApplicationIds\":{" }, StringSplitOptions.RemoveEmptyEntries);
                    parts = parts[1].Split('}');
                    string applicationIds = parts[0];
                    if (!string.IsNullOrEmpty(applicationIds))
                    {
                        parts = applicationIds.Split(new[] { ',', '"', ':' }, StringSplitOptions.RemoveEmptyEntries);
                        res.ApplicationIds = new Dictionary<string, string>(parts.Length / 2);
                        for (int i = 0; i < parts.Length; i = i + 2)
                        {
                            res.ApplicationIds.Add(parts[i], parts[i + 1]);
                        }
                    }
                    else
                    {
                        Debug.LogError("The server did not return any AppId, ApplicationIds was empty in the response.");
                        return null;
                    }
                }
                return res;
            }
            catch (Exception ex) // probably JSON parsing exception, check if returned string is valid JSON
            {
                Debug.LogException(ex);
                return null;
            }
        }
        private static string GetServiceTypesFromList(List<ServiceTypes> appTypes)
        {
            if (appTypes == null || appTypes.Count <= 0)
            {
                return null;
            }

            string serviceTypes = ((int)appTypes[0]).ToString();
            for (int i = 1; i < appTypes.Count; i++)
            {
                int appType = (int)appTypes[i];
                serviceTypes = string.Format("{0},{1}", serviceTypes, appType);
            }

            return serviceTypes;
        }
        private static Regex reg = new Regex("^((?>[a-zA-Z\\d!#$%&'*+\\-/=?^_{|}~]+\\x20*|\"((?=[\\x01-\\x7f])[^\"\\]|\\[\\x01-\\x7f])*\"\\x20*)*(?<angle><))?((?!\\.)(?>\\.?[a-zA-Z\\d!#$%&'*+\\-/=?^_{|}~]+)+|\"((?=[\\x01-\\x7f])[^\"\\]|\\[\\x01-\\x7f])*\")@(((?!-)[a-zA-Z\\d\\-]+(?<!-)\\.)+[a-zA-Z]{2,}|\\[(((?(?<!\\[)\\.)(25[0-5]|2[0-4]\\d|[01]?\\d?\\d)){4}|[a-zA-Z\\d\\-]*[a-zA-Z\\d]:((?=[\\x01-\\x7f])[^\\\\[\\]]|\\[\\x01-\\x7f])+)\\])(?(angle)>)$",
             RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        public static bool IsValidEmail(string mailAddress)
        {
            if (string.IsNullOrEmpty(mailAddress))
            {
                return false;
            }
            var result = reg.Match(mailAddress);
            return result.Success;
        }
    }

    [Serializable]
    public class AccountServiceResponse
    {
        public int ReturnCode;
        public string Message;
        public Dictionary<string, string> ApplicationIds; // Unity's JsonUtility does not support deserializing Dictionary
    }


    public class AccountServiceReturnCodes
    {
        public static int Success = 0;
        public static int EmailAlreadyRegistered = 8;
        public static int InvalidParameters = 12;
    }

    public enum ServiceTypes
    {
        Realtime = 0,
        Turnbased = 1,
        Chat = 2,
        Voice = 3,
        TrueSync = 4,
        Pun = 5,
        Thunder = 6,
        Quantum = 7,
        Fusion = 8,
        Bolt = 20
    }
}

#endif