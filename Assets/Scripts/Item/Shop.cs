using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using TMPro;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    [SerializeField] private GameObject joystick;

    public TextMeshProUGUI goldText;
    public string[] itemsForSale;
    public ItemButton[] buyItemButtons;
    public Item selectedItem;
    public TextMeshProUGUI buyItemName, buyItemDescription, buyItemValue;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        goldText.text = GameManager.instance.currentGold.ToString();

        joystick = FindObjectOfType<FixedJoystick>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShop()
    {
        //buyItemButtons[0].Press();

        shopMenu.SetActive(true);
        //deactive joystick
        joystick.SetActive(false);
        AudioManager.instance.PlaySFX(9);

        for(int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;


            if(itemsForSale[i] != "")
            {
                //Debug.Log(GameManager.instance.itemsHeld[i]);
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
                buyItemButtons[i].itemName.text = GameManager.instance.GetItemDetails(itemsForSale[i]).itemName;
                buyItemButtons[i].itemDescription.text = GameManager.instance.GetItemDetails(itemsForSale[i]).description;

            }
            else
            {
                //Debug.Log("Item show start 2");
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
                buyItemButtons[i].itemName.text = "";
                buyItemButtons[i].itemDescription.text = "";
            }
        }
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        //active joystick
        joystick.SetActive(true);
        AudioManager.instance.PlaySFX(9);
    }

    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        //buyItemName.text = selectedItem.itemName;
        //buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Harga: " + selectedItem.coinValue.ToString() + " koin";
    }

    public void BuyItem()
    {
        if(GameManager.instance.currentGold >= selectedItem.coinValue)
        {
            GameManager.instance.currentGold -= selectedItem.coinValue;

            AudioManager.instance.PlaySFX(14);

            GameManager.instance.AddItem(selectedItem.itemName);

            FirebaseAnalytics.LogEvent("buy", new Parameter("type", "item " + selectedItem.itemName.ToString()));
        }

        goldText.text = GameManager.instance.currentGold.ToString();
    }
}
