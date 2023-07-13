using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase variable
    [Header("Firebase")]
    //public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;


    private void Awake()
    {
        // Check that all of the necessary dependencies for firebase are present on the system
        
    }

    private void Start() 
    {

        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        DependencyStatus dependencyStatus = dependencyTask.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                yield return new WaitForEndOfFrame();
                StartCoroutine(CheckForAutoLogin());
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

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckForAutoLogin()
    {
        if(user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            MainMenu.instance.OpenLoginPanel();
        }
    }

    private void AutoLogin()
    {
        if(user != null)
        {
            References.userName = user.DisplayName;
            MainMenu.instance.OpenMenuPanel();
            MainMenu.instance.accountInfo.text = "Masuk sebagai " + user.DisplayName;
        }
        else
        {
            MainMenu.instance.OpenLoginPanel();
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
}
