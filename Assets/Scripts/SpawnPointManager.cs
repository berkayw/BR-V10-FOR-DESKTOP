using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager instance;
    
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    
}
