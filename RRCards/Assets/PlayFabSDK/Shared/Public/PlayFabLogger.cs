using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using PlayFab.Internal;
using UnityEngine;

namespace PlayFab.Public
{
#if !UNITY_WSA && !UNITY_WP8 && !NETFX_CORE
    public interface IPlayFabLogger
    {
        IPAddress ip { get; set; }
        int port { get; set; }
        string url { get; set; }
        void OnEnable();
        void OnDisable();
        void OnDestroy();
    }
    public abstract class PlayFabLoggerBase : IPlayFabLogger
    {
        private static readonly StringBuilder Sb = new StringBuilder();
        private readonly Queue<string> LogMessageQueue = new Queue<string>();
        private const int LOG_CACHE_INTERVAL_MS = 10000;

        private Thread _writeLogThread;
        private readonly object _threadLock = new object();
        private static readonly TimeSpan _threadKillTimeout = TimeSpan.FromSeconds(60);
        private DateTime _threadKillTime = DateTime.UtcNow + _threadKillTimeout; // Kill the thread after 1 minute of inactivity
        private bool _isApplicationPlaying = true;
        private int _pendingLogsCount;

        public IPAddress ip { get; set; }
        public int port { get; set; }
        public string url { get; set; }

        protected PlayFabLoggerBase()
        {
            var gatherer = new PlayFabDataGatherer();
            var message = gatherer.GenerateReport();
            lock (LogMessageQueue)
            {
                LogMessageQueue.Enqueue(message);
            }
        }

        public virtual void OnEnable()
        {
            PlayFabHttp.instance.StartCoroutine(RegisterLogger()); // Coroutine helper to set up log-callbacks
        }

        private IEnumerator RegisterLogger()
        {
            yield return new WaitForEndOfFrame(); // Effectively just a short wait before activating this registration
            if (!string.IsNullOrEmpty(PlayFabSettings.LoggerHost))
            {
#if UNITY_5 || UNITY_5_3_OR_NEWER
                Application.logMessageReceivedThreaded += HandleUnityLog;
#else
                Application.RegisterLogCallback(HandleUnityLog);
#endif
            }
        }

        public virtual void OnDisable()
        {
            if (!string.IsNullOrEmpty(PlayFabSettings.LoggerHost))
            {
#if UNITY_5 || UNITY_5_3_OR_NEWER
                Application.logMessageReceivedThreaded -= HandleUnityLog;
#else
                Application.RegisterLogCallback(null);
#endif
            }
        }

        public virtual void OnDestroy()
        {
            _isApplicationPlaying = false;
        }
        protected abstract void BeginUploadLog();
        protected abstract void UploadLog(string message);
        protected abstract void EndUploadLog();
        private void HandleUnityLog(string message, string stacktrace, LogType type)
        {
            if (!PlayFabSettings.EnableRealTimeLogging)
                return;

            Sb.Length = 0;
            if (type == LogType.Log || type == LogType.Warning)
            {
                Sb.Append(type).Append(": ").Append(message);
                message = Sb.ToString();
                lock (LogMessageQueue)
                {
                    LogMessageQueue.Enqueue(message);
                }
            }
            else if (type == LogType.Error || type == LogType.Exception)
            {
                Sb.Append(type).Append(": ").Append(message).Append("\n").Append(stacktrace).Append(StackTraceUtility.ExtractStackTrace());
                message = Sb.ToString();
                lock (LogMessageQueue)
                {
                    LogMessageQueue.Enqueue(message);
                }
            }
            ActivateThreadWorker();
        }

        private void ActivateThreadWorker()
        {
            lock (_threadLock)
            {
                if (_writeLogThread != null)
                {
                    return;
                }
                _writeLogThread = new Thread(WriteLogThreadWorker);
                _writeLogThread.Start();
            }
        }

        private void WriteLogThreadWorker()
        {
            try
            {
                bool active;
                lock (_threadLock)
                {
                    _threadKillTime = DateTime.UtcNow + _threadKillTimeout;
                }

                var localLogQueue = new Queue<string>();
                do
                {
                    lock (LogMessageQueue)
                    {
                        _pendingLogsCount = LogMessageQueue.Count;
                        while (LogMessageQueue.Count > 0) // Transfer the messages to the local queue
                            localLogQueue.Enqueue(LogMessageQueue.Dequeue());
                    }

                    BeginUploadLog();
                    while (localLogQueue.Count > 0) // Transfer the messages to the local queue
                        UploadLog(localLogQueue.Dequeue());
                    EndUploadLog();

                    #region Expire Thread.
                    lock (_threadLock)
                    {
                        var now = DateTime.UtcNow;
                        if (_pendingLogsCount > 0 && _isApplicationPlaying)
                        {
                            _threadKillTime = now + _threadKillTimeout;
                        }
                        active = now <= _threadKillTime;
                        if (!active)
                        {
                            _writeLogThread = null;
                        }
                    }
                    #endregion

                    Thread.Sleep(LOG_CACHE_INTERVAL_MS);
                } while (active);

            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _writeLogThread = null;
            }
        }
    }
#else
    public interface IPlayFabLogger
    {
        string ip { get; set; }
        int port { get; set; }
        string url { get; set; }
        void OnEnable();
        void OnDisable();
        void OnDestroy();
    }
    public abstract class PlayFabLoggerBase : IPlayFabLogger
    {
        public string ip { get; set; }
        public int port { get; set; }
        public string url { get; set; }
        public void OnEnable() { }
        public void OnDisable() { }
        public void OnDestroy() { }

        protected abstract void BeginUploadLog();
        protected abstract void UploadLog(string message);
        protected abstract void EndUploadLog();
    }
#endif
    public class PlayFabLogger : PlayFabLoggerBase
    {
        protected override void BeginUploadLog()
        {
        }
        protected override void UploadLog(string message)
        {
        }
        protected override void EndUploadLog()
        {
        }
    }
}
