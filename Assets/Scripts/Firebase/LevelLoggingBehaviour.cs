using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using UnityEngine.SceneManagement;

public class LevelLoggingBehaviour : MonoBehaviour
{
    private int sceneIndex;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        var activeScene = SceneManager.GetActiveScene();
        sceneIndex = activeScene.buildIndex;
        sceneName = activeScene.name;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
        new Parameter(FirebaseAnalytics.ParameterLevel, sceneIndex),
        new Parameter(FirebaseAnalytics.ParameterLevelName, sceneName));
    }

    private void OnDestroy() 
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
        new Parameter(FirebaseAnalytics.ParameterLevel, sceneIndex),
        new Parameter(FirebaseAnalytics.ParameterLevelName, sceneName));
    }
}
