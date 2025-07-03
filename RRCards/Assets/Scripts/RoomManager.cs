using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Player Name Display")]
    public TextMeshProUGUI localPlayerNameText;
    public TextMeshProUGUI opponentPlayerNameText;

    [Header("Game Management")]
    public GameObject[] flaskPrefabs;
    [SerializeField] private RectTransform[] leftSlots;
    [SerializeField] private RectTransform[] rightSlots;

    private Player localPlayer;
    private Player opponentPlayer;

    void Start()
    {
        localPlayer = PhotonNetwork.LocalPlayer;
        SetupPlayerNames();
    }

    private void SetupPlayerNames()
    {
        if (localPlayerNameText)
        {
            string localName = localPlayer.NickName ?? "You";
            localPlayerNameText.text = localName;
        }
        UpdateOpponentName();
    }

    private void UpdateOpponentName()
    {
        opponentPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => p != localPlayer);

        if (opponentPlayer != null)
        {
            if (opponentPlayerNameText)
            {
                string opponentName = opponentPlayer.NickName ?? "Opponent";
                opponentPlayerNameText.text = opponentName;
            }
            SpawnFlasksInFrame(leftSlots);
            SpawnFlasksInFrame(rightSlots);
        }
        else
        {
            if (opponentPlayerNameText)
            {
                opponentPlayerNameText.text = "Waiting...";
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateOpponentName();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        opponentPlayer = null;
        UpdateOpponentName();
    }

    void SpawnFlasksInFrame(RectTransform[] slots)
    {
        if (leftSlots.Length == 0 || rightSlots.Length == 0 || flaskPrefabs.Length == 0)
        {
            Debug.LogError("Slots hoặc Prefabs chưa được gán hoặc tìm thấy.");
            return;
        }

        List<RectTransform> orderedSlots = new List<RectTransform>(slots);
        orderedSlots.Sort((a, b) =>
        {
            int indexA = a.GetComponent<SlotIndex>().index;
            int indexB = b.GetComponent<SlotIndex>().index;
            return indexA.CompareTo(indexB);
        });

        int flaskCount = Random.Range(1, Mathf.Min(5, orderedSlots.Count + 1));
        for (int i = 0; i < flaskCount; i++)
        {
            RectTransform targetSlot = orderedSlots[i];
            int randomFlaskIndex = Random.Range(0, flaskPrefabs.Length);
            GameObject selectedFlask = flaskPrefabs[randomFlaskIndex];
            GameObject flaskInstance = Instantiate(selectedFlask, targetSlot);
            RectTransform flaskRect = flaskInstance.GetComponent<RectTransform>();
            flaskRect.anchoredPosition = Vector2.zero;
            flaskRect.localScale = Vector3.one;
            Animator anim = flaskInstance.GetComponent<Animator>();
            if (anim != null)
                anim.Play("FlaskPopIn");
        }
    }
}