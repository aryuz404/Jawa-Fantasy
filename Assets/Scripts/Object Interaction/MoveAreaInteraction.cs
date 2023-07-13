using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveAreaInteraction : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    [SerializeField] private GameObject moveAreaPanel;
    [SerializeField] private GameObject joystick;

    private bool playerInRange;

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
        moveAreaPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<FixedJoystick>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange)
        {
            visualCue.SetActive(true);
        
        }
        else
        {
            visualCue.SetActive(false);
            
        }
    }

    public void OpenGate()
    {
        if(playerInRange && !PlayerUI.instance.inventoryMenu.activeInHierarchy)
        {
            moveAreaPanel.SetActive(true);
            joystick.SetActive(false);
            AudioManager.instance.PlaySFX(9);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
