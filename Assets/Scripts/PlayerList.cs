using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;


public class PlayerList : MonoBehaviourPunCallbacks
{
    public static PlayerList instance;
    
    [Header("UI")] public Transform playerListParent;
    public GameObject playerListItemPrefab;
    
    public Dictionary<Player, bool> playerDictionary;

    public GameObject startGameButton;
    public GameObject readyButton;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerDictionary = new Dictionary<Player, bool>();
        UpdateUI();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
            readyButton.SetActive(false);
        }
        else
        {
            startGameButton.SetActive(false);
            readyButton.SetActive(true);
        }
    }
    

    public override void OnJoinedRoom() //for local player
    {
        playerDictionary.Clear();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            bool isReady = player.IsMasterClient;
            playerDictionary.Add(player, isReady);
        }
        
        UpdateUI();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Yeni oyuncu odaya katıldığında listeyi güncelle
        bool isReady = newPlayer.IsMasterClient;
        playerDictionary.Add(newPlayer, isReady);

        UpdateUI();
    }
    
    // MasterClient değiştiğinde çağrılan olay / master client odadan cıkınca
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // Yeni MasterClient'i otomatik olarak ready olarak işaretle
        if (playerDictionary.ContainsKey(newMasterClient))
        {
            playerDictionary[newMasterClient] = true;
        }

        UpdateUI();
    }
    
    public override void OnLeftRoom()
    {
        RoomList.instance.UpdateUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Bir oyuncu odadan ayrıldığında listeyi güncelle
        playerDictionary.Remove(otherPlayer);

        UpdateUI();
    }

    public void OnClickReadyButton()
    {
        photonView.RPC("SetReadyStatus", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
        
        TextMeshProUGUI readyButtonText = readyButton.GetComponentInChildren<TextMeshProUGUI>();
        Button readyButtonComp = readyButton.GetComponent<Button>();
        
        if (readyButtonText.text == "Ready")
        {
            readyButtonText.text = "Unready";
            readyButtonComp.image.color = Color.red;
        }
        else
        {
            readyButtonText.text = "Ready";
            readyButtonComp.image.color = Color.green;
        }
        
    }

    [PunRPC]
    public void SetReadyStatus(Player player)
    {
        if (playerDictionary.ContainsKey(player))
        {
            playerDictionary[player] = !playerDictionary[player];
            UpdateUI();
        }
    }

    public bool CheckIfAllPlayersReady()
    {
        foreach (var playerReady in playerDictionary.Values)
        {
            if (!playerReady)
            {
                return false;
            }
        }
        return true;
    }
    
    void UpdateUI()
    {
        foreach (Transform playerItem in playerListParent)
        {
            Destroy(playerItem.gameObject);
        }
        
        foreach (var player in playerDictionary)
        {
            GameObject playerItem = Instantiate(playerListItemPrefab, playerListParent);

            
            playerItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                player.Key.NickName; // player.NickName;
            
            if (player.Value) // ready
            {
                playerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;
                playerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Ready";
            }
            else
            {
                playerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
                playerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Not Ready";
            }
            
            if (player.Key.IsMasterClient)
            {
                playerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.blue;
                playerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Leader"; //player.Value ? "LReady" : "LNot Ready";
            }
            
            

        }
        
    }

}
