using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Space]
    [Header("Menu Panel")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject logOrRegPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    public TextMeshProUGUI accountInfo;

    // Login Variables
    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    [Space]
    [Header("Feedback Panel")]
    public GameObject nameEmptyPanel;
    public GameObject passwordEmptyPanel;
    public GameObject emailEmptyPanel;
    public GameObject passwordNotMatchPanel;
    public GameObject emailOrPassWrongPanel;
    public GameObject registrationFailedPanel;
    public GameObject registrationSuccessPanel;
    public GameObject loginSuccessPanel;
    public TextMeshProUGUI accountName;

    public static MainMenu instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenuPanel()
    {
        menuPanel.SetActive(true);
        loginPanel.SetActive(false);
        logOrRegPanel.SetActive(false);
        registerPanel.SetActive(false);
    }

    public void OpenLogOrRegPanel()
    {
        menuPanel.SetActive(false);
        loginPanel.SetActive(false);
        logOrRegPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void OpenLoginPanel()
    {
        menuPanel.SetActive(false);
        logOrRegPanel.SetActive(false);
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void OpenRegisterPanel()
    {
        menuPanel.SetActive(false);
        loginPanel.SetActive(false);
        logOrRegPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void ClearLoginInputFieldText()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }

    public void ClearRegisterInputFieldText()
    {
        nameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        confirmPasswordRegisterField.text = "";
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
