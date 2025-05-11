using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;

    private bool isReady = false;

    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master. Joining Lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby. Ready to create or join rooms.");
        isReady = true;
    }

    public void CreateRoom()
    {
        if (!isReady)
        {
            Debug.LogWarning("Not ready to create room. Wait until joined lobby.");
            return;
        }

        if (string.IsNullOrEmpty(input_Create.text))
        {
            Debug.LogWarning("Room name is empty.");
            return;
        }

        PhotonNetwork.CreateRoom(input_Create.text);
    }

    public void JoinRoom()
    {
        if (!isReady)
        {
            Debug.LogWarning("Not ready to join room. Wait until joined lobby.");
            return;
        }

        if (string.IsNullOrEmpty(input_Join.text))
        {
            Debug.LogWarning("Room name is empty.");
            return;
        }

        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room. Loading GamePlay scene...");
        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("CreateRoom failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("JoinRoom failed: " + message);
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon: " + cause.ToString());
        isReady = false;
    }
}