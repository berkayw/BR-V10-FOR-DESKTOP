using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviourPunCallbacks
{
    public int iron = 200;
    public int gold = 200;
    public int diamond = 200;

    public TextMeshProUGUI playerIronText;
    public TextMeshProUGUI playerGoldText;
    public TextMeshProUGUI playerDiamondText;


    public int health = 100;
    public int maxHealth = 100;
    public TextMeshProUGUI playerHealthText;
    public TextMeshPro playerHealthPopUp;

    public int playerID;
    public Transform spawnPoint;

    //Combat 
    public bool invincible;
    
    
    //sword
    public float attackRange = 1f; // Saldırı menzili
    public int attackDamage = 10; // Saldırı hasarı
    public float attackInterval = 1f; // Saldırı aralığı (saniye)

    //magicball
    public float magicBallRange = 6f;
    public int magicBallDamage = 5;
    public float magicBallInterval = 1f;
    public Transform magicBallSpawnpoint;


    public TextMeshProUGUI playerPowerText;
    public TextMeshProUGUI playerInfoText;

    public PlayerRespawner playerRespawner;


    private void Start()
    {
        playerRespawner = GetComponent<PlayerRespawner>();

        playerIronText.text = "Iron: " + iron;
        playerGoldText.text = "Gold: " + gold;
        playerDiamondText.text = "Diamond: " + diamond;

        playerHealthText.text = "Health: " + health;
        playerHealthPopUp.text = health.ToString();
        playerPowerText.text = "Power:" + attackDamage;
    }

    [PunRPC]
    public void AddResource(int amount, string resourceType)
    {
        if (resourceType == "Iron")
        {
            iron += amount;
            playerIronText.text = "Iron: " + iron;
        }

        if (resourceType == "Gold")
        {
            gold += amount;
            playerGoldText.text = "Gold: " + gold;
        }

        if (resourceType == "Diamond")
        {
            diamond += amount;
            playerDiamondText.text = "Diamond: " + diamond;
        }
    }

    [PunRPC]
    public void RemoveIron(int amount)
    {
        iron -= amount;
        playerIronText.text = "Iron: " + iron;
    }

    [PunRPC]
    public void RemoveGold(int amount)
    {
        gold -= amount;
        playerGoldText.text = "Gold: " + gold;
    }

    [PunRPC]
    public void RemoveDiamond(int amount)
    {
        diamond -= amount;
        playerDiamondText.text = "Diamond: " + diamond;
    }

    /*[PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        playerHealthText.text = "Health: " + health;
        playerHealthPopUp.text = health.ToString();
        if (health <= 0)
        {
            if (CrystalManager.instance.IsCrystalActive(playerID))
            {
                PlayerRespawner.instance.RespawnPlayer();
            }
            else
            {
                Destroy(gameObject, 1f);
                PhotonNetwork.LeaveRoom();
            }
        }

    }*/

    [PunRPC]
    public void TakeDamage(int damage, int attackerViewID)
    {
        health -= damage;
        playerHealthText.text = "Health: " + health;
        playerHealthPopUp.text = health.ToString();

        if (health <= 0)
        {
            PhotonView attackerView = PhotonView.Find(attackerViewID);
            
            // Kaynakları aktar   
            if(attackerView.IsMine)         
            {
                 attackerView.RPC("AddResource", RpcTarget.All, this.iron, "Iron");
                 attackerView.RPC("AddResource", RpcTarget.All, this.gold, "Gold");
                 attackerView.RPC("AddResource", RpcTarget.All, this.diamond, "Diamond");
            }
       
            if (CrystalManager.instance.IsCrystalActive(playerID))
            {
                if (photonView.IsMine)
                {
                    playerRespawner.GetComponent<PhotonView>().RPC("RespawnPlayer", RpcTarget.All);
                }
                
               
            }
            else
            {
                if (photonView.IsMine)
                {
                    //delay ekledim cunku kristal kırıksa view'a ve oyuncuya erisemiyor ve kaynak aktarımı yapmıyor.
                    StartCoroutine("BackToHubAfterDelay");
                }
            }
        }
    }

    public IEnumerator BackToHubAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        //Die and Back to Lobby
        Destroy(gameObject);
        
        PhotonNetwork.Disconnect();
        
        SceneManager.LoadScene("HubScene");
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Destroy(gameObject);
        Debug.Log("Disconnected");
        SceneManager.LoadScene("HubScene");

    }

    [PunRPC]
    public void PowerUp(int powerAmount)
    {
        attackDamage += powerAmount;
        playerPowerText.text = "Power:" + attackDamage;
    }

    [PunRPC]
    public void Heal(int healAmount)
    {
        health += healAmount;
        playerHealthText.text = "Health: " + health;
        playerHealthPopUp.text = health.ToString();
    }

    public void SetInfoText(string text)
    {
        playerInfoText.text = text;
    }
}