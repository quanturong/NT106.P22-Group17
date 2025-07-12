using System;
using System.Collections.Concurrent;
using PlayFab.Internal;

namespace PlayFab
{
    public class PluginManager
    {
        private ConcurrentDictionary<PluginContractKey, IPlayFabPlugin> plugins = new ConcurrentDictionary<PluginContractKey, IPlayFabPlugin>(new PluginContractKeyComparator());
        private static readonly PluginManager Instance = new PluginManager();

        private PluginManager()
        {
        }
        public static T GetPlugin<T>(PluginContract contract, string instanceName = "") where T : IPlayFabPlugin
        {
            return (T)Instance.GetPluginInternal(contract, instanceName);
        }
        public static void SetPlugin(IPlayFabPlugin plugin, PluginContract contract, string instanceName = "")
        {
            Instance.SetPluginInternal(plugin, contract, instanceName);
        }

        private IPlayFabPlugin GetPluginInternal(PluginContract contract, string instanceName)
        {
            var key = new PluginContractKey { _pluginContract = contract, _pluginName = instanceName };
            IPlayFabPlugin plugin;
            if (!this.plugins.TryGetValue(key, out plugin))
            {
                switch (contract)
                {
                    case PluginContract.PlayFab_Serializer:
                        plugin = this.CreatePlugin<PlayFab.Json.SimpleJsonInstance>();
                        break;
                    case PluginContract.PlayFab_Transport:
                        plugin = this.CreatePlayFabTransportPlugin();
                        break;
                    default:
                        throw new ArgumentException("This contract is not supported", "contract");
                }

                this.plugins[key] = plugin;
            }

            return plugin;
        }

        private void SetPluginInternal(IPlayFabPlugin plugin, PluginContract contract, string instanceName)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException("plugin", "Plugin instance cannot be null");
            }

            var key = new PluginContractKey { _pluginContract = contract, _pluginName = instanceName };
            this.plugins[key] = plugin;
        }

        private IPlayFabPlugin CreatePlugin<T>() where T : IPlayFabPlugin, new()
        {
            return (IPlayFabPlugin)System.Activator.CreateInstance(typeof(T));
        }

        private ITransportPlugin CreatePlayFabTransportPlugin()
        {
            ITransportPlugin transport = null;
#if !UNITY_WSA && !UNITY_WP8
            if (PlayFabSettings.RequestType == WebRequestType.HttpWebRequest)
                transport = new PlayFabWebRequest();
#endif

#if UNITY_2018_2_OR_NEWER // PlayFabWww will throw warnings as Unity has deprecated Www
            if (transport == null)
                transport = new PlayFabUnityHttp();
#elif UNITY_2017_2_OR_NEWER
            if (PlayFabSettings.RequestType == WebRequestType.UnityWww)
                transport = new PlayFabWww();

            if (transport == null)
                transport = new PlayFabUnityHttp();
#else
            if (transport == null)
                transport = new PlayFabWww();
#endif

            return transport;
        }
    }
}
