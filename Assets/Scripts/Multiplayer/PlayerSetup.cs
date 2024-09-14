using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    //This class for only enable scripts on localPlayer

    public GameObject[] onlyLocalPlayerStuff;


    public TextMeshPro nicknameText;
    
    
    private void Update()
    {
        if (!photonView.IsMine)
        {
            foreach (GameObject g in onlyLocalPlayerStuff)
            {
                g.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void SetNickname(string _nickname)
    {
        nicknameText.text = _nickname;
    }
    
    [PunRPC]
    public void SetColor(float r, float g, float b) //calling rom InGameManager, Setting Player Colors.
    {
        Color color = new Color(r, g, b);
        Renderer renderer = transform.GetChild(1).GetChild(1).GetComponent<Renderer>();

        renderer.material.color = color;
        renderer.material.SetFloat("_Metallic", 0.1f);
        renderer.material.SetFloat("_Glossiness", 0.7f);
    }
}
