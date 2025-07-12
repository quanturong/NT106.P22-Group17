

namespace Photon.Pun.UtilityScripts
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    public class OnClickDestroy : MonoBehaviourPun, IPointerClickHandler
    {
        public PointerEventData.InputButton Button;
        public KeyCode ModifierKey;

        public bool DestroyByRpc;
        

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!PhotonNetwork.InRoom || (this.ModifierKey != KeyCode.None && !Input.GetKey(this.ModifierKey)) || eventData.button != this.Button )
            {
                return;
            }


            if (this.DestroyByRpc)
            {
                this.photonView.RPC("DestroyRpc", RpcTarget.AllBuffered);
            }
            else
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }


        [PunRPC]
        public IEnumerator DestroyRpc()
        {
            Destroy(this.gameObject);
            yield return 0; // if you allow 1 frame to pass, the object's OnDestroy() method gets called and cleans up references.
        }
    }
}