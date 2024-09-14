using Photon.Pun;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                SkillManager.instance.OpenShopCanvas(other.gameObject);
            }
        }
    }
}
