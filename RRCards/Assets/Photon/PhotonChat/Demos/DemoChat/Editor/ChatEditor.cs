

#if !PHOTON_UNITY_NETWORKING
using System;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;


namespace Photon.Chat.Editor
{
    [InitializeOnLoad]
    public class ChatEditor : EditorWindow
    {
        static ChatEditor()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private static void OnEditorUpdate()
        {
            EditorApplication.update -= OnEditorUpdate;
        }


        [MenuItem("Window/Photon Chat/Setup")]
        public static void OpenWizard()
        {

            ChatEditor editor = (ChatEditor)EditorWindow.GetWindow(typeof (ChatEditor), false, "Photon Chat");
            editor.minSize = editor.preferredSize;
        }


        private ChatGui cGui;
        internal string mailOrAppId;
        internal bool showDashboardLink = false;
        internal bool showRegistrationDone = false;
        internal bool showRegistrationError = false;
        private readonly Vector2 preferredSize = new Vector2(350, 400);

        internal static string UrlCloudDashboard = "https://dashboard.photonengine.com/en-US/";

        public string WelcomeText = "Thanks for importing Photon Chat.\nThis window should set you up.\n\nYou will need a free Photon Account to setup a Photon Chat application.\nOpen the Photon Dashboard (webpage) to access your account (see button below).\n\nCopy and paste a Chat AppId into the field below and click \"Setup\".";
        public string SetupCompleteInfo = "<b>Done!</b>\nYour Chat AppId is now stored in the <b>Scripts</b> object, Chat App Settings.";
        public string CloseWindowButton = "Close";
        public string OpenCloudDashboardText = "Photon Dashboard Login";
        public string OpenCloudDashboardTooltip = "Review Cloud App information and statistics.";


        public void OnGUI()
        {
            if (this.cGui == null)
            {
                #if UNITY_6000_0_OR_NEWER
                cGui = FindFirstObjectByType<ChatGui>();
                #else
                cGui = FindObjectOfType<ChatGui>();
                #endif
            }

            GUI.skin.label.wordWrap = true;
            GUI.skin.label.richText = true;
            if (string.IsNullOrEmpty(mailOrAppId))
            {
                mailOrAppId = string.Empty;
            }

            GUILayout.Label("Chat Settings", EditorStyles.boldLabel);
            GUILayout.Label(this.WelcomeText);
            GUILayout.Space(15);


            GUILayout.Label("Chat AppId");
            string input = EditorGUILayout.TextField(this.mailOrAppId);


            if (GUI.changed)
            {
                this.mailOrAppId = input.Trim();
            }
            bool minimumInput = false;
            bool isAppId = false;

            if (IsValidEmail(this.mailOrAppId))
            {
                minimumInput = true;
            }
            else if (IsAppId(this.mailOrAppId))
            {
                minimumInput = true;
                isAppId = true;
            }


            EditorGUI.BeginDisabledGroup(!minimumInput);


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool setupBtn = GUILayout.Button("Setup", GUILayout.Width(205));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();


            if (setupBtn)
            {
                this.showDashboardLink = false;
                this.showRegistrationDone = false;
                this.showRegistrationError = false;
                if (isAppId)
                {
                    if (this.cGui != null)
                    {
                        this.cGui.ChatAppSettings.AppIdChat = this.mailOrAppId;
                        EditorUtility.SetDirty(this.cGui);
                    }

                    showRegistrationDone = true;
                }
            }
            EditorGUI.EndDisabledGroup();


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(OpenCloudDashboardText, OpenCloudDashboardTooltip), GUILayout.Width(205)))
            {
                EditorUtility.OpenWithDefaultApp(UrlCloudDashboard);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (this.showRegistrationDone)
            {
                GUILayout.Space(15);
                GUILayout.Space(15);
                GUILayout.Label(SetupCompleteInfo);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(CloseWindowButton, GUILayout.Width(205)))
                {
                    this.Close();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        public static bool IsAppId(string val)
        {
            if (string.IsNullOrEmpty(val) || val.Length < 16)
            {
                return false;
            }

            try
            {
                new Guid(val);
            }
            catch
            {
                return false;
            }
            return true;
        }
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                return false;
            }
            try
            {
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(email);
                return email.Equals(addr.Address);
            }
            catch
            {
                return false;
            }
        }
    }
}
#endif