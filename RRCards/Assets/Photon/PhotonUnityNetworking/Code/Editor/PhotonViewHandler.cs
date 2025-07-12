

namespace Photon.Pun
{
	using System.Collections.Generic;
    using Realtime;
    using UnityEditor;
	using UnityEngine;
    using Debug = UnityEngine.Debug;


    [InitializeOnLoad]
	public class PhotonViewHandler : EditorWindow
	{
		static PhotonViewHandler()
		{
			#if (UNITY_2018 || UNITY_2018_1_OR_NEWER)
			EditorApplication.hierarchyChanged += OnHierarchyChanged;
			#else
			EditorApplication.hierarchyWindowChanged += OnHierarchyChanged;
			#endif
		}


		internal static void OnHierarchyChanged()
        {
            if (Application.isPlaying)
            {
                return;
            }


            PhotonView[] photonViewResources = Resources.FindObjectsOfTypeAll<PhotonView>();
            List<PhotonView> photonViewInstances = new List<PhotonView>();
            Dictionary<int, List<PhotonView>> viewInstancesPerViewId = new Dictionary<int, List<PhotonView>>();
            List<PhotonView> photonViewsToReassign = new List<PhotonView>();

            foreach (PhotonView view in photonViewResources)
            {
                if (PhotonEditorUtils.IsPrefab(view.gameObject))
                {
                    if (view.ViewID != 0 || view.sceneViewId != 0)
                    {
                        view.ViewID = 0;
                        view.sceneViewId = 0;
                        EditorUtility.SetDirty(view);
                    }

                    continue;   // skip prefabs in further processing
                }

                photonViewInstances.Add(view);
                if (!IsViewIdOkForScene(view))
                {
                    photonViewsToReassign.Add(view);
                    continue;   // this view definitely gets cleaned up, so it does not count versus duplicates, checked below
                }
                if (!viewInstancesPerViewId.ContainsKey(view.sceneViewId))
                {
                    viewInstancesPerViewId[view.sceneViewId] = new List<PhotonView>();
                }
                viewInstancesPerViewId[view.sceneViewId].Add(view);
            }

            foreach (List<PhotonView> list in viewInstancesPerViewId.Values)
            {
                if (list.Count <= 1)
                {
                    continue;   // skip lists with just one entry (the viewID is unique)
                }


                PhotonView previousAssignment = null;
                bool wasAssigned = PunSceneViews.Instance.Views.TryGetValue(list[0].sceneViewId, out previousAssignment);

                foreach (PhotonView view in list)
                {
                    if (wasAssigned && view.Equals(previousAssignment))
                    {
                        continue;
                    }
                    photonViewsToReassign.Add(view);
                }
            }

            int i;
            foreach (PhotonView view in photonViewsToReassign)
            {
                i = MinSceneViewId(view);
                while (viewInstancesPerViewId.ContainsKey(i))
                {
                    i++;
                }
                view.sceneViewId = i;
                viewInstancesPerViewId.Add(i, null);    // we don't need the lists anymore but we care about getting the viewIDs listed
                EditorUtility.SetDirty(view);
            }
            PunSceneViews.Instance.Views.Clear();
            foreach (PhotonView view in photonViewInstances)
            {
                if (PunSceneViews.Instance.Views.ContainsKey(view.sceneViewId))
                {
                    Debug.LogError("ViewIDs should no longer have duplicates! "+view.sceneViewId, view);  
                    continue;
                }

                PunSceneViews.Instance.Views[view.sceneViewId] = view;
            }
        }


        private static int MinSceneViewId(PhotonView view)
        {
            int result = PunSceneSettings.MinViewIdForScene(view.gameObject.scene.name);
            return result;
        }

        private static bool IsViewIdOkForScene(PhotonView view)
        {
            return view.sceneViewId >= MinSceneViewId(view);
        }
	}
    public class PunSceneViews : ScriptableObject
    {
        [SerializeField]
        public Dictionary<int, PhotonView> Views = new Dictionary<int, PhotonView>();

        private static PunSceneViews instanceField;
        public static PunSceneViews Instance
        {
            get
            {
                if (instanceField != null)
                {
                    return instanceField;
                }
                
                #if UNITY_6000_0_OR_NEWER
                instanceField = GameObject.FindFirstObjectByType<PunSceneViews>();
                #else
                instanceField = GameObject.FindObjectOfType<PunSceneViews>();
                #endif
                if (instanceField == null)
                {
                    instanceField = ScriptableObject.CreateInstance<PunSceneViews>();
                }

                return instanceField;
            }
        }
    }
}