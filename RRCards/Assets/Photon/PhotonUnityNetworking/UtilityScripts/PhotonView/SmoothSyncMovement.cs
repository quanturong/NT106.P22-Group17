
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    [RequireComponent(typeof(PhotonView))]
    public class SmoothSyncMovement : Photon.Pun.MonoBehaviourPun, IPunObservable
    {
        public float SmoothingDelay = 5;
        public void Awake()
        {
            bool observed = false;
            foreach (Component observedComponent in this.photonView.ObservedComponents)
            {
                if (observedComponent == this)
                {
                    observed = true;
                    break;
                }
            }
            if (!observed)
            {
                Debug.LogWarning(this + " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used.");
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                correctPlayerPos = (Vector3)stream.ReceiveNext();
                correctPlayerRot = (Quaternion)stream.ReceiveNext();
            }
        }

        private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
        private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

        public void Update()
        {
            if (!photonView.IsMine)
            {
                transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
                transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
            }
        }

    }
}