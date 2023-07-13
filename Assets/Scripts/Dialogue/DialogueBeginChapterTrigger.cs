using UnityEngine;

public class DialogueBeginChapterTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset initialTextAsset;
   
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartChapter), 0.02f);
        //Destroy(this);
    }

    private void StartChapter()
    {
        DialogueManager.GetInstance().gameObject.SetActive(true);
        DialogueManager.GetInstance().EnterDialogueMode(initialTextAsset);
    }

    
}
