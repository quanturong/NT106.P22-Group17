
namespace Photon.Pun
{
    using UnityEngine;
    using Photon.Realtime;
    public interface IPunObservable
    {
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
    }
    public interface IPunOwnershipCallbacks
    {
        void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer);
        void OnOwnershipTransfered(PhotonView targetView, Player previousOwner);
        void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest);
    }
    public interface IPunInstantiateMagicCallback
    {
        void OnPhotonInstantiate(PhotonMessageInfo info);
    }
    public interface IPunPrefabPool
    {
        GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation);
        void Destroy(GameObject gameObject);
    }
}