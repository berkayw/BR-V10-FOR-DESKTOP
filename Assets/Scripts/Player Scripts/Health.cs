using Photon.Pun;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    public TextMeshProUGUI healthText;
    
    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;

        healthText.text = health.ToString();
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
