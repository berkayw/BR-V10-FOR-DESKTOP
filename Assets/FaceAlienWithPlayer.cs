using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAlienWithPlayer : MonoBehaviour
{
    //Update is called once per frame
    void Update()
    {
        if (Camera.main != null)
        {
            //Main Camera -> Camera Holder -> Player
            transform.LookAt(Camera.main.transform.parent.transform.parent.transform);
        }
    }
}
