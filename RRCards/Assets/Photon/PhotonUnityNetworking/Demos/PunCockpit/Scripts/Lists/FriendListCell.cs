
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;

namespace Photon.Pun.Demo.Cockpit
{
    public class FriendListCell : MonoBehaviour
    {
        public FriendListView ListManager;

        public Text NameText;
        public GameObject OnlineFlag;

        public GameObject inRoomText;
        public GameObject JoinButton;

        FriendInfo _info;


        public void RefreshInfo(FriendListView.FriendDetail details)
        {
            NameText.text = details.NickName;

            OnlineFlag.SetActive(false);

            inRoomText.SetActive(false);
            JoinButton.SetActive(false);
        }

        public void RefreshInfo(FriendInfo info)
        {
            _info = info;

            OnlineFlag.SetActive(_info.IsOnline);

            inRoomText.SetActive(_info.IsInRoom);
            JoinButton.SetActive(_info.IsInRoom);
        }

        public void JoinFriendRoom()
        {
            ListManager.JoinFriendRoom(_info.Room);
        }

        public void RemoveFromList()
        {
            Destroy(this.gameObject);
        }

    }
}