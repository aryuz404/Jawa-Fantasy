using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class QuizTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Manager")]
    [SerializeField] private QuizManager quizManager;
    [SerializeField] private QuizUI quizUI;
    [SerializeField] private CameraController cameraManager;

    [Header("Question Collection")]
    public string levelPart;
    public string questionCollection;
    public string questionEnemyName;

    private bool playerInRange;

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Start() 
    {
        FirebaseManager.Instance.GetData(levelPart, questionCollection, questionEnemyName);

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

    public void StartQuizing()
    {
        if(playerInRange && !PlayerUI.instance.inventoryMenu.activeInHierarchy)
        {
            AudioManager.instance.PlaySFX(15);
            quizManager.SelectQuestion();
            StartCoroutine(LoadBattle());
            //Debug.Log(this.gameObject.GetInstanceID().ToString() + " called");
            FirebaseAnalytics.LogEvent("trigger", new Parameter("type", "enemy"));
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

    IEnumerator LoadBattle()
    {
        quizUI.StartQuiz();
        yield return new WaitForSeconds(1f);
        //quizManager.SelectQuestion();
        quizUI.AfterStartLoad();
        this.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1.0f);
    }

}
