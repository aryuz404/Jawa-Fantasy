using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class Shopkeeper : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    private bool canOpen;
    public string[] itemsForSale = new string[3];

    //private bool playerInRange;

    private void Awake() 
    {
        canOpen = false;
        visualCue.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpen)
        {
            visualCue.SetActive(true);
            
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    public void OpenShop()
    {
        if(canOpen && !Shop.instance.shopMenu.activeInHierarchy && !PlayerUI.instance.inventoryMenu.activeInHierarchy)
        {
            
            Shop.instance.itemsForSale = itemsForSale;
            Shop.instance.OpenShop();

            FirebaseAnalytics.LogEvent("open", new Parameter("type", "shop"));
        }
    }



    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
