using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI itemName; 
    public TextMeshProUGUI itemDescription;
    public GameObject useButton;
    public GameObject buyButton;
    public int buttonValue;
    public bool buttonPressed;


    // Start is called before the first frame update
    void Start()
    {
        useButton.SetActive(false);
        buyButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        if(PlayerUI.instance.inventoryMenu.activeInHierarchy)
        {
            if(GameManager.instance.itemsHeld[buttonValue] != "")
            {
                PlayerUI.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
                useButton.SetActive(true);
            }
            else
            {
                useButton.SetActive(false);
            }
        }
        
        
    }

    public void PressToBuy()
    {
        if(Shop.instance.shopMenu.activeInHierarchy)
        {
            
            Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            buyButton.SetActive(true);            
        }
        else
        {
            buyButton.SetActive(false);
        }
    }

}
