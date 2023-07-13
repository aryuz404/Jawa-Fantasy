using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 3f;
    public string areaTransitionName;
    public GameObject joystick;
    public GameObject explorationPanel;

    private void Start() 
    {
        joystick = FindObjectOfType<FixedJoystick>().gameObject;
        explorationPanel = FindObjectOfType<ExplorationPanel>().gameObject;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNewNameLevel()
    {
        joystick.SetActive(false);
        StartCoroutine(LoadNameLevel(LevelSelect.instance.areaTransitionName));

        StartCoroutine(LoadJoystick());
        
    }

    public void LoadHometownLevel()
    {
        joystick.SetActive(false);
        StartCoroutine(LoadNameLevel("Hometown"));

        StartCoroutine(LoadJoystick());
        
    }


    public void LoadNewScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void BackToMenu()
    {
        StartCoroutine(LoadNameLevel("Main Menu"));
    }

    public void CancelMoveArea()
    {
        joystick.SetActive(true);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //play animation
        transition.SetTrigger("Start");

        //wait for animation
        yield return new WaitForSeconds(transitionTime);

        //load scene
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadNameLevel(string sceneName)
    {
        //play animation
        transition.SetTrigger("Start");

        //wait for animation
        yield return new WaitForSeconds(transitionTime);

        //load scene
        SceneManager.LoadScene(sceneName);

        PlayerMovement.instance.areaTransitionName = areaTransitionName;

    }

    IEnumerator LoadJoystick()
    {
        yield return new WaitForSeconds(2f);

        joystick.SetActive(true);
        explorationPanel.SetActive(true);
    }
}
