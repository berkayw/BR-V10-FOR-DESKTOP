using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombatManager : MonoBehaviourPunCallbacks
{
    private PlayerStats playerStats;
    
    private float attackCooldown;
    private float magicBallCooldown;

    public GameObject magicBallPrefab;

    public Animator playerAnimator;
    
    //FlameOrbs
    /*public GameObject flameOrbPrefab;
    public int numberOfOrbs = 3;*/
    public float orbSpawnRadius = 2f;
    public GameObject[] flameOrbs;
    private PhotonView magicballTarget;

    public bool hasMagicBallSkill;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (photonView.IsMine)
        {
            SpawnFlameOrbs();
        }
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f)
        {
            CheckForEnemiesInRange();
        }

        magicBallCooldown -= Time.deltaTime;
        
        if (hasMagicBallSkill && magicBallCooldown <= 0f)
        {
            CheckForMagicAttackRange();
        }
        
        
    }

    private void CheckForEnemiesInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerStats.attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && hitCollider.gameObject != gameObject && !hitCollider.GetComponent<PlayerStats>().invincible)
            {
                // Düşmana saldır
                //photonView.RPC("Attack", RpcTarget.All, hitCollider.GetComponent<PhotonView>().ViewID);

                Attack(hitCollider.GetComponent<PhotonView>().ViewID);
                attackCooldown = playerStats.attackInterval; // Saldırı aralığını sıfırla
                
                //attack animation
                playerAnimator.SetTrigger("swordAttack");
                break; // İlk düşmanı bulduğunda saldır ve döngüyü kır
            }
            
            if (hitCollider.CompareTag("Crystal"))
            {
                if (hitCollider.gameObject != CrystalManager.instance.crystals[playerStats.playerID] && photonView.IsMine) 
                {
                    
                    //RPC
                    //hitCollider.GetComponent<Crystal>().crystalHealth -= playerStats.attackDamage;
                    int crystalViewID = hitCollider.GetComponent<PhotonView>().ViewID;
                    photonView.RPC("DamageCrystal", RpcTarget.AllBuffered, crystalViewID);
                    
                    attackCooldown = playerStats.attackInterval; // Saldırı aralığını sıfırla
                
                    playerAnimator.SetTrigger("swordAttack");
                    
                    Debug.Log("[RPC]It's your own crystal! No damage should be applied.");
                    Debug.Log("[RPC]Attacking Crystal: " + hitCollider.gameObject.name);
                    Debug.Log("[RPC]Player ID: " + playerStats.playerID);
                    
                }
                else //own crystal
                {
                    Debug.Log("It's your own crystal! No damage should be applied.");
                    Debug.Log("Attacking Crystal: " + hitCollider.gameObject.name);
                    Debug.Log("Player ID: " + playerStats.playerID);
                    playerStats.SetInfoText("It's Your Own Crystal!");
                }
            }

            //Debug with dummy
            /*if (hitCollider.CompareTag("Dummy"))
            {
                Debug.Log("Hitting Dummy!");
                playerAnimator.SetTrigger("swordAttack");
            }*/
        }
    }
    [PunRPC]
    public void DamageCrystal(int crystalViewID)
    {
        PhotonView crystalView = PhotonView.Find(crystalViewID);
        if (crystalView != null)
        {
            crystalView.GetComponent<Crystal>().crystalHealth -= playerStats.attackDamage;
            
            //crystal.crystalHealth -= damage;

        }
    }
    
    private void SpawnFlameOrbs()
    {
        float angleStep = 360f / flameOrbs.Length;
        float angle = 0;
        
        foreach (GameObject flameOrb in flameOrbs)
        {
            float angleRad = angle * Mathf.Deg2Rad; 
            
            Vector3 orbOffsetPosition = new Vector3(Mathf.Sin(angleRad), 0.25f, Mathf.Cos(angleRad)) * orbSpawnRadius;
            flameOrb.transform.position = transform.position + orbOffsetPosition;

            flameOrb.GetComponent<FlameOrb>().initialAngle = angleRad;
            
            angle += angleStep; // 0, 120, 240
        }
        
    }

    private void CheckForMagicAttackRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerStats.magicBallRange); 
        foreach (var hitCollider in hitColliders) 
        { 
            if (hitCollider.CompareTag("Player") && hitCollider.gameObject != gameObject && photonView.IsMine) 
            { 
                magicBallCooldown = playerStats.magicBallInterval;
                CastMagicBall(hitCollider.transform.position);
                magicballTarget = hitCollider.GetComponent<PhotonView>();
                break; // İlk düşmanı bulduğunda saldır ve döngüyü kır
            }
        }
    }
    
    private void CastMagicBall(Vector3 targetPosition)
    {       
        //playerAnimator.SetTrigger("magicBall");
        GameObject magicBall = PhotonNetwork.Instantiate
                (magicBallPrefab.name, playerStats.magicBallSpawnpoint.position, Quaternion.identity);
        magicBall.GetComponent<MagicBall>().SetTargetAndShooter(magicballTarget, photonView);
        magicBall.GetComponent<MagicBall>().Initialize(targetPosition);

        Debug.Log(photonView.IsMine + " " + photonView.ViewID);

        
  
    }
    

    //Attack cagrıldı -> dusmanın photonView'ına erisildi ve dusmandaki TakeDamage RPC'si butun server'ın haberi olacak sekilde calıstı.
    private void Attack(int enemyViewID)
    {
        PhotonView enemyView = PhotonView.Find(enemyViewID);
        if (enemyView != null && enemyView.IsMine)
        {
            enemyView.RPC("TakeDamage", RpcTarget.All, playerStats.attackDamage, photonView.ViewID);
        }
    }

    

    private void OnDrawGizmosSelected()
    {
       /* // sword attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerStats.attackRange);
        
        // magicball range 
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, playerStats.magicBallRange);
        */
    }
}
