using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isArmor;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int coinValue;
    public Sprite itemSprite;

    [Header("Item Effect Details")]
    public int amountToChange;
    public bool affectHP;
    public bool affectQuizOption;

    [Header("Armor Effect Details")]
    public int armorStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        PlayerStats player = GameManager.instance.playerStats;

        if(isItem)
        {
            if(affectHP)
            {
                player.currentHP += amountToChange;

                if(player.currentHP > player.maxHP)
                {
                    player.currentHP = player.maxHP;
                }

                FirebaseAnalytics.LogEvent("use", new Parameter("type", "jamu"));
            }

            if(affectQuizOption)
            {
                //search for correct answer
                //disable two button except correct answer
            }
        }

        if(isArmor)
        {
            if(player.equippedArmor != "")
            {
                GameManager.instance.AddItem(player.equippedArmor);
            }

            player.equippedArmor = itemName;
            player.armorPower = armorStrength;

            FirebaseAnalytics.LogEvent("use", new Parameter("type", "perisai"));
        }

        AudioManager.instance.PlaySFX(9);

        GameManager.instance.RemoveItem(itemName);
    }
}
