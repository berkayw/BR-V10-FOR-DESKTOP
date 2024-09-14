using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
 
public class Crystal : MonoBehaviour
{
    public int crystalHealth = 100;
    public TextMeshPro crystalHealthText;
    
    
    private void Update()
    {
        crystalHealthText.text = crystalHealth + "/100"; 
        
        if (crystalHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}