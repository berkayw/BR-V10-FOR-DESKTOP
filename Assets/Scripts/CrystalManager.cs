using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    public static CrystalManager instance;

    private void Start()
    {
        instance = this;
    }

    // Start is called before the first frame update
    public GameObject[] crystals;

    public bool IsCrystalActive(int playerID)
    {
        if (crystals[playerID] != null)
        {
            return true;
        }
        
        return  false;
    }
    
}

