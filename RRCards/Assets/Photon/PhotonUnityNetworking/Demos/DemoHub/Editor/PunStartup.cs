
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

namespace Photon.Pun.Demo.Hub
{
    [InitializeOnLoad]
    public class PunStartup : MonoBehaviour
    {

        static PunStartup()
        {
            bool doneBefore = EditorPrefs.GetBool("PunDemosOpenedBefore");
            if (!doneBefore)
            {
                EditorApplication.update += OnUpdate;
            }
        }

        static void OnUpdate()
        {
            if (EditorApplication.isUpdating || Application.isPlaying)
            {
                return;
            }

            bool doneBefore = EditorPrefs.GetBool("PunDemosOpenedBefore");
            EditorPrefs.SetBool("PunDemosOpenedBefore", true);
            EditorApplication.update -= OnUpdate;

            if (doneBefore)
            {
                return;
            }

            if (string.IsNullOrEmpty(SceneManagerHelper.EditorActiveSceneName) && EditorBuildSettings.scenes.Length == 0)
            {
                LoadPunDemoHub();
                SetPunDemoBuildSettings();
                Debug.Log("No scene was open. Loaded PUN Demo Hub Scene and added demos to build settings. Ready to go! This auto-setup is now disabled in this Editor.");
            }
        }

        [MenuItem("Window/Photon Unity Networking/Configure Demos (build setup)", false, 5)]
        public static void SetupDemo()
        {
            SetPunDemoBuildSettings();
        }

        public static void LoadPunDemoHub()
        {
            string scenePath = FindAssetPath("DemoHub-Scene t:scene");
            if (!string.IsNullOrEmpty(scenePath))
            {
                    EditorSceneManager.OpenScene (scenePath);
                    Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (scenePath);
            }
        }
        public static string FindAssetPath(string asset)
        {
            string[] guids = AssetDatabase.FindAssets(asset, null);
            if (guids.Length < 1)
            {
                Debug.LogError("We have a problem finding the asset: " + asset);
                return string.Empty;
            } else
            {
                return AssetDatabase.GUIDToAssetPath(guids[0]);
            }
        }
        public static void SetPunDemoBuildSettings()
        {
            string _PunPath = string.Empty;

            string _thisPath = PhotonNetwork.FindAssetPath ("PunStartUp");

            _thisPath = Application.dataPath + _thisPath.Substring (6); // remove "Assets/"

            if (string.IsNullOrEmpty(_PunPath))
            {
                _PunPath = Application.dataPath+"/Photon";
            }

            string[] tempPaths = Directory.GetDirectories(_PunPath, "Demos*", SearchOption.AllDirectories);
            if (tempPaths == null)
            {
                return;
            }

            List<EditorBuildSettingsScene> sceneAr = new List<EditorBuildSettingsScene> ();
            foreach (string guidePath in tempPaths)
            {
                tempPaths = Directory.GetFiles (guidePath, "*.unity", SearchOption.AllDirectories);

                if (tempPaths == null || tempPaths.Length == 0)
                {
                    return;
                }
                for (int i = 0; i < tempPaths.Length; i++)
                {
                    string path = tempPaths [i].Substring (Application.dataPath.Length - "Assets".Length);
                    path = path.Replace ('\\', '/');

                    if (path.Contains ("PUNGuide_M2H") || path.Contains("DemoLoadBalancing"))
                    {
                        continue;
                    }
                    if (path.Contains ("DemoHub-Scene"))
                    {
                        sceneAr.Insert (0, new EditorBuildSettingsScene (path, true));
                        continue;
                    }

                    sceneAr.Add (new EditorBuildSettingsScene (path, true));
                }
            }

            EditorBuildSettings.scenes = sceneAr.ToArray();
            EditorSceneManager.OpenScene(sceneAr[0].path);
        }
    }
}