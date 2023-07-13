using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;
    public GameObject gameMan;
    public GameObject audioMan;
    // public GameObject battleMan;

    // Start is called before the first frame update
    void Start()
    {
        // if(UIFade.instance == null)
        // {
        //     UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        // }

        //if(UIScreen)

        if(PlayerMovement.instance == null)
        {
            PlayerMovement clone = Instantiate(player).GetComponent<PlayerMovement>();
            PlayerMovement.instance = clone;
        }

        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }

        if(AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }

        // if(BattleManager.instance == null)
        // {
        //     BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
