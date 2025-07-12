
namespace Photon.Chat
{
    public enum ChatDisconnectCause
    {
        None,
        ExceptionOnConnect,
        DisconnectByServerLogic,
        DisconnectByServerReasonUnknown,
        ServerTimeout,
        ClientTimeout,
        Exception,
        InvalidAuthentication,
        MaxCcuReached,
        InvalidRegion,
        OperationNotAllowedInCurrentState,
        CustomAuthenticationFailed,
        AuthenticationTicketExpired,
        DisconnectByClientLogic
    }
}