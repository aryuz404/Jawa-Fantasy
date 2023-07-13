using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueList : ScriptableObject
{
    public string Description;

    [Header("Things will be said:")]
    public List<string> Sentences;
}
