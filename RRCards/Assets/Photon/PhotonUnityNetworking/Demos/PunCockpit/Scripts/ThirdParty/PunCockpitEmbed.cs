
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.Cockpit
{
    public class PunCockpitEmbed : MonoBehaviourPunCallbacks
    {

        string PunCockpit_scene = "PunCockpit-Scene";

        public bool EmbeddCockpit = true;
        public string CockpitGameTitle = "";

        public GameObject LoadingIndicator;
        public ConnectAndJoinRandom AutoConnect;

        void Awake()
        {
            if (LoadingIndicator != null)
            {
                LoadingIndicator.SetActive(false);
            }
        }
        IEnumerator Start()
        {


            PunCockpit.Embedded = EmbeddCockpit;
            PunCockpit.EmbeddedGameTitle = CockpitGameTitle;

            SceneManager.LoadScene(PunCockpit_scene, LoadSceneMode.Additive);

            yield return new WaitForSeconds(1f);

            if (SceneManager.sceneCount == 1)
            {

                AutoConnect.ConnectNow();

                if (LoadingIndicator != null)
                {
                    LoadingIndicator.SetActive(true);
                }
            }
            else
            {
                Destroy(AutoConnect);
            }

            yield return 0;
        }

        #region MonoBehaviourPunCallbacks implementation

        public override void OnJoinedRoom()
        {

            if (LoadingIndicator != null)
            {
                LoadingIndicator.SetActive(false);
            }

            if (PunCockpit.Instance != null)
            {
                PunCockpit.Instance.SwitchtoMinimalPanel();

            }
        }
        #endregion





    }


}