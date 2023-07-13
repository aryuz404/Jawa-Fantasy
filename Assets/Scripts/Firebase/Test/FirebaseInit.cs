using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    
    // Start is called before the first frame update
    async void Start()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task=>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            Debug.Log("Firebase active");
        });
    }

}
