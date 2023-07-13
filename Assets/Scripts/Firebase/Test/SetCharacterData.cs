using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;


public class SetCharacterData : MonoBehaviour
{
    //[SerializeField] private string characterPath = "character_sheets/cool_dude";

    [SerializeField] private InputField nameField;
    [SerializeField] private InputField descriptionField;
    [SerializeField] private InputField attackField;
    [SerializeField] private InputField defenceField;
    [SerializeField] private Button submitButton;

    FirebaseFirestore db;

    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        submitButton.onClick.AddListener(OnHandleClick);
        
    }

    void OnHandleClick()
    {
        var characterData = new CharacterData
            {
                Name = nameField.text,
                Description = descriptionField.text,
                Attack = int.Parse(attackField.text),
                Defence = int.Parse(defenceField.text)
            };
        DocumentReference charaRef = db.Collection("characters").Document("character");
        charaRef.SetAsync(characterData).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Updated character data");
        });
    }

}
