using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FlameOrb : MonoBehaviourPunCallbacks
{
    public float rotationSpeed = 100f;
    public float radius = 2f;
    public int damage = 1;
    private Transform player;
    public float initialAngle;

    private void Start()
    {
        player = transform.parent.transform.parent;
    }

    private void Update()
    {
        //Rotate orbs
        if (player != null && player.GetComponent<PhotonView>().IsMine)
        {
            float angle = rotationSpeed * Time.time + initialAngle; //in radian***
            Vector3 offset = new Vector3(Mathf.Sin(angle), 0.25f, Mathf.Cos(angle)) * radius;
            transform.position = player.position + offset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView targetView = other.GetComponent<PhotonView>();

        if (other.CompareTag("Player"))
        {
            if (targetView != null && targetView.IsMine)
            {
                targetView.RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
        if (other.CompareTag("Crystal"))
        {
            //other.GetComponent<Crystal>().crystalHealth -= damage;
        }
        
    }
}
