#if !DISABLE_PLAYFABENTITY_API && !DISABLE_PLAYFABCLIENT_API
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFab.Public
{
    public interface IScreenTimeTracker
    {
        void OnEnable();
        void OnDisable();
        void OnDestroy();
        void OnApplicationQuit();
        void OnApplicationFocus(bool isFocused);
        void ClientSessionStart(string entityId, string entityType, string playFabUserId);
        void Send();
    }
    public class ScreenTimeTracker : IScreenTimeTracker
    {
        private Guid focusId;
        private Guid gameSessionID = Guid.NewGuid();
        private bool initialFocus = true;
        private bool isSending = false;
        private DateTime focusOffDateTime = DateTime.UtcNow;
        private DateTime focusOnDateTime = DateTime.UtcNow;

        private Queue<EventsModels.EventContents> eventsRequests = new Queue<EventsModels.EventContents>();

        private EventsModels.EntityKey entityKey = new EventsModels.EntityKey();
        private const string eventNamespace = "com.playfab.events.sessions";
        private const int maxBatchSizeInEvents = 10;

        private PlayFabEventsInstanceAPI eventApi;

        public ScreenTimeTracker()
        {
            eventApi = new PlayFabEventsInstanceAPI(PlayFabSettings.staticPlayer);
        }

        private void EnsureSingleGameSessionId()
        {
            if (gameSessionID == Guid.Empty)
            {
                gameSessionID = Guid.NewGuid();
            }
        }
        public void ClientSessionStart(string entityId, string entityType, string playFabUserId)
        {
            EnsureSingleGameSessionId();

            entityKey.Id = entityId;
            entityKey.Type = entityType;

            EventsModels.EventContents eventInfo = new EventsModels.EventContents();

            eventInfo.Name = "client_session_start";
            eventInfo.EventNamespace = eventNamespace;
            eventInfo.Entity = entityKey;
            eventInfo.OriginalTimestamp = DateTime.UtcNow;

            var payload = new Dictionary<string, object>
                {
                    { "UserID", playFabUserId},
                    { "DeviceType", SystemInfo.deviceType},
                    { "DeviceModel", SystemInfo.deviceModel},
                    { "OS", SystemInfo.operatingSystem },
                    { "ClientSessionID", gameSessionID },
                };

            eventInfo.Payload = payload;
            eventsRequests.Enqueue(eventInfo);
            OnApplicationFocus(true);
        }
        public void OnApplicationFocus(bool isFocused)
        {
            EnsureSingleGameSessionId();
            EventsModels.EventContents eventInfo = new EventsModels.EventContents();
            DateTime currentUtcDateTime = DateTime.UtcNow;

            eventInfo.Name = "client_focus_change";
            eventInfo.EventNamespace = eventNamespace;
            eventInfo.Entity = entityKey;

            double focusStateDuration = 0.0;

            if (initialFocus)
            {
                focusId = Guid.NewGuid();
            }

            if (isFocused)
            {
                focusOnDateTime = currentUtcDateTime;
                focusId = Guid.NewGuid();

                if (!initialFocus)
                {
                    focusStateDuration = (currentUtcDateTime - focusOffDateTime).TotalSeconds;
                    if (focusStateDuration < 0)
                    {
                        focusStateDuration = 0;
                    }
                }
            }
            else
            {
                focusStateDuration = (currentUtcDateTime - focusOnDateTime).TotalSeconds;
                if (focusStateDuration < 0)
                {
                    focusStateDuration = 0;
                }
                focusOffDateTime = currentUtcDateTime;
            }

            var payload = new Dictionary<string, object> {
                    { "FocusID", focusId },
                    { "FocusState", isFocused },
                    { "FocusStateDuration", focusStateDuration },
                    { "EventTimestamp", currentUtcDateTime },
                    { "ClientSessionID", gameSessionID },
                };

            eventInfo.OriginalTimestamp = currentUtcDateTime;
            eventInfo.Payload = payload;
            eventsRequests.Enqueue(eventInfo);

            initialFocus = false;

            if (!isFocused)
            {
                Send();
            }

        }
        public void Send()
        {
            if (PlayFabSettings.staticPlayer.IsClientLoggedIn() && (isSending == false))
            {
                isSending = true;

                EventsModels.WriteEventsRequest request = new EventsModels.WriteEventsRequest();
                request.Events = new List<EventsModels.EventContents>();

                while ((eventsRequests.Count > 0) && (request.Events.Count < maxBatchSizeInEvents))
                {
                    EventsModels.EventContents eventInfo = eventsRequests.Dequeue();
                    request.Events.Add(eventInfo);
                }

                if (request.Events.Count > 0)
                {
                    eventApi.WriteEvents(request, EventSentSuccessfulCallback, EventSentErrorCallback);
                }

                isSending = false;
            }
        }
        private void EventSentSuccessfulCallback(EventsModels.WriteEventsResponse response)
        {
        }
        private void EventSentErrorCallback(PlayFabError response)
        {
            Debug.LogWarning("Failed to send session data. Error: " + response.GenerateErrorReport());
        }

        #region Unused MonoBehaviour compatibility  methods
        public void OnEnable()
        {
        }
        public void OnDisable()
        {
        }
        public void OnDestroy()
        {
        }
        #endregion
        public void OnApplicationQuit()
        {
            Send();
        }
    }
}
#endif
