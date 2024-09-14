using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList instance;

    public GameObject roomManagerGameObject;
    public RoomManager roomManager;
    
    
    [Header("UI")] public Transform roomListParent;
    public GameObject roomListItemPrefab;

    public List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    
    private void Awake()
    {
        instance = this;
    }
    
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //Aphoton'daki oda listesiyle ca birbirine senkronize ediyoruz.
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                // Listeden kaldırılacak odayı bul ve kaldır
                cachedRoomList.RemoveAll(x => x.Name == room.Name);
            }
            else
            {
                // Oda zaten varsa güncelle, yoksa ekle
                int index = cachedRoomList.FindIndex(x => x.Name == room.Name);
                if (index != -1)
                {
                    cachedRoomList[index] = room; // Oda zaten var, güncelle
                }
                else
                {
                    cachedRoomList.Add(room); // Oda yok, ekle
                }
            }
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name; //room name
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                room.PlayerCount +"/" + room.MaxPlayers; //player count

            roomItem.GetComponent<RoomItemButton>().roomName = room.Name; 
        }
    }


}
