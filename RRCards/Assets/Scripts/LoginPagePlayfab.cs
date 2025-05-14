using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private Coroutine messageCoroutine;
    #region MesageBox
    private void ShowMessage(string msg, float duration = 5f)
    {
        // Nếu đã có coroutine đang chạy thì dừng lại
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
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = NameRegisterInput.text,
            Email = EmailRegisterInput.text,
            Password = PasswordRegisterInput.text,

            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OneregisterSuccess, onError);
    }

    private void onError(PlayFabError Error)
    {
        ShowMessage($"Error: {Error.ErrorMessage}", 5f);
        Debug.Log(Error.GenerateErrorReport());
    }

    private void OneregisterSuccess(RegisterPlayFabUserResult Result)
    {
        ShowMessage("Register successful. You can now log in.", 5f);
        OpenLoginPage();
    }
    public void LoginUser()
    {
        var request1 = new LoginWithEmailAddressRequest
        {
            Email = EmailLoginInput.text,
            Password = PasswordLoginInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request1, OneLoginSuccess, onError);

    }

    private void OneLoginSuccess(LoginResult Result)
    {
        ShowMessage("Loggin in");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion
}
