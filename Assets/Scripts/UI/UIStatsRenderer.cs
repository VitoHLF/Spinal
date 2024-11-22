using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIStatsRenderer : MonoBehaviour
{
    private GameObject player;
    private int playerHP;
    private int playerMaxHP = 0;
    private float playerEnergy;
    private float playerMaxEnergy;

    [Header("UI Elements")]
    public GameObject[] hearts;
    public GameObject[] heartBGS;
    public Slider energyBar;
    public Image energyFill;

    [Header("Debug")]
    public bool isDebugger = false;
    public TMP_Text hpText;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        playerHP = player.GetComponent<playerBehaviour>().currentHP;
        playerMaxHP = player.GetComponent<playerBehaviour>().maxHP;
        playerEnergy = player.GetComponent<playerBehaviour>().currentEnergy;
        playerMaxEnergy = player.GetComponent<playerBehaviour>().maxEnergy;
        if (isDebugger)
        {
            hpText.text = "HP: " + playerHP.ToString() + "/" + playerMaxHP.ToString() + "\n" +
                            "Energy: " + ((int)playerEnergy).ToString() + "/" + ((int)playerMaxEnergy).ToString();
        }
        
        else{
            for (int i = 0; i < playerMaxHP; i++)                
            {
                if(playerMaxHP>i) heartBGS[i].SetActive(true);
                else heartBGS[i].SetActive(false);
                if(playerHP>i) hearts[i].SetActive(true);
                else hearts[i].SetActive(false);
            }
            energyBar.value = playerEnergy/playerMaxEnergy;

            if(player.GetComponent<playerBehaviour>().GetOverheatStatus()) energyFill.color = Color.red;
            else if(energyBar.value < 0.5) energyFill.color = Color.white;
            else energyFill.color = new Color(1f, 0.33f, 0, 1);
        }


    }
}
