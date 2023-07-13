using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public string playerName;
    public int playerLevel = 1;
    public int currentEXP;
    public int baseEXP = 1000;
    public int[] expToNextLevel;
    public int maxLevel = 10;

    public int currentHP;
    public int maxHP = 100;
    public int armorPower = 0;
    public string equippedArmor;
    public Sprite playerImage;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(500);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;
        if(playerLevel < maxLevel)
        {
            if(currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //currentHP = maxHP;
            }
        }
        
        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
