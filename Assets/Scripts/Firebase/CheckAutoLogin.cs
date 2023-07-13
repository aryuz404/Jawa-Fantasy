using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAutoLogin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseManager.Instance.CheckAutoLogin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
