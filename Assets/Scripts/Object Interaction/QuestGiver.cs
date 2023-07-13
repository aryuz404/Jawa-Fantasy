using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject joystick;
    private bool canOpen;

    private void Awake() 
    {
        canOpen = false;
        visualCue.SetActive(false);
        questPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<FixedJoystick>().gameObject;
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

    public void OpenQuestMenu()
    {
        if(canOpen && !PlayerUI.instance.inventoryMenu.activeInHierarchy)
        {
            questPanel.SetActive(true);
            joystick.SetActive(false);
            AudioManager.instance.PlaySFX(9);
        }
        
    }

    public void CloseQuestMenu()
    {
        questPanel.SetActive(false);
        joystick.SetActive(true);
        AudioManager.instance.PlaySFX(9);
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
