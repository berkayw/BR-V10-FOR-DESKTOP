using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SkillManager : MonoBehaviourPunCallbacks
{
    public static SkillManager instance;
    public GameObject shopCanvas;
    
    /*public string[] tier1Skills = { "Power +10", "Heal +10", "Speed +10" };
    public string[] tier2Skills = { "Defense +10", "Damage +10", "Critical +10" };
    public string[] tier3Skills = { "Ultimate +10", "Regen +10", "Shield +10" };*/
    
    public string[] tier1Skills = { "+5 Power", "+10 Health", "+10 Speed"};
    public string[] tier2Skills = { "+10 Power", "+20 Health", "+15 Speed"};
    public string[] tier3Skills = { "MagicBall (5 Power)", "MagicBall (10 Power)", "MagicBall (15 Power)"};

    public int[] skillTierCosts = { 50, 25, 10};
    public string[] skillTierResources = { "Iron", "Gold", "Diamond" };
    public int refreshShopCost = 10;
    
    public Button[] skillButtons;
    private string[] selectedSkills;

    public GameObject debugCam;
    public GameObject playerCam;
    public GameObject localPlayer;
    
    public int selectedSkillTier;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateShopUI(); //update for first skills; then updating them in PhaseManager when phase changed
    }

    private void Update()
    {
        if (shopCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShopCanvas();
        }
    }

    public void UpdateShopUI() //call it from phaseManager and refresh shop button onclick
    {
        selectedSkills = GetRandomSkills();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].interactable = true;
            skillButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedSkills[i];
            skillButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 
                "Tier " + (i+1) + "\n" + skillTierCosts[i] + " " + skillTierResources[i];
        }
    }

    private string[] GetRandomSkills()
    {
        //Her tier'dan bir tane random skill.
        string[] randomSkillList = new string[3];

        randomSkillList[0] = tier1Skills[Random.Range(0, tier1Skills.Length)]; 
        randomSkillList[1] = tier2Skills[Random.Range(0, tier2Skills.Length)];
        randomSkillList[2] = tier3Skills[Random.Range(0, tier3Skills.Length)];

        return randomSkillList;
    }

    // Bu fonksiyonu butonlara OnClick olayında atayabilirsiniz
    public void OnSkillButtonClicked(int buttonIndex)
    {
        Debug.Log("Button " + buttonIndex + "clicked.");
        selectedSkillTier = buttonIndex;
        ChooseSkill(selectedSkills[buttonIndex]);
    }

    public void RefreshButtonClicked()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                if (player.GetComponent<PlayerStats>().gold >= refreshShopCost)
                {
                    UpdateShopUI();
                    player.GetComponent<PhotonView>().RPC
                        ("RemoveGold", RpcTarget.All, refreshShopCost);
                }
            }
        }
    }
    
    [PunRPC]
    private void ChooseSkill(string skill)
    {
        Debug.Log("Skill Chosen: " + skill + " Tier = " + selectedSkillTier);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                //TIER 1
                if (selectedSkillTier == 0 && player.GetComponent<PlayerStats>().iron >= skillTierCosts[selectedSkillTier])
                {
                    if (skill == "+5 Power")
                    {
                        player.GetComponent<PhotonView>().RPC("PowerUp", RpcTarget.All, 5);
                        player.GetComponent<PhotonView>().RPC("RemoveIron", RpcTarget.All, skillTierCosts[selectedSkillTier]);

                    }
                    else if(skill == "+10 Health")
                    {
                        player.GetComponent<PhotonView>().RPC("Heal", RpcTarget.All, 10);
                        player.GetComponent<PhotonView>().RPC("RemoveIron", RpcTarget.All, skillTierCosts[selectedSkillTier]);

                    }
                    else if(skill == "+10 Speed")
                    {
                        player.GetComponent<PlayerMovementManager>().movementSpeed += 0.3f;
                        //player.transform.GetChild(1).GetComponent<Animator>()
                        player.GetComponent<PhotonView>().RPC("RemoveIron", RpcTarget.All, skillTierCosts[selectedSkillTier]);

                    }

                    SetSkillSold();
                    
                }
                
                //TIER 2
                if (selectedSkillTier == 1 && player.GetComponent<PlayerStats>().gold >= skillTierCosts[selectedSkillTier])
                {
                    if (skill == "+10 Power")
                    {
                        player.GetComponent<PhotonView>().RPC("PowerUp", RpcTarget.All, 10);
                        player.GetComponent<PhotonView>().RPC("RemoveGold", RpcTarget.All, skillTierCosts[selectedSkillTier]);
                    }
                    else if(skill == "+20 Health")
                    {
                        player.GetComponent<PhotonView>().RPC("Heal", RpcTarget.All, 20);
                        player.GetComponent<PhotonView>().RPC("RemoveGold", RpcTarget.All, skillTierCosts[selectedSkillTier]);
                    }
                    else if(skill == "+15 Speed")
                    {
                        player.GetComponent<PlayerMovementManager>().movementSpeed += 0.45f;
                        player.GetComponent<PhotonView>().RPC("RemoveGold", RpcTarget.All, skillTierCosts[selectedSkillTier]);

                    }
                    
                    SetSkillSold();
                }
                
                //TIER 3 
                if(selectedSkillTier == 2 && player.GetComponent<PlayerStats>().diamond >= skillTierCosts[selectedSkillTier])
                {
                    if (skill == "MagicBall (5 Power)")
                    {
                        player.GetComponent<PlayerCombatManager>().hasMagicBallSkill = true;
                        player.GetComponent<PlayerStats>().magicBallDamage = 5;
                        player.GetComponent<PhotonView>().RPC("RemoveDiamond", RpcTarget.All, skillTierCosts[selectedSkillTier]);
                    }
                    else if(skill == "MagicBall (10 Power)")
                    {
                        player.GetComponent<PlayerCombatManager>().hasMagicBallSkill = true;
                        player.GetComponent<PlayerStats>().magicBallDamage = 10;
                        player.GetComponent<PhotonView>().RPC("RemoveDiamond", RpcTarget.All, skillTierCosts[selectedSkillTier]);
                    }
                    else if(skill == "MagicBall (15 Power)")
                    {
                        player.GetComponent<PlayerCombatManager>().hasMagicBallSkill = true;
                        player.GetComponent<PlayerStats>().magicBallDamage = 15;
                        player.GetComponent<PhotonView>().RPC("RemoveDiamond", RpcTarget.All, skillTierCosts[selectedSkillTier]);
                    }
                    
                    SetSkillSold();
                }
            }
        }
    }

    public void SetSkillSold()
    {
        selectedSkills[selectedSkillTier] = "Skill Sold!";
        skillButtons[selectedSkillTier].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 
            selectedSkills[selectedSkillTier];
        skillButtons[selectedSkillTier].interactable = false;
    }

    public void OpenShopCanvas(GameObject _localPlayer) // calling from merchant.cs
    {
        localPlayer = _localPlayer;
        playerCam = localPlayer.transform.GetChild(0).GetChild(0).gameObject; //CameraHolder
        debugCam.transform.position = playerCam.transform.position;
        debugCam.transform.rotation = playerCam.transform.rotation;
        
        localPlayer.GetComponent<PlayerMovementManager>().enabled = false;
        localPlayer.transform.GetChild(1).GetComponent<Animator>().ResetTrigger("run");
        playerCam.SetActive(false);
        debugCam.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        shopCanvas.SetActive(true);
    }
    
    public void CloseShopCanvas() // calling from closebutton onClick
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        localPlayer.GetComponent<PlayerMovementManager>().enabled = true;
        playerCam.SetActive(true);
        debugCam.SetActive(false);
        shopCanvas.SetActive(false);
    }
}
