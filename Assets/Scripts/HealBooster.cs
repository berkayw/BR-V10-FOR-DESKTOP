using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HealBooster : MonoBehaviourPunCallbacks
{
    public int healAmount = 10;
    public int goldNeeded = 10;

    public TextMeshPro healText; // power  UI Text
    

    private void Update()
    {
        healText.text =  "+" + healAmount + " Health = " + goldNeeded + " Gold";
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerStats>().gold >= goldNeeded)
            {
                if (other.GetComponent<PhotonView>().IsMine)  
                {
                    other.GetComponent<PhotonView>().RPC("Heal", RpcTarget.All, healAmount);
                    other.GetComponent<PhotonView>().RPC("RemoveGold", RpcTarget.All, goldNeeded);
                    other.GetComponent<PlayerStats>().SetInfoText(healAmount + " heal added!");
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