
using UnityEngine;
using Photon.Pun.UtilityScripts;


namespace Photon.Pun.Demo.Cockpit
{

    public class ScoreHelper : MonoBehaviour
    {
        public int Score;

        int _currentScore;
        void Start()
        {

        }
        void Update()
        {


            if (PhotonNetwork.LocalPlayer != null && Score != _currentScore)
            {
                _currentScore = Score;
                PhotonNetwork.LocalPlayer.SetScore(Score);
            }

        }
    }

}