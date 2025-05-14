using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabAuthManager : MonoBehaviour
{
    public void Login(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request,
            result => {
                Debug.Log("✅ Đăng nhập thành công!");
                // Gọi Server hoặc vào scene chính ở đây
            },
            error => {
                Debug.LogError("❌ Lỗi đăng nhập: " + error.GenerateErrorReport());
            });
    }

    public void Register(string email, string password, string username)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            Username = username,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request,
            result => {
                Debug.Log("✅ Đăng ký thành công!");
            },
            error => {
                Debug.LogError("❌ Lỗi đăng ký: " + error.GenerateErrorReport());
            });
    }

    public void RecoverPassword(string email)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = PlayFabSettings.staticSettings.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request,
            result => {
                Debug.Log("✅ Email khôi phục đã được gửi!");
            },
            error => {
                Debug.LogError("❌ Lỗi gửi email khôi phục: " + error.GenerateErrorReport());
            });
    }
}
