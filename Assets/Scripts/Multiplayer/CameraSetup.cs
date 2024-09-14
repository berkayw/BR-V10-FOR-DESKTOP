using System;
using Photon.Pun;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    //This class for only enable scripts on localPlayer

    public GameObject cameraHolder;
    public GameObject screenSpaceCanvas;
    public GameObject worldSpaceCanvas;
    
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!view.IsMine)
        {
            cameraHolder.SetActive(false);
            screenSpaceCanvas.SetActive(false);
            worldSpaceCanvas.SetActive(false);
        }
    }
}
