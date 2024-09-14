using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MagicBall : MonoBehaviourPunCallbacks
{
    private float speed = 10f;
    private int damage = 10;
    
    private Rigidbody rb;
    private Vector3 targetPosition;
    public GameObject hitFX;
    public PhotonView shooterView;
    public PhotonView targetView;

    public void SetTargetAndShooter(PhotonView targetView, PhotonView shooterView)
    {
        this.shooterView = shooterView;
        this.targetView = targetView;
    }
    
    public void Initialize(Vector3 targetPos)
    {
        targetPosition = targetPos + new Vector3(0, 0.6f, 0); // biraz yukarıdan atış
        rb = GetComponent<Rigidbody>();
        rb.velocity = (targetPosition - transform.position).normalized * speed;
        Debug.Log("Magic ball initialized");
    }

    private void Start()
    { 
        Destroy(gameObject, 1f);
        Debug.Log("shooter: " + shooterView.ViewID);
        Debug.Log("target: " + targetView.ViewID);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>() == targetView)
        {
            targetView.RPC
                ("TakeDamage", RpcTarget.All, 
                    shooterView.GetComponent<PlayerStats>().magicBallDamage, shooterView.ViewID);

            // Hit FX 
            PhotonNetwork.Instantiate(hitFX.name, transform.position, Quaternion.identity);

            Debug.Log(  shooterView.ViewID + " hasar verdi.\n" + targetView.ViewID + " hasar aldı.");

            Destroy(gameObject);
        }
            
    }
}

