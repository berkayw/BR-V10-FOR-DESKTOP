using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawner : MonoBehaviourPunCallbacks
{
    
    private CharacterController characterController;
    
    public GameObject respawnTrigger;
    public Transform spawnPoint;
    private PhotonView view;
    public PlayerStats playerStats;
    public PlayerCombatManager playerCombatManager;
    public GameObject invincibleFX;
    
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        view = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        respawnTrigger = GameObject.FindGameObjectWithTag("RespawnTrigger");
        // Spawn point'i başlatırken kendi playerID'ye göre belirle
        spawnPoint = SpawnPointManager.instance.spawnPoints[playerStats.playerID];

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == respawnTrigger)
        {
            if (CrystalManager.instance.IsCrystalActive(playerStats.playerID))
            {
                if (view.IsMine)
                {
                    view.RPC("RespawnPlayer", RpcTarget.All);
                }
            }
            else
            {
                if (view.IsMine)
                {
                    //Die and Back to Lobby
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

    [PunRPC]
    public void RespawnPlayer()
    {
        //Respawn
        Debug.Log("respawning");
        playerStats.health = playerStats.maxHealth;
        playerStats.playerHealthText.text ="Health: " + playerStats.health;
        playerStats.playerHealthPopUp.text = playerStats.health.ToString();
        
        if(playerStats.GetComponent<PhotonView>().IsMine)
        {
            playerStats.GetComponent<PhotonView>().RPC("RemoveIron",RpcTarget.All, playerStats.iron);
            playerStats.GetComponent<PhotonView>().RPC("RemoveGold",RpcTarget.All, playerStats.gold);
            playerStats.GetComponent<PhotonView>().RPC("RemoveDiamond",RpcTarget.All, playerStats.diamond);
        }
     
        playerStats.invincible = true;
        
        invincibleFX.SetActive(true);
        playerCombatManager.enabled = false;
        characterController.enabled = false;
        transform.position = spawnPoint.position;
        
        StartCoroutine(EnablePlayerAfterDelay(5f));
    }

    private IEnumerator EnablePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        invincibleFX.SetActive(false);
        playerStats.invincible = false;
        characterController.enabled = true;
        playerCombatManager.enabled = true;

    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
    
}