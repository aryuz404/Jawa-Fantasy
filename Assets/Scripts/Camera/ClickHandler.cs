using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // if(Input.touchCount > 0)
        if(Input.GetMouseButtonDown(0))
        {
            // Touch touch = Input.GetTouch(0);
            // Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero);
            if(hit2D.collider != null)
            {
                Debug.Log(hit2D.collider.name);
                DialogueTrigger dialogueTrigger = hit2D.collider.GetComponent<DialogueTrigger>();
                QuestGiver questGiver = hit2D.collider.GetComponent<QuestGiver>();
                Shopkeeper shopKeeper = hit2D.collider.GetComponent<Shopkeeper>();
                Dictionary dictionary = hit2D.collider.GetComponent<Dictionary>();
                MoveAreaInteraction moveAreaInteraction = hit2D.collider.GetComponent<MoveAreaInteraction>();
                PuzzleInteraction puzzleInteraction = hit2D.collider.GetComponent<PuzzleInteraction>();
                QuizTrigger quizTrigger = hit2D.collider.GetComponent<QuizTrigger>();

                if(dialogueTrigger)
                {
                    dialogueTrigger.StartDialogue();
                }
                else if(questGiver)
                {
                    questGiver.OpenQuestMenu();
                }
                else if(shopKeeper)
                {
                    shopKeeper.OpenShop();
                }
                else if(dictionary)
                {
                    dictionary.OpenDictionary();
                }
                else if(moveAreaInteraction)
                {
                    moveAreaInteraction.OpenGate();
                }
                else if(puzzleInteraction)
                {
                    puzzleInteraction.StartGuessing();
                }
                else if(quizTrigger)
                {
                    quizTrigger.StartQuizing();
                }

            }
        }
    }

}
