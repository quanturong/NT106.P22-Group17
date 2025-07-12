 

using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Photon.Realtime;

namespace Photon.Pun.Demo.Cockpit
{
    public class PlayerListView : MonoBehaviourPunCallbacks
    {
        public PlayerDetailsController PlayerDetailManager;

        public PlayerListCell CellPrototype;

        public Text PlayerCountsText;

        public Text UpdateStatusText;

        Dictionary<int, PlayerListCell> playerCellList = new Dictionary<int, PlayerListCell>();


        void Awake()
        {
            CellPrototype.gameObject.SetActive(false);
        }

        public override void OnEnable()
        {

            base.OnEnable();

            UpdateStatusText.text = string.Empty;

            if (PhotonNetwork.CurrentRoom == null)
            {
                return;
            }

            RefreshCount();

            foreach (KeyValuePair<int, Player> _entry in PhotonNetwork.CurrentRoom.Players)
            {
                if (playerCellList.ContainsKey(_entry.Key))
                {
                    continue;
                }
                playerCellList[_entry.Key] = Instantiate(CellPrototype);
                playerCellList[_entry.Key].transform.SetParent(CellPrototype.transform.parent, false);
                playerCellList[_entry.Key].gameObject.SetActive(true);
                playerCellList[_entry.Key].AddToList(_entry.Value, false);
            }
        }

        public void SelectPlayer(Player player)
        {
            PlayerDetailManager.SetPlayerTarget(player);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!playerCellList.ContainsKey(newPlayer.ActorNumber))
            {
                playerCellList[newPlayer.ActorNumber] = Instantiate(CellPrototype.gameObject).GetComponent<PlayerListCell>();
                playerCellList[newPlayer.ActorNumber].transform.SetParent(CellPrototype.transform.parent, false);
                playerCellList[newPlayer.ActorNumber].gameObject.SetActive(true);
                playerCellList[newPlayer.ActorNumber].AddToList(newPlayer, true);
            }
            else // rejoin
            {
                playerCellList[newPlayer.ActorNumber].RefreshInfo(null);
            }


            StartCoroutine("UpdateUIPing");
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            foreach (KeyValuePair<int, Player> _entry in PhotonNetwork.CurrentRoom.Players)
            {
                playerCellList[_entry.Key].RefreshInfo(null);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (playerCellList.ContainsKey(target.ActorNumber))
            {
                playerCellList[target.ActorNumber].RefreshInfo(changedProps);
            }
            else
            {
                Debug.LogWarning("PlayerListView: missing Player Ui Cell for " + target, this);
            }

            StartCoroutine("UpdateUIPing");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {

            if (!PhotonNetwork.PlayerListOthers.Contains(otherPlayer))
            {
                playerCellList[otherPlayer.ActorNumber].RemoveFromList();
                playerCellList.Remove(otherPlayer.ActorNumber);
            }
            else
            {

                playerCellList[otherPlayer.ActorNumber].RefreshInfo(null);
            }

            StartCoroutine("UpdateUIPing");
        }


        void RefreshCount()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                PlayerCountsText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString("00");
            }

        }
        IEnumerator UpdateUIPing()
        {
            UpdateStatusText.text = "Updated";

            yield return new WaitForSeconds(1f);

            UpdateStatusText.text = string.Empty;
        }


        public void ResetList()
        {
            foreach (KeyValuePair<int, PlayerListCell> entry in playerCellList)
            {
                if (entry.Value != null)
                {
                    Destroy(entry.Value.gameObject);
                }
            }

            playerCellList = new Dictionary<int, PlayerListCell>();
        }
    }
}