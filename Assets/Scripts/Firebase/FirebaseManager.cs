using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;
using Firebase;
using Firebase.Analytics;
using Firebase.Firestore;
using Firebase.Storage;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;


public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager instance;
    public static FirebaseManager Instance {get {return instance;} }

    private FirebaseFirestore db;
    private FirebaseStorage storage;
    private StorageReference storageReference;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public RawImage questionImg;

    private QuizUI quizUI;
    private QuizTrigger quizTrigger;
    
    public List<QuestionData> questionDatas = new List<QuestionData>();
    public List<DictionaryData> dictionaryDatas = new List<DictionaryData>();

    private PlayerStats playerStats;
    private string userID;
    private bool isConnected;

    //List<QuestionData> questionToAnswer = new List<QuestionData>();

    //int currentQuestion = 0;

    

    private void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isConnected = false;
        Debug.Log("Firebase jalan");
        SetupFirebase();
        StartCoroutine(CheckAndFixDependenciesAsync());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupFirebase()
    {
        db = FirebaseFirestore.DefaultInstance;
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://jawa-fantasy-v3.appspot.com/");

        // FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task=>
        // {
            

            
        // });

        //questions = db.Collection("questions");
    }


    // Authentication

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        DependencyStatus dependencyStatus = dependencyTask.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                yield return new WaitForEndOfFrame();
                CheckAutoLogin();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
            }
    }

    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Debug.Log("Firebase active");

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void CheckAutoLogin()
    {
        StartCoroutine(CheckForAutoLogin());
    }

    private IEnumerator CheckForAutoLogin()
    {
        if(user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            AutoLogin();
            isConnected = true;
        }
        else
        {
            MainMenu.instance.OpenLogOrRegPanel();
        }
    }

    private void AutoLogin()
    {
        if(user != null)
        {
            if(SceneManager.GetActiveScene().name == "Main Menu")
            {
                References.userName = user.DisplayName;
                MainMenu.instance.OpenMenuPanel();
                MainMenu.instance.accountInfo.text = "Masuk sebagai " + user.DisplayName;
                isConnected = true;
            }
            else
            {
                
            }
        }
        else
        {
            MainMenu.instance.OpenLogOrRegPanel();
        }
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                MainMenu.instance.OpenLogOrRegPanel();
                MainMenu.instance.ClearLoginInputFieldText();
                MainMenu.instance.ClearRegisterInputFieldText();
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                
                Debug.Log("Signed in " + user.UserId);
                userID = user.UserId;
                isConnected = true;
                
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(MainMenu.instance.emailLoginField.text, MainMenu.instance.passwordLoginField.text));
    }

    public void Logout()
    {
        if(auth != null && user != null)
        {
            auth.SignOut();
        }
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
            MainMenu.instance.emailOrPassWrongPanel.SetActive(true);
        }
        else
        {
            user = loginTask.Result;

            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);

            MainMenu.instance.loginSuccessPanel.SetActive(true);
            MainMenu.instance.accountName.text = "Masuk sebagai " + user.DisplayName;

            References.userName = user.DisplayName;
            MainMenu.instance.OpenMenuPanel();
            MainMenu.instance.accountInfo.text = "Masuk sebagai " + user.DisplayName;
            isConnected = true;
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(MainMenu.instance.nameRegisterField.text, MainMenu.instance.emailRegisterField.text, MainMenu.instance.passwordRegisterField.text, MainMenu.instance.confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("User Name is empty");
            MainMenu.instance.nameEmptyPanel.SetActive(true);
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
            MainMenu.instance.emailEmptyPanel.SetActive(true);
        }
        else if (MainMenu.instance.passwordRegisterField.text != MainMenu.instance.confirmPasswordRegisterField.text)
        {
            Debug.LogError("Password does not match");
           MainMenu.instance. passwordNotMatchPanel.SetActive(true);
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }

                Debug.Log(failedMessage);
                MainMenu.instance.registrationFailedPanel.SetActive(true);
            }
            else
            {
                // Get The User After Registration Success
                user = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;


                    string failedMessage = "Profile update Failed! Because ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }

                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log("Registration Sucessful Welcome " + user.DisplayName);
                    MainMenu.instance.registrationSuccessPanel.SetActive(true);
                    MainMenu.instance.OpenLoginPanel();
                }
            }
        }
    }


    // Firestore

    public void GetData(string levelPart, string collectionName, string enemyName)
    {
        //Query questionQuery = db.Collection("questions").Document("part_one").Collection("question");
        Query questionQuery = db.Collection("questions").Document(levelPart).Collection(collectionName).WhereEqualTo("name", enemyName);
        questionQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot questionQuerySnapshot = task.Result;
            foreach(DocumentSnapshot documentSnapshot in questionQuerySnapshot.Documents)
            {
                Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));

                               
                var questionData = documentSnapshot.ConvertTo<QuestionData>();
                
                questionDatas.Add(questionData);

            }


        });
    }

    public void GetDictionaryData()
    {
        Query dictionaryQuery = db.Collection("dictionary");
        dictionaryQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot dictionaryQuerySnapshot = task.Result;
            foreach(DocumentSnapshot documentSnapshot in dictionaryQuerySnapshot.Documents)
            {
                Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));

                var dictionaryData = documentSnapshot.ConvertTo<DictionaryData>();

                dictionaryDatas.Add(dictionaryData);
            }
        });
    }

    // public void SaveData()
    // {
    //     if(isConnected)
    //     {
    //         playerStats = GameManager.instance.playerStats;

    //         string name = user.DisplayName;
    //         string email = user.Email;
    //         int level = playerStats.playerLevel;
    //         int currentHP = playerStats.currentHP;
    //         int currentEXP = playerStats.currentEXP;
    //         int currentArmor = playerStats.armorPower;
    //         int currentCoin = GameManager.instance.currentGold;
    //         int level1Score = QuizManager.instance.level1Score;
    //         int level2Score = QuizManager.instance.level2Score;
    //         int level3Score = QuizManager.instance.level3Score;
    //         List<string> itemsHeld = new List<string>();
    //         itemsHeld = GameManager.instance.itemsHeld.ToList();
    //         List<int> numberOfItems = new List<int>();
    //         numberOfItems = GameManager.instance.numberOfItems.ToList();

    //         Dictionary<string, object> saveValues = new Dictionary<string, object>
    //         {
    //             {"name", name},
    //             {"email", email},
    //             {"level", level},
    //             {"currentHP", currentHP},
    //             {"currentEXP", currentEXP},
    //             {"currentArmor", currentArmor},
    //             {"currentCoin", currentCoin},
    //             {"level1Score", level1Score},
    //             {"level2Score", level2Score},
    //             {"level3Score", level3Score},
    //             {"playerInventory", itemsHeld},
    //             {"inventoryCount", numberOfItems},
    //         };

    //         DocumentReference documentReference = db.Collection("PlayerData").Document(userID);
    //         documentReference.SetAsync(saveValues).ContinueWithOnMainThread(task =>
    //         {
    //             if(task.IsCompleted)
    //             {
    //                 Debug.Log("Save Completed");
    //             }
    //             else
    //             {
    //                 Debug.LogError("Error Saving Data");
    //             }
    //         });
    //     }
    //     else
    //     {
    //         Debug.LogError("Save error : Firebase not connected");
    //     }
    // }

    // public void LoadData()
    // {
    //     if(isConnected)
    //     {
    //         DocumentReference documentReference = db.Collection("PlayerData").Document(userID);
    //         documentReference.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    //         {
    //             DocumentSnapshot documentSnapshot = task.Result;
    //             if(documentSnapshot.Exists)
    //             {
    //                 playerStats = GameManager.instance.playerStats;

    //                 playerStats.playerLevel = documentSnapshot.GetValue<int>("level");
    //                 playerStats.currentHP = documentSnapshot.GetValue<int>("currentHP");
    //                 playerStats.currentEXP = documentSnapshot.GetValue<int>("currentEXP");
    //                 playerStats.armorPower = documentSnapshot.GetValue<int>("currentArmor");
    //                 GameManager.instance.currentGold = documentSnapshot.GetValue<int>("currentCoin");
                
    //                 List<string> inventoryList = documentSnapshot.GetValue<List<string>>("playerInventory");
    //                 GameManager.instance.itemsHeld = inventoryList.ToArray();
    //                 // GameManager.instance.itemsHeld[0] = inventoryList[0];
    //                 // GameManager.instance.itemsHeld[1] = inventoryList[1];
    //                 // GameManager.instance.itemsHeld[2] = inventoryList[2];
    //                 List<int> inventoryNumber = documentSnapshot.GetValue<List<int>>("inventoryCount");
    //                 GameManager.instance.numberOfItems = inventoryNumber.ToArray();
    //                 // GameManager.instance.numberOfItems[0] = inventoryNumber[0];
    //                 // GameManager.instance.numberOfItems[1] = inventoryNumber[1];
    //                 // GameManager.instance.numberOfItems[2] = inventoryNumber[2];
    //             }
    //             else
    //             {
    //                 Debug.LogError("Load Failed : No Previous Data");
    //             }
    //         });
    //     }
    //     else
    //     {
    //         Debug.LogError("Load error : Firebase not connected");
    //     }
    // }


    // Storage

    public void GetImageData(string imageFile)
    {
        //StorageReference image = storageReference.Child("ayame.jpg");
        StorageReference image = storageReference.Child(imageFile);

        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if(!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result)));
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    private IEnumerator LoadImage(string MediaURL)
    {
        Debug.Log("saya hadir");
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaURL);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError  || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            //quizUI.questionImg.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            questionImg.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Debug.Log("saya keluar");
        }
    }
}
