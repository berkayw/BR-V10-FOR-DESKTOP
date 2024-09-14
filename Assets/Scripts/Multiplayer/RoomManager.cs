using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public string roomNameToJoin = "unnamed";

    private Player[] allPlayersinCurrentRoom;
    private int myNumberInRoom;
    
    public TMP_InputField createRoomInputField;
    
    public GameObject playerListUI;
    public GameObject roomListUI;

    private void Start()
    {
        instance = this;
    }
    
    public void CreateRoomWithName(string _roomName) //create -> input field on value changed
    {
        roomNameToJoin = _roomName;
    }

    //Create room and join
    public void CreateRoomButtonPressed() //roomNameToJoin is set by Room
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, roomOptions, null);
        Debug.Log(roomNameToJoin + " room created and joined");
        //RESET INPUT FIELD VALUE, OR ROOM NAME CACHE CAN CAUSE BUG LIKE CREATING ROOM WITH THE LATEST INPUTFIELD TEXT!!!
        createRoomInputField.text = "";

    }

    //Join existing room
    public void JoinRoomItemButtonPressed(string _roomName)
    {
        roomNameToJoin = _roomName;
        PhotonNetwork.JoinRoom(roomNameToJoin);
        roomListUI.SetActive(false);
        playerListUI.SetActive(true);
        //Debug.Log("Room nameee:" + roomNameToJoin);
    }

    public void LeaveRoomButtonPressed() //Leave Button Pressed
    {
        PhotonNetwork.LeaveRoom();
    }
    
    //[PunRPC]
    public void StartGame()
    {
        if (PlayerList.instance.CheckIfAllPlayersReady()) //&& PhotonNetwork.CurrentRoom.PlayerCount == 8)
        {
            //oyun basliyor ui!
            //oyun basladıktan sonra odayı gizle.
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            //PhotonNetwork.LoadLevel("GameScene");
            PhotonNetwork.LoadLevel("SpaceScene");


            
        }
        else
        {
            //tum oyuncular hazır degil!
            
            Debug.LogError("Make Sure That All Players Are Ready!");
        }
    }

    /*public void StartGameButtonPressed()
    {
        photonView.RPC("StartGame", RpcTarget.All);
    }*/
    
}
    

