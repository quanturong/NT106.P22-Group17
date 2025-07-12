

namespace Photon.Pun.UtilityScripts
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    public class OnClickInstantiate : MonoBehaviour, IPointerClickHandler
    {
        public enum InstantiateOption { Mine, Scene }


        public PointerEventData.InputButton Button;
        public KeyCode ModifierKey;

        public GameObject Prefab;

        [SerializeField]
		private InstantiateOption InstantiateType = InstantiateOption.Mine;


        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!PhotonNetwork.InRoom || (this.ModifierKey != KeyCode.None && !Input.GetKey(this.ModifierKey)) || eventData.button != this.Button)
            {
                return;
            }


            switch (this.InstantiateType)
            {
                case InstantiateOption.Mine:
                    PhotonNetwork.Instantiate(this.Prefab.name, eventData.pointerCurrentRaycast.worldPosition + new Vector3(0, 0.5f, 0), Quaternion.identity, 0);
                    break;
                case InstantiateOption.Scene:
                    PhotonNetwork.InstantiateRoomObject(this.Prefab.name, eventData.pointerCurrentRaycast.worldPosition + new Vector3(0, 0.5f, 0), Quaternion.identity, 0, null);
                    break;
            }
        }
    }
}