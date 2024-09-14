using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Chest : MonoBehaviourPunCallbacks
{
    public int goldNeeded = 25;
    public int powerAmount = 25;

    public bool chestOpened;

    public TextMeshPro chestText; // power  UI Text

    private void Update()
    {
        if (!chestOpened)
        {
            chestText.text =  "+" + powerAmount + " Power = " + goldNeeded + " Gold";
        }
        else
        {
            chestText.text = "Chest is Empty!";
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !chestOpened)
        {
            if (other.GetComponent<PlayerStats>().gold >= goldNeeded)
            {
                if (other.GetComponent<PhotonView>().IsMine)
                {
                    other.GetComponent<PhotonView>().RPC("PowerUp", RpcTarget.All, powerAmount);
                    other.GetComponent<PhotonView>().RPC("RemoveGold", RpcTarget.All, goldNeeded);
                    other.GetComponent<PlayerStats>().SetInfoText(powerAmount + " power added!");
                }
                
                chestOpened = true;
            }
            else
            {
                other.GetComponent<PlayerStats>().SetInfoText("Not enough gold!");
            }
        }    
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().SetInfoText("");
        }
    }
}
