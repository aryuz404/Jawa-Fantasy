using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public string areaTransitionName;
    public TextMeshProUGUI levelChosen;

    public static LevelSelect instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel(string levelName)
    {
        areaTransitionName = levelName;
        levelChosen.text = "Kamu memilih : " + levelName;

        FirebaseAnalytics.LogEvent("open", new Parameter("type", "level " + levelName));
    }
}
