using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFinder : MonoBehaviour
{
    void Update()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
