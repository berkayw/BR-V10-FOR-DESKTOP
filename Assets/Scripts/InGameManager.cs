using System;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Serialization;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager instance;

    public GameObject player;

    public int myNumberInRoom;

    public TextMeshProUGUI numberText;
    
    public Color[] playerColors = new Color[8];

    private void Start()
    {
        instance = this;
        PhotonNetwork.AutomaticallySyncScene = false; //oyundayken master client'ten bagımsız sahne gecisi.

        Debug.Log("We're in a game now");
        

        int myNumberInRoom = GetSpawnNumber();
        numberText.text = myNumberInRoom.ToString();
        Color playerColor = playerColors[myNumberInRoom];
        
        GameObject _player = PhotonNetwork.Instantiate(player.name,
            SpawnPointManager.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);

        _player.GetComponent<PlayerStats>().playerID = myNumberInRoom;
        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        _player.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBuffered, playerColor.r, playerColor.g, playerColor.b); //from player setup
    }

    public int GetSpawnNumber()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                return i;
            }
        }
        return 0;
    }   
    
    
}

    

