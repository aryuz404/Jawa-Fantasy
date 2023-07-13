using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class GetCharacterData : MonoBehaviour
{
    //[SerializeField] private string characterPath = "character_sheets/cool_dude";

    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text attackText;
    [SerializeField] private Text defenceText;

    FirebaseFirestore db;

    private void Start() {
        db = FirebaseFirestore.DefaultInstance;
        //GetData();
    }

    private void Update() 
    {
        GetData();
    }

    public void GetData()
    {
        db.Collection("characters").Document("character").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            var characterData = task.Result.ConvertTo<CharacterData>();

            nameText.text = $"Name: {characterData.Name}";
            descriptionText.text = $"Description: {characterData.Description}";
            attackText.text = $"Attack: {characterData.Attack}";
            defenceText.text = $"Defence: {characterData.Defence}";
        });
    }
}
