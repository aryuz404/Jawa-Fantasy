using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] private GameObject dictionaryPanel;
    [SerializeField] private GameObject joystick;

    //private DictionaryUI dictionaryUI;

    private DictionaryData selectedDictionary = new DictionaryData();
    private bool canOpen;
    private int currentDictionary = 0;

    private void Awake() 
    {
        canOpen = false;
        visualCue.SetActive(false);
        dictionaryPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseManager.Instance.GetDictionaryData();
        joystick = FindObjectOfType<FixedJoystick>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpen)
        {
            visualCue.SetActive(true);
            
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    public void OpenDictionary()
    {
        if(canOpen && !PlayerUI.instance.inventoryMenu.activeInHierarchy)
        {
            dictionaryPanel.SetActive(true);
            joystick.SetActive(false);

            AudioManager.instance.PlaySFX(9);

            SelectDictionary();

            FirebaseAnalytics.LogEvent("open", new Parameter("type", "dictionary"));
        }
        
    }

    public void CloseDictionary()
    {
        dictionaryPanel.SetActive(false);
        joystick.SetActive(true);
        AudioManager.instance.PlaySFX(9);
    }

    public void SelectDictionary()
    {
        selectedDictionary = FirebaseManager.Instance.dictionaryDatas[currentDictionary];

        DictionaryUI.instance.SetDictionary(selectedDictionary);
    }

    public void NextDictionary()
    {
        if(currentDictionary < FirebaseManager.Instance.dictionaryDatas.Count)
        {
            currentDictionary++;
            AudioManager.instance.PlaySFX(9);
            SelectDictionary();
        }
    }

    public void PreviousDictionary()
    {
        if(currentDictionary > 0)
        {
            currentDictionary--;
            AudioManager.instance.PlaySFX(9);
            SelectDictionary();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
