using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameConnectManager : MonoBehaviourPunCallbacks
{
    public static GameConnectManager instance;
    
    public TextMeshProUGUI nicknameText;
    public TextMeshProUGUI debugRoomText;

    public TextMeshProUGUI roomNameForRoomUI;

    public GameObject nickSelectScreen;
    public GameObject roomListScreen;
    public GameObject createRoomScreen;
    public GameObject playerListScreen;
    public GameObject connectingScreen;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        nicknameText.text = "My nickname is: " + PhotonNetwork.NickName;
        
        if (PhotonNetwork.CurrentRoom != null)
        {
            roomNameForRoomUI.text = "Room Name: \n" + PhotonNetwork.CurrentRoom.Name;
            debugRoomText.text = "Room: " + PhotonNetwork.CurrentRoom.Name + "\nPlayers: " + PhotonNetwork.CurrentRoom.PlayerCount + 
                                 "\nOda Lideri:" + PhotonNetwork.MasterClient.NickName;

        }
        else
        {
            debugRoomText.text = "Not in Room";
        }

        if (nickSelectScreen.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            nickSelectScreen.SetActive(false);
            roomListScreen.SetActive(true);
        }

        if (createRoomScreen.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            createRoomScreen.SetActive(false);
            playerListScreen.SetActive(true);
            RoomManager.instance.CreateRoomButtonPressed();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");

        PhotonNetwork.JoinLobby();
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined To Lobby");
        PhotonNetwork.AutomaticallySyncScene = true;
        connectingScreen.SetActive(false);
    }
    
    public void ChangeNickname(string _nickname)
    {
        PhotonNetwork.NickName = _nickname;
    }
    
}