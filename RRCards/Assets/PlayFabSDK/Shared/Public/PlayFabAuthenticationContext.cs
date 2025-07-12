namespace PlayFab
{
    public sealed class PlayFabAuthenticationContext
    {
        public PlayFabAuthenticationContext()
        {
        }

        public PlayFabAuthenticationContext(string clientSessionTicket, string entityToken, string playFabId, string entityId, string entityType, string telemetryKey = null) : this()
        {
#if !DISABLE_PLAYFABCLIENT_API
            ClientSessionTicket = clientSessionTicket;
            PlayFabId = playFabId;
#endif
#if !DISABLE_PLAYFABENTITY_API
            EntityToken = entityToken;
            EntityId = entityId;
            EntityType = entityType;
#endif
            TelemetryKey = telemetryKey;
        }

        public void CopyFrom(PlayFabAuthenticationContext other)
        {
#if !DISABLE_PLAYFABCLIENT_API
            ClientSessionTicket = other.ClientSessionTicket;
            PlayFabId = other.PlayFabId;
#endif
#if !DISABLE_PLAYFABENTITY_API
            EntityToken = other.EntityToken;
            EntityId = other.EntityId;
            EntityType = other.EntityType;
#endif
            TelemetryKey = other.TelemetryKey;
        }

#if !DISABLE_PLAYFABCLIENT_API
        public string ClientSessionTicket;
        public string PlayFabId;
        public bool IsClientLoggedIn()
        {
            return !string.IsNullOrEmpty(ClientSessionTicket);
        }
#endif

#if !DISABLE_PLAYFABENTITY_API
        public string EntityToken;
        public string EntityId;
        public string EntityType;
        public bool IsEntityLoggedIn()
        {
            return !string.IsNullOrEmpty(EntityToken);
        }
#endif

        public string TelemetryKey;

        public bool IsTelemetryKeyProvided()
        {
            return !string.IsNullOrEmpty(TelemetryKey);
        }

        public void ForgetAllCredentials()
        {
#if !DISABLE_PLAYFABCLIENT_API
            PlayFabId = null;
            ClientSessionTicket = null;
#endif
#if !DISABLE_PLAYFABENTITY_API
            EntityToken = null;
            EntityId = null;
            EntityType = null;
#endif
            TelemetryKey = null;
        }
    }
}
