
namespace Photon.Pun
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(MonoBehaviourPun))]
    public abstract class MonoBehaviourPunEditor : Editor
    {
        MonoBehaviourPun mbTarget;

        private void OnEnable()
        {
            mbTarget = target as MonoBehaviourPun;
        }

        public override void OnInspectorGUI()
        {
            mbTarget = target as MonoBehaviourPun;

            base.OnInspectorGUI();

            if (mbTarget.photonView == null)
            {
                EditorGUILayout.HelpBox("Unable to find a PhotonView on this GameObject or on any parent GameObject.", MessageType.Warning);
            }
        }


    }
}
