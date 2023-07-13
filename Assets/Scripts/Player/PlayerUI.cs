using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject explorationPanel;
    private PlayerStats playerStats;

    [SerializeField] private Color normalCol, shieldCol;

    public TextMeshProUGUI levelText, goldText;
    public Slider playerSlider;
    public Image sliderFillArea;
    //public Slider battlePlayerSlider;

    public ItemButton[] itemButtons;

    public static PlayerUI instance;
    public string selectedItem;
    public Item activeItem;

    private void Awake() {
        
    }
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        explorationPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMainStats();
    }

    public void OpenInventory()
    {
        // if(!DialogueManager.GetInstance().dialogueIsPlaying)
        // {
        //     AudioManager.instance.PlaySFX(4);
        //     inventoryMenu.SetActive(true);
        // }

        AudioManager.instance.PlaySFX(4);
        inventoryMenu.SetActive(true);
    }

    public void CloseInventory()
    {
        // if(!DialogueManager.GetInstance().dialogueIsPlaying)
        // {
        //     AudioManager.instance.PlaySFX(10);
        //     inventoryMenu.SetActive(false);
        // }

        AudioManager.instance.PlaySFX(10);
        inventoryMenu.SetActive(false);
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        levelText.text = "Lv. " + playerStats.playerLevel;
        playerSlider.maxValue = playerStats.maxHP;
        playerSlider.value = playerStats.currentHP;
        if(playerStats.armorPower > 0 && playerStats.currentHP > 0)
        {
            sliderFillArea.color = shieldCol;
        }
        else if(playerStats.armorPower <= 0 && playerStats.currentHP > 0)
        {
            sliderFillArea.color = normalCol;
        }
        goldText.text = GameManager.instance.currentGold.ToString();
    }

    public void ShowItems()
    {

        GameManager.instance.SortItems();


        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                //Debug.Log(GameManager.instance.itemsHeld[i]);
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
                itemButtons[i].itemName.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemName;
                itemButtons[i].itemDescription.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).description;

            }
            else
            {
                //Debug.Log("Item show start 2");
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
                itemButtons[i].itemName.text = "";
                itemButtons[i].itemDescription.text = "";
            }
        }

        //Debug.Log("Item showed");
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

    }

    public void UseItem()
    {
        activeItem.Use();

    }
}
