using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using UnityEngine.SceneManagement;


public class QuizManager : MonoBehaviour
{

    private List<QuestionData> questionDatas = new List<QuestionData>();
    
    private QuestionData selectedQuestion = new QuestionData();    

    [SerializeField] private QuizUI quizUI;
    private QuizTrigger quizTrigger;
    public GameObject[] enemies;
    
    public int enemyMaxHealth = 100;
    public int enemyCurrentHealth;
    public int expAdded;
    public int coinAdded;
    public int totalExp;
    public int totalCoin;
    public int totalScore;
    public int level1Score;
    public int level2Score;
    public int level3Score;
    private int damage = 20;
    private int questionGiven = 0;
    private int enemiesAttacked = 0;
    //private bool canAnswer = false;
    private string sceneName;

    public static QuizManager instance;
    

    // Start is called before the first frame update
    void Start()
    {

        enemyCurrentHealth = enemyMaxHealth;
        quizUI.SetEnemyMaxHealth(enemyMaxHealth);

        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectQuestion()
    {
        
        int val = UnityEngine.Random.Range(0, FirebaseManager.Instance.questionDatas.Count);
        selectedQuestion = FirebaseManager.Instance.questionDatas[val];

        quizUI.SetQuestion(selectedQuestion);

        FirebaseManager.Instance.questionDatas.RemoveAt(val);

        Debug.Log("Question Given : " + questionGiven);
        questionGiven++;
        Debug.Log("Question Given after : " + questionGiven);
        

    }

    public bool Answer(string answered)
    {
        Debug.Log("fungsi answer jalan");
        bool answer = false;

        PlayerStats playerStats = GameManager.instance.playerStats;

        
        if(answered == selectedQuestion.answer)
        {
            //correct
            answer = true;
            AudioManager.instance.PlaySFX(13);
            AudioManager.instance.PlaySFX(0);
            AudioManager.instance.PlaySFX(8);
            enemyCurrentHealth -= damage;
            quizUI.SetEnemyHealth(enemyCurrentHealth);
            expAdded += 500;
            coinAdded += 20;
            totalScore += 5;

            FirebaseAnalytics.LogEvent("answer", new Parameter("type", "correct"));
        }
        else
        {
            //wrong
            if(playerStats.armorPower > 0)
            {
                playerStats.armorPower -= damage;
            }
            else
            {
                playerStats.currentHP -= damage;
            }

            AudioManager.instance.PlaySFX(12);
            AudioManager.instance.PlaySFX(0);
            AudioManager.instance.PlaySFX(8);

            FirebaseAnalytics.LogEvent("answer", new Parameter("type", "wrong"));
            
        }

        if(FirebaseManager.Instance.questionDatas.Count > 0 && playerStats.currentHP > 0 && enemyCurrentHealth > 0 && questionGiven < 4)
        {
            

            Invoke("SelectQuestion", 3.0f);

        }
        else if(FirebaseManager.Instance.questionDatas.Count > 0 && playerStats.currentHP <= 0 && enemyCurrentHealth > 0 && enemiesAttacked != 5)
        {
            quizUI.Lose();

            FirebaseAnalytics.LogEvent("score", new Parameter("type", "lose"));
            
        }
        else
        {
            StartCoroutine(EndQuiz());
            
            enemiesAttacked++;

            totalExp = expAdded;
            totalCoin = coinAdded;

            playerStats.AddExp(expAdded);
            GameManager.instance.AddGold(coinAdded);

            questionGiven = 0;
            expAdded = 0;
            coinAdded = 0;

            if(enemiesAttacked == 5)
            {
                quizUI.Win();

                var activeScene = SceneManager.GetActiveScene();
                sceneName = activeScene.name;

                if(sceneName == "Paramasastra")
                {
                    level1Score = totalScore;
                    FirebaseAnalytics.LogEvent("score", new Parameter("type", "level Paramasastra " + level1Score.ToString()));
                }
                else if(sceneName == "Kawruh Basa")
                {
                    level2Score = totalScore;
                    FirebaseAnalytics.LogEvent("score", new Parameter("type", "level Kawruh Basa " + level2Score.ToString()));
                }
                else if(sceneName == "Kawruh Wigati")
                {
                    level3Score = totalScore;
                    FirebaseAnalytics.LogEvent("score", new Parameter("type", "level Kawruh Wigati " + level3Score.ToString()));
                }

                //FirebaseAnalytics.LogEvent("score", new Parameter("type", "level " + sceneName + " " + totalScore.ToString()));
                
            }
            
        }

        return answer;
    }

    IEnumerator EndQuiz()
    {
        yield return new WaitForSeconds(3.0f);
        
        quizUI.EndQuiz();

        enemyCurrentHealth = enemyMaxHealth;
        quizUI.SetEnemyMaxHealth(enemyMaxHealth);
    }
}

[System.Serializable]
public class Questions
{
    public string name;

    public string question;

    public string imageFile;

    public string option_a;

    public string option_b;

    public string option_c;

    public string option_d;

    public string answer;
}
