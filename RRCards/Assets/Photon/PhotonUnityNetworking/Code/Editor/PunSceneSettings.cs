

using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;

namespace Photon.Pun
{
    [Serializable]
    public class SceneSetting
    {
        public SceneAsset sceneAsset;
        public string sceneName;
        public int minViewId;
    }

    [HelpURL("https://doc.photonengine.com/en-us/pun/current/getting-started/feature-overview#scene_photonviews_in_multiple_scenes")]
    public class PunSceneSettings : ScriptableObject
    {

        #if UNITY_EDITOR
        #pragma warning disable 0414
        [SerializeField]
        bool SceneSettingsListFoldoutOpen = true;
        #pragma warning restore 0414
        #endif
        
        [SerializeField]
        public List<SceneSetting> MinViewIdPerScene = new List<SceneSetting>();

      
        private const string SceneSettingsFileName = "PunSceneSettingsFile.asset";
        private static string punSceneSettingsCsPath;

        public static string PunSceneSettingsCsPath
        {
            get
            {
                if (!string.IsNullOrEmpty(punSceneSettingsCsPath))
                {
                    return punSceneSettingsCsPath;
                }
                var result = Directory.GetFiles(Application.dataPath, "PunSceneSettings.cs", SearchOption.AllDirectories);
                if (result.Length >= 1)
                {
                    punSceneSettingsCsPath = Path.GetDirectoryName(result[0]);
                    punSceneSettingsCsPath = punSceneSettingsCsPath.Replace('\\', '/');
                    punSceneSettingsCsPath = punSceneSettingsCsPath.Replace(Application.dataPath, "Assets");
                    punSceneSettingsCsPath = punSceneSettingsCsPath + "/" + SceneSettingsFileName;
                }

                return punSceneSettingsCsPath;
            }
        }


        private static PunSceneSettings instanceField;

        public static PunSceneSettings Instance
        {
            get
            {
                if (instanceField != null)
                {
                    return instanceField;
                }

                instanceField = (PunSceneSettings)AssetDatabase.LoadAssetAtPath(PunSceneSettingsCsPath, typeof(PunSceneSettings));
                if (instanceField == null)
                {
                    instanceField = CreateInstance<PunSceneSettings>();
                    #pragma warning disable 0168
                    try
                    {
                        AssetDatabase.CreateAsset(instanceField, PunSceneSettingsCsPath);
                    }
                    catch (Exception e)
                    {
                        #if PHOTON_UNITY_NETWORKING
                        Debug.LogError("-- WARNING: PROJECT CLEANUP NECESSARY -- If you delete pun from your project, make sure you also clean up the Scripting define symbols from any reference to PUN like 'PHOTON_UNITY_NETWORKING ");
                        #endif
                    }
                    #pragma warning restore 0168
                }

                return instanceField;
            }
        }


        public static int MinViewIdForScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return 1;
            }

            PunSceneSettings pss = Instance;
            if (pss == null)
            {
                Debug.LogError("pss cant be null");
                return 1;
            }

            foreach (SceneSetting setting in pss.MinViewIdPerScene)
            {
                if (setting.sceneName.Equals(sceneName))
                {
                    return setting.minViewId;
                }
            }
            return 1;
        }

        public static void SanitizeSceneSettings()
        {
            if (Instance == null)
            {
                return;
            }
            
            #if UNITY_EDITOR
            foreach (SceneSetting sceneSetting in Instance.MinViewIdPerScene)
            {
                if (sceneSetting.sceneAsset == null && !string.IsNullOrEmpty(sceneSetting.sceneName))
                {
                    
                    string[] guids = AssetDatabase.FindAssets(sceneSetting.sceneName + " t:SceneAsset");

                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        if (Path.GetFileNameWithoutExtension(path) == sceneSetting.sceneName)
                        {
                            sceneSetting.sceneAsset =
                                AssetDatabase.LoadAssetAtPath<SceneAsset>(
                                    AssetDatabase.GUIDToAssetPath(guid));
                            break;
                        }
                    }
                    
                    continue;
                }
                
                if (sceneSetting.sceneAsset != null && sceneSetting.sceneName!= sceneSetting.sceneAsset.name )
                {
                    sceneSetting.sceneName = sceneSetting.sceneAsset.name;
                    continue;
                }
            }
            #endif
        }
    }
}