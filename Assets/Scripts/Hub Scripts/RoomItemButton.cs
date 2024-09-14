using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItemButton : MonoBehaviour
{
    public string roomName; //set roomName in RoomList.cs

    public void OnButtonPressed()
    {
        RoomManager.instance.JoinRoomItemButtonPressed(roomName);
    }

}
