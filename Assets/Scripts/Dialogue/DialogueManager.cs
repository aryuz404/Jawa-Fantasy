using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject explorationPanel;
    [SerializeField] private GameObject joystick;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;

    private Animator layoutAnimator;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;
    private bool canSkip = false;
    private bool submitSkip;

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private void Awake() 
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene!");
        }

        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start() 
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //get layout animator
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        //if(Input.touchCount > 0)
        {
            //Input.GetTouch(0);
            submitSkip = true;
        }

       //return if dialogue not playing
       if(!dialogueIsPlaying)
       {
            return;
       }

       if(canContinueToNextLine && Input.GetMouseButtonDown(0))
       //if(canContinueToNextLine && Input.touchCount > 0)
       {
            //Input.GetTouch(0);
            AudioManager.instance.PlaySFX(9);
            ContinueStory();
       }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        //explorationPanel.SetActive(false);
        joystick.SetActive(false);
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        //explorationPanel.SetActive(true);
        joystick.SetActive(true);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if(currentStory.canContinue)
        {
            //set text for the current dialogue line
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            //handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        //empty the dialogue text
        dialogueText.text = "";
        canContinueToNextLine = false;
        submitSkip = false;

        StartCoroutine(CanSkip());

        //display each letter one at a time
        foreach(char letter in line.ToCharArray())
        {
            //AudioManager.instance.PlaySFX(11);

            //if the submit button it pressed, finish up displaying line right away
            if(canSkip && submitSkip)
            {
                submitSkip = false;
                dialogueText.text = line;
                AudioManager.instance.StopSFX();
                break;
            }

            dialogueText.text += letter;
            
            yield return new WaitForSeconds(typingSpeed);
        }

        canContinueToNextLine = true;
        
        canSkip = false;
    }

    private IEnumerator CanSkip()
    {
        canSkip = false;
        yield return new WaitForSeconds(0.05f);
        canSkip = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        //loop through each tag and handle it accordingly
        foreach(string tag in currentTags)
        {
            //parse the tag
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle the tag
            switch(tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
}
