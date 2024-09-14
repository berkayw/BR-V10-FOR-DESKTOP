using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemButton : MonoBehaviour
{
    public string playerName;

    public void OnButtonPressed()
    {
        Debug.Log("Player's Name: " + playerName);
    }
}
