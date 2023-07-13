using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI optionAText;
    [SerializeField] private TextMeshProUGUI optionBText;
    [SerializeField] private TextMeshProUGUI optionCText;
    [SerializeField] private TextMeshProUGUI optionDText;
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private Image imageHolder;
    [SerializeField] private Image sliderFillArea;
    [SerializeField] private TextMeshProUGUI correctResult;
    [SerializeField] private TextMeshProUGUI wrongResult;
    [SerializeField] private TextMeshProUGUI quizFinished;
    [SerializeField] private TextMeshProUGUI expGained;
    [SerializeField] private TextMeshProUGUI goldGained;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private List<Button> options;
    [SerializeField] private Color correctCol, wrongCol, normalCol, normalHPCol, shieldCol;
    [SerializeField] private Slider playerSlider, enemySlider;
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject hpPanel;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject explorationPanel;
    [SerializeField] private GameObject joystick;
    //[SerializeField] private Animator battleLoad;
    [SerializeField] private GameObject battleScene;
    [SerializeField] private Animator player;
    [SerializeField] private Animator enemy;
    [SerializeField] private Animator playerAttack;
    [SerializeField] private Animator enemyAttack;
    [SerializeField] private Transform playerPos;
 
    private QuestionData questionData;

    public FirebaseUser user;
    
    private bool answered;

    private PlayerStats playerStats;

    [SerializeField] private QuizManager quizManager;
    [SerializeField] private CameraController cameraController;
    //[SerializeField] private FirebaseManager firebaseManager;

    List<QuestionData> questionDatas = new List<QuestionData>();

    void Awake()
    {
        for(int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }
    }

    private void Start() {
        joystick = FindObjectOfType<FixedJoystick>().gameObject;
        playerPos = FindObjectOfType<PlayerMovement>().transform;
    }


    public void SetQuestion(QuestionData questionData)
    {
        this.questionData = questionData;
        //int val = UnityEngine.Random.Range(0, questionDatas.Count);

        if(questionData.imageFile == "none")
        {
            FirebaseManager.Instance.questionImg.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            FirebaseManager.Instance.questionImg.transform.parent.gameObject.SetActive(true);
            FirebaseManager.Instance.questionImg.transform.gameObject.SetActive(true);
            FirebaseManager.Instance.GetImageData(questionData.imageFile);
        }

        //References.userName = user.DisplayName;

        //playerNameText.text = user.DisplayName;
        enemyNameText.text = questionData.name;
        questionText.text = questionData.question;
        optionAText.text = questionData.option_a;
        optionBText.text = questionData.option_b;
        optionCText.text = questionData.option_c;
        optionDText.text = questionData.option_d;
        answerText.text = questionData.answer;

        for (int i = 0; i < options.Count; i++)
        {
            options[i].image.color = normalCol;
        }

        answered = false;

    }

    private void Update() 
    {
        UpdateBattleStat();
        
        battleScene.transform.position = playerPos.position;
    }

    private void OnClick(Button button)
    {
        if(!answered)
        {
            answered = true;

            bool val = quizManager.Answer(button.GetComponentInChildren<TextMeshProUGUI>().text);

            if(val)
            {
                button.image.color = correctCol;
                StartCoroutine("ShowResult");
                correctResult.gameObject.SetActive(true);
                wrongResult.gameObject.SetActive(false);
                answerText.gameObject.SetActive(true);
                player.SetTrigger("Attack");
                playerAttack.SetTrigger("Attack");
                enemy.SetTrigger("Hit");

                StartCoroutine(IdleAnimation());
            }
            else
            {
                button.image.color = wrongCol;
                StartCoroutine("ShowResult");
                correctResult.gameObject.SetActive(false);
                wrongResult.gameObject.SetActive(true);
                answerText.gameObject.SetActive(true);
                enemy.SetTrigger("Attack");
                enemyAttack.SetTrigger("Attack");
                player.SetTrigger("Hit");

                StartCoroutine(IdleAnimation());
            }
        }
        
        Debug.Log(button.GetComponentInChildren<TextMeshProUGUI>().text);
        

        //Debug.Log("Listener on");
    }

    public void UpdateBattleStat()
    {
        playerStats = GameManager.instance.playerStats;
        playerSlider.maxValue = playerStats.maxHP;
        playerSlider.value = playerStats.currentHP;
        if(playerStats.armorPower > 0 && playerStats.currentHP > 0)
        {
            sliderFillArea.color = shieldCol;
        }
        else if(playerStats.armorPower <= 0 && playerStats.currentHP > 0)
        {
            sliderFillArea.color = normalHPCol;
        }
    }

    public void StartQuiz()
    {
        //battleLoad.SetTrigger("Start");
        battleScene.SetActive(true);
        explorationPanel.SetActive(false);
        joystick.SetActive(false);
    }

    public void AfterStartLoad()
    {
        battlePanel.SetActive(true);
        hpPanel.SetActive(true);
    }

    public void EndQuiz()
    {

        StartCoroutine(ShowReward());
        
        battlePanel.SetActive(false);
        hpPanel.SetActive(false);
        explorationPanel.SetActive(true);
        joystick.SetActive(true);
        
        //battleLoad.SetTrigger("End");
        battleScene.SetActive(false);
    }

    public void Lose()
    {
        StartCoroutine(LosePanel());
    }

    public void Win()
    {
        StartCoroutine(WinPanel());
    }

    public void SetEnemyMaxHealth(int enemyHealth)
    {
        enemySlider.maxValue = enemyHealth;
        enemySlider.value = enemyHealth;
    }

    public void SetEnemyHealth(int enemyHealth)
    {
        enemySlider.value = enemyHealth;
    }

    private IEnumerator ShowResult()
    {
        questionPanel.SetActive(false);
        resultPanel.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        questionPanel.SetActive(true);
        resultPanel.SetActive(false);        
    }

    private IEnumerator ShowReward()
    {
        rewardPanel.SetActive(true);
        quizFinished.text = "Perang Selesai";
        expGained.text = "Exp didapatkan : " + quizManager.totalExp;
        goldGained.text = "Koin didapatkan : " + quizManager.totalCoin;

        yield return new WaitForSeconds(1.5f);

        rewardPanel.SetActive(false);
    }

    private IEnumerator IdleAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        enemy.SetTrigger("Idle");
        enemyAttack.SetTrigger("Idle");
        player.SetTrigger("Idle");
        playerAttack.SetTrigger("Idle");
    }

    private IEnumerator WinPanel()
    {
        yield return new WaitForSeconds(5.5f);

        battleScene.SetActive(false);
        winPanel.SetActive(true);
        totalScore.text = "Skor : " + quizManager.totalScore;
        joystick.SetActive(false);
        AudioManager.instance.PlaySFX(6);
    }

    private IEnumerator LosePanel()
    {
        yield return new WaitForSeconds(4f);

        battleScene.SetActive(false);
        losePanel.SetActive(true);
        joystick.SetActive(false);
        AudioManager.instance.PlaySFX(5);
    }

}
