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
    private bool hasSpawned = false;

    void Start()
    {
        localPlayer = PhotonNetwork.LocalPlayer;
        SetupPlayerNames();

        Invoke("CheckAndSpawn", 1f);
    }

    void CheckAndSpawn()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2 && !hasSpawned)
        {
            SpawnFlasksForAllPlayers();
        }
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

        if (PhotonNetwork.IsMasterClient && !hasSpawned)
        {
            Invoke("CheckAndSpawn", 1f);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        opponentPlayer = null;
        UpdateOpponentName();
    }

    void SpawnFlasksForAllPlayers()
    {
        if (hasSpawned) return;

        hasSpawned = true;

        List<FlaskSpawnData> spawnData = GenerateSpawnData();

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["FlaskData"] = JsonUtility.ToJson(new FlaskDataWrapper { flasks = spawnData.ToArray() });
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        SpawnFromData(spawnData);
    }

    List<FlaskSpawnData> GenerateSpawnData()
    {
        List<FlaskSpawnData> data = new List<FlaskSpawnData>();

        Random.InitState(System.DateTime.Now.Millisecond + PhotonNetwork.LocalPlayer.ActorNumber);

        int maxSlots = Mathf.Min(leftSlots.Length, rightSlots.Length);

        int flaskCountA = Random.Range(1, Mathf.Min(5, maxSlots + 1));

        int flaskCountB = Random.Range(1, Mathf.Min(5, maxSlots + 1));

        List<int> usedSlotsA = new List<int>();
        for (int i = 0; i < flaskCountA; i++)
        {
            int nextSlot = GetNextAvailableSlot(usedSlotsA, maxSlots);
            if (nextSlot != -1)
            {
                usedSlotsA.Add(nextSlot);
                data.Add(new FlaskSpawnData
                {
                    slotIndex = nextSlot,
                    isPlayerA = true,
                    flaskIndex = Random.Range(0, flaskPrefabs.Length)
                });
            }
        }

        List<int> usedSlotsB = new List<int>();
        for (int i = 0; i < flaskCountB; i++)
        {
            int nextSlot = GetNextAvailableSlot(usedSlotsB, maxSlots);
            if (nextSlot != -1)
            {
                usedSlotsB.Add(nextSlot);
                data.Add(new FlaskSpawnData
                {
                    slotIndex = nextSlot,
                    isPlayerA = false,
                    flaskIndex = Random.Range(0, flaskPrefabs.Length)
                });
            }
        }

        return data;
    }

    int GetNextAvailableSlot(List<int> usedSlots, int maxSlots)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (!usedSlots.Contains(i))
            {
                return i;
            }
        }
        return -1; 
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void SpawnFromData(List<FlaskSpawnData> spawnData)
    {
        ClearAllFlasks();

        bool isPlayerA = PhotonNetwork.LocalPlayer.ActorNumber == 1;

        foreach (var data in spawnData)
        {
            RectTransform[] targetSlots;

            if (data.isPlayerA)
            {
                if (isPlayerA)
                {
                    targetSlots = leftSlots;
                }
                else
                {
                    targetSlots = rightSlots;
                }
            }
            else
            {
                if (isPlayerA)
                {
                    targetSlots = rightSlots;
                }
                else
                {
                    targetSlots = leftSlots;
                }
            }

            if (data.slotIndex < targetSlots.Length)
            {
                SpawnFlaskAtSlot(targetSlots[data.slotIndex], data.flaskIndex);
            }
        }
    }

    void SpawnFlaskAtSlot(RectTransform targetSlot, int flaskIndex)
    {
        if (flaskIndex >= flaskPrefabs.Length) return;

        GameObject selectedFlask = flaskPrefabs[flaskIndex];
        GameObject flaskInstance = Instantiate(selectedFlask, targetSlot);

        RectTransform flaskRect = flaskInstance.GetComponent<RectTransform>();
        flaskRect.anchoredPosition = Vector2.zero;
        flaskRect.localScale = Vector3.one;

        Animator anim = flaskInstance.GetComponent<Animator>();
        if (anim != null)
            anim.Play("FlaskPopIn");
    }

    void ClearAllFlasks()
    {
        foreach (var slot in leftSlots)
        {
            for (int i = slot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(slot.GetChild(i).gameObject);
            }
        }

        foreach (var slot in rightSlots)
        {
            for (int i = slot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(slot.GetChild(i).gameObject);
            }
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("FlaskData") && !hasSpawned)
        {
            hasSpawned = true;
            string jsonData = (string)propertiesThatChanged["FlaskData"];
            FlaskDataWrapper wrapper = JsonUtility.FromJson<FlaskDataWrapper>(jsonData);
            SpawnFromData(wrapper.flasks.ToList());
        }
    }

    [ContextMenu("Force Spawn Flasks")]
    public void ForceSpawnFlasks()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            hasSpawned = false;
            SpawnFlasksForAllPlayers();
        }
    }
}

[System.Serializable]
public class FlaskSpawnData
{
    public int slotIndex;
    public bool isPlayerA; 
    public int flaskIndex;
}

[System.Serializable]
public class FlaskDataWrapper
{
    public FlaskSpawnData[] flasks;
}