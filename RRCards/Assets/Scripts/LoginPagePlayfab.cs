using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoginPagePlayfab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Message;

    [Header("Login")]
    [SerializeField] TMP_InputField EmailLoginInput;
    [SerializeField] TMP_InputField PasswordLoginInput;
    [SerializeField] GameObject LoginPage;

    [Header("Register")]
    [SerializeField] TMP_InputField EmailRegisterInput;
    [SerializeField] TMP_InputField PasswordRegisterInput;
    [SerializeField] TMP_InputField NameRegisterInput;
    [SerializeField] GameObject RegisterPage;

    [Header("Recovery")]
    [SerializeField] TMP_InputField EmailRecoveryInput;
    [SerializeField] TMP_InputField PasswordRecoveryInput;
    [SerializeField] TMP_InputField OtpRecoveryInput;
    [SerializeField] GameObject RecoveryPage;

    private Coroutine messageCoroutine;

    void Awake()
    {

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "183B51";
    }

    void Start()
    {
        if (PlayfabAuthManager.Instance == null)
        {
            var authManager = new GameObject("PlayfabAuthManager");
            authManager.AddComponent<PlayfabAuthManager>();
        }
    }

    #region MessageBox
    private void ShowMessage(string msg, float duration = 5f)
    {
        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageCoroutine = StartCoroutine(ShowMessageForSeconds(msg, duration));
    }

    private IEnumerator ShowMessageForSeconds(string msg, float duration)
    {
        Message.text = msg;
        yield return new WaitForSeconds(duration);
        Message.text = "";
        messageCoroutine = null;
    }
    #endregion

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Button Functions
    public void OpenLoginPage()
    {
        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
        RecoveryPage.SetActive(false);
    }

    public void OpenRegisterPage()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
        RecoveryPage.SetActive(false);
    }

    public void OpenRecoveryPage()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(false);
        RecoveryPage.SetActive(true);
    }
    #endregion

    #region PlayFab Auth
    public void RegisterUser()
    {
        if (string.IsNullOrEmpty(NameRegisterInput.text) ||
            string.IsNullOrEmpty(EmailRegisterInput.text) ||
            string.IsNullOrEmpty(PasswordRegisterInput.text))
        {
            ShowMessage("Please fill all fields", 3f);
            return;
        }

        PlayfabAuthManager.Instance.Register(
            EmailRegisterInput.text,
            PasswordRegisterInput.text,
            NameRegisterInput.text
        );

        PlayfabAuthManager.Instance.OnRegisterSuccess += OnRegisterSuccess;
        PlayfabAuthManager.Instance.OnRegisterFailed += OnRegisterFailed;
    }

    private void OnRegisterSuccess(string message)
    {
        ShowMessage(message, 5f);
        OpenLoginPage();

        PlayfabAuthManager.Instance.OnRegisterSuccess -= OnRegisterSuccess;
        PlayfabAuthManager.Instance.OnRegisterFailed -= OnRegisterFailed;
    }

    private void OnRegisterFailed(string error)
    {
        ShowMessage($"Register failed: {error}", 5f);

        PlayfabAuthManager.Instance.OnRegisterSuccess -= OnRegisterSuccess;
        PlayfabAuthManager.Instance.OnRegisterFailed -= OnRegisterFailed;
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(EmailLoginInput.text) ||
            string.IsNullOrEmpty(PasswordLoginInput.text))
        {
            ShowMessage("Please enter email and password", 3f);
            return;
        }

        ShowMessage("Logging in...", 3f);

        PlayfabAuthManager.Instance.Login(EmailLoginInput.text, PasswordLoginInput.text);

        PlayfabAuthManager.Instance.OnLoginSuccess += OnLoginSuccess;
        PlayfabAuthManager.Instance.OnLoginFailed += OnLoginFailed;
    }

    private void OnLoginSuccess(string message)
    {
        ShowMessage("Login successful! Loading game...", 3f);

        if (PlayerStatisticsManager.Instance != null)
            PlayerStatisticsManager.Instance.InitializeStatisticsIfNeeded();

        PlayfabAuthManager.Instance.OnLoginSuccess -= OnLoginSuccess;
        PlayfabAuthManager.Instance.OnLoginFailed -= OnLoginFailed;

        StartCoroutine(WaitForPhotonThenLoadScene());
    }

    private void OnLoginFailed(string error)
    {
        ShowMessage($"Login failed: {error}", 5f);

        PlayfabAuthManager.Instance.OnLoginSuccess -= OnLoginSuccess;
        PlayfabAuthManager.Instance.OnLoginFailed -= OnLoginFailed;
    }

    private IEnumerator WaitForPhotonThenLoadScene()
    {
        float timeout = 10f;
        float timer = 0f;

        while (!PlayfabAuthManager.Instance.IsPhotonReady() && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (timer >= timeout)
        {
            ShowMessage("Connection timeout. Please try again.", 5f);
            yield break;
        }

        ShowMessage("Ready! Loading game...", 2f);
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RecoverPassword()
    {
        if (string.IsNullOrEmpty(EmailRecoveryInput.text))
        {
            ShowMessage("Please enter your account email", 3f);
            return;
        }

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = EmailRecoveryInput.text,
            TitleId = PlayFabSettings.staticSettings.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(
            request,
            OnRecoverySuccess,
            OnRecoveryError
        );
    }

    private void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
    {
        ShowMessage("Recovery email sent! Check your inbox.", 5f);
        OpenLoginPage();
    }

    private void OnRecoveryError(PlayFabError error)
    {
        ShowMessage($"Password recovery failed: {error.ErrorMessage}", 5f);
    }
    public void LogoutUser()
    {
        StartCoroutine(HandleLogout());
    }

    private IEnumerator HandleLogout()
    {
        // Nếu đang kết nối Photon thì ngắt kết nối
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();

            float timeout = 5f;
            float timer = 0f;

            while (PhotonNetwork.IsConnected && timer < timeout)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        // Xóa thông tin đăng nhập PlayFab
        PlayFabClientAPI.ForgetAllCredentials();

        // Hiển thị thông báo
        ShowMessage("Bạn đã đăng xuất", 2.5f);

        // Nếu bạn có scene riêng cho login
        // SceneManager.LoadScene("LoginScene"); 

        // Nếu đang dùng chung scene, thì bật lại giao diện login
        OpenLoginPage();

        // Reset các input field nếu cần
        EmailLoginInput.text = "";
        PasswordLoginInput.text = "";
    }
    public void OnLogoutClicked()
    {
        PlayfabAuthManager.Instance.Logout(() =>
        {
            // Load lại màn login
            OpenLoginPage();
        });
    }
    #endregion
}