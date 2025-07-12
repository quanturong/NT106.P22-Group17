

namespace Photon.Pun
{
    public enum ConnectMethod { NotCalled, ConnectToMaster, ConnectToRegion, ConnectToBest }
    public enum PunLogLevel
    {
        ErrorsOnly,
        Informational,
        Full
    }
    public enum RpcTarget
    {
        All,
        Others,
        MasterClient,
        AllBuffered,
        OthersBuffered,
        AllViaServer,
        AllBufferedViaServer
    }


    public enum ViewSynchronization { Off, ReliableDeltaCompressed, Unreliable, UnreliableOnChange }
    public enum OwnershipOption
    {
        Fixed,
        Takeover,
        Request
    }
}