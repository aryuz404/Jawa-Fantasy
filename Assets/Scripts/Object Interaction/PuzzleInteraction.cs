using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Analytics;
using TMPro;

public class PuzzleInteraction : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject visualCue;
    [SerializeField] private GameObject explorationPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private List<Button> options;
    [SerializeField] private TextMeshProUGUI optionAText;
    [SerializeField] private TextMeshProUGUI optionBText;
    [SerializeField] private TextMeshProUGUI optionCText;
    [SerializeField] private TextMeshProUGUI optionDText;
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private TextMeshProUGUI correctResult;
    [SerializeField] private TextMeshProUGUI wrongResult;
    [SerializeField] private TextMeshProUGUI guessFinished;
    [SerializeField] private TextMeshProUGUI expGained;
    [SerializeField] private TextMeshProUGUI goldGained;
    [SerializeField] private GameObject joystick;
    [SerializeField] private CameraController cameraManager;

    [Header("Set Option")]
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;
    public string answer;

    [Header("Reward")]
    private int expAdded;
    private int coinAdded;
    private int totalExp;
    private int totalCoin;

    private bool playerInRange;
    private bool answered;
    private bool guessedTrue;

    

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
        puzzlePanel.SetActive(false);

        for(int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }
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

    public void StartGuessing()
    {
        if(playerInRange && !PlayerUI.instance.inventoryMenu.activeInHierarchy)
        {
            StartGuess();
            SetGuessing();
            AudioManager.instance.PlaySFX(9);

            FirebaseAnalytics.LogEvent("trigger", new Parameter("type", "animal"));
        }
    }

    public void SetGuessing()
    {
        optionAText.text = optionA;
        optionBText.text = optionB;
        optionCText.text = optionC;
        optionDText.text = optionD;
        answerText.text = answer;

        answered = false;
        guessedTrue = false;
    }

    public void StartGuess()
    {
        AudioManager.instance.PlaySFX(9);
        
        puzzlePanel.SetActive(true);
        explorationPanel.SetActive(false);
        joystick.SetActive(false);
        if(this.gameObject.tag == "Fish")
        {
            cameraManager.StartBattleCamera(-2f);
        }
        else
        {
            cameraManager.StartBattleCamera(2f);
        }
    
    }

    public void EndGuess()
    {
        StartCoroutine(ShowReward());
        
        puzzlePanel.SetActive(false);
        explorationPanel.SetActive(true);
        joystick.SetActive(true);
        cameraManager.EndBattleCamera();
    
    }

    private void OnClick(Button button)
    {
        if(!answered)
        {
            answered = true;

            if(button.GetComponentInChildren<TextMeshProUGUI>().text == answer)
            {
                //jawaban benar

                AudioManager.instance.PlaySFX(13);

                StartCoroutine("ShowResult");
                correctResult.gameObject.SetActive(true);
                wrongResult.gameObject.SetActive(false);
                answerText.gameObject.SetActive(true);

                expAdded = 0;
                coinAdded = 0;

                expAdded += 500;
                coinAdded += 20;
                guessedTrue = true;

                FirebaseAnalytics.LogEvent("answer", new Parameter("type", "correct"));
                
            }
            else
            {
                //jawaban salah

                AudioManager.instance.PlaySFX(12);

                StartCoroutine("ShowResult");
                correctResult.gameObject.SetActive(false);
                wrongResult.gameObject.SetActive(true);
                answerText.gameObject.SetActive(true);

                FirebaseAnalytics.LogEvent("answer", new Parameter("type", "wrong"));
            }
        }
    }

    private IEnumerator ShowResult()
    {
        questionPanel.SetActive(false);
        resultPanel.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        questionPanel.SetActive(true);
        resultPanel.SetActive(false);
        EndGuess();

        totalExp = expAdded;
        totalCoin = coinAdded;

        PlayerStats playerStats = GameManager.instance.playerStats;

        playerStats.AddExp(expAdded);
        GameManager.instance.AddGold(coinAdded);

        // expAdded = 0;
        // coinAdded = 0;

        if(guessedTrue)
        {
            this.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, 1.0f);
        }
                
    }

    private IEnumerator ShowReward()
    {
        rewardPanel.SetActive(true);
        guessFinished.text = "Tebak-tebakan Selesai";
        expGained.text = "Exp didapatkan : " + expAdded;
        goldGained.text = "Koin didapatkan : " + coinAdded;

        yield return new WaitForSeconds(1.5f);

        rewardPanel.SetActive(false);
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
