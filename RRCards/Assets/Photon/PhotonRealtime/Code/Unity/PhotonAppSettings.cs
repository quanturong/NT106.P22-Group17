
#if UNITY_2017_4_OR_NEWER
#define SUPPORTED_UNITY
#endif


#if !PHOTON_UNITY_NETWORKING

namespace Photon.Realtime
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    [Serializable]
    [HelpURL("https://doc.photonengine.com/en-us/pun/v2/getting-started/initial-setup")]
    public class PhotonAppSettings : ScriptableObject
    {
        [Tooltip("Core Photon Server/Cloud settings.")]
        public AppSettings AppSettings;

        #if UNITY_EDITOR
        [HideInInspector]
        public bool DisableAutoOpenWizard;
        #endif

        private static PhotonAppSettings instance;
        public static PhotonAppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    LoadOrCreateSettings();
                }

                return instance;
            }

            private set { instance = value; }
        }



        public static void LoadOrCreateSettings()
        {
            if (instance != null)
            {
                Debug.LogWarning("Instance is not null. Will not LoadOrCreateSettings().");
                return;
            }


            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
            instance = (PhotonAppSettings)Resources.Load(typeof(PhotonAppSettings).Name, typeof(PhotonAppSettings));
            if (instance != null)
            {
                return;
            }
            if (instance == null)
            {
                instance = (PhotonAppSettings)CreateInstance(typeof(PhotonAppSettings));
                if (instance == null)
                {
                    Debug.LogError("Failed to create ServerSettings. PUN is unable to run this way. If you deleted it from the project, reload the Editor.");
                    return;
                }
            }
            #if UNITY_EDITOR
            string punResourcesDirectory = "Assets/Photon/Resources/";
            string serverSettingsAssetPath = punResourcesDirectory + typeof(PhotonAppSettings).Name + ".asset";
            string serverSettingsDirectory = Path.GetDirectoryName(serverSettingsAssetPath);

            if (!Directory.Exists(serverSettingsDirectory))
            {
                Directory.CreateDirectory(serverSettingsDirectory);
                AssetDatabase.ImportAsset(serverSettingsDirectory);
            }

            AssetDatabase.CreateAsset(instance, serverSettingsAssetPath);
            AssetDatabase.SaveAssets();
            #endif
        }
    }
}
#endif