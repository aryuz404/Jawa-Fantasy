using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool SettingIsOpen = false;

    public GameObject pausePanel;
    //public GameObject joystick;

    private void Start() 
    {
        //joystick = FindObjectOfType<FixedJoystick>().gameObject;
    }

    //public GameObject settingsPanel;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        //joystick.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        if(!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            //joystick.SetActive(true);
        }
        
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
