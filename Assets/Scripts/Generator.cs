using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Generator : MonoBehaviour
{
    public int resourceAmount = 0; 
    public int maxResourceStock = 10; 
    public TextMeshPro generatorText;

    public string resourceType; //change from prefab(inspector) / iron, gold, diamond 

    private float productionRate = 1f; 
    private bool isGenerating = true;

    
    private void Update()
    {
        UpdateText();
    }

    private void Start()
    {
        switch (resourceType)
        {
            case "Iron":
                productionRate = 1f;
                break;
            
            case "Gold":
                productionRate = 2.5f;
                break;
            
            case "Diamond":
                productionRate = 5f;
                break;
        }
        
        StartCoroutine(GenerateResource());
    }

    private IEnumerator GenerateResource()
    {
        while (isGenerating)
        {
            if (resourceAmount < maxResourceStock)
            {
                resourceAmount++;
            }
            yield return new WaitForSeconds(productionRate);
        }
    }

    private void UpdateText()
    {
        if (generatorText != null)
        {
            generatorText.text = resourceType + ": " + resourceAmount;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                other.GetComponent<PhotonView>().RPC("AddResource", RpcTarget.All, resourceAmount, resourceType);
            }
            resourceAmount = 0;
            UpdateText();
        }    
    }
}
