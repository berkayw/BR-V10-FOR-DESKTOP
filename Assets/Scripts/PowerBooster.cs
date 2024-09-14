using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PowerBooster : MonoBehaviourPunCallbacks
{
    public int powerAmount = 10;
    public int goldNeeded = 10;

    
    public TextMeshPro powerText; // power  UI Text
    
    private void Update()
    {

        powerText.text =  "+" + powerAmount + " Power = " + goldNeeded + " Gold";

    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerStats>().gold >= goldNeeded)
            {
                if (other.GetComponent<PhotonView>().IsMine)  
                {
                    other.GetComponent<PhotonView>().RPC("PowerUp", RpcTarget.All, powerAmount);
                    other.GetComponent<PhotonView>().RPC("RemoveGold", RpcTarget.All, goldNeeded);
                    other.GetComponent<PlayerStats>().SetInfoText(powerAmount + " power added!");
                }
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
