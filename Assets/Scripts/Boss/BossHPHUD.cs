using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPHUD : MonoBehaviour
{
    public Slider HPBar;
    public GameObject boss;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(boss){
            var maxHP = (float)boss.GetComponent<BossControler>().maxHP;
            var currentHP = (float)boss.GetComponent<BossControler>().currentHP;
            HPBar.value = currentHP/maxHP;
        }else{
            Destroy(gameObject);
        }
    }
}
