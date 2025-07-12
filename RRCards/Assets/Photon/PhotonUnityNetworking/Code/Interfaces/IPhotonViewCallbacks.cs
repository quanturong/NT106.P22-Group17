namespace Photon.Pun
{
    using Photon.Realtime;
    public interface IPhotonViewCallback
    {

    }
    public interface IOnPhotonViewPreNetDestroy : IPhotonViewCallback
    {
        void OnPreNetDestroy(PhotonView rootView);
    }
    public interface IOnPhotonViewOwnerChange : IPhotonViewCallback
    {
        void OnOwnerChange(Player newOwner, Player previousOwner);
    }
    public interface IOnPhotonViewControllerChange : IPhotonViewCallback
    {
        void OnControllerChange(Player newController, Player previousController);
    }
}
