using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Position")]
    public Transform player;
    public float verticalOffset;
    public float followSpeed;

    // [Header("Music")]
    // public int musicToPlay;
    // private bool musicStarted;

    private void Start() 
    {
        player = FindFirstObjectByType<PlayerMovement>().transform;
        followSpeed = 5f;
    }

    private void Update() 
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y + verticalOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);
    }

    // private void LateUpdate() 
    // {
    //     if(!musicStarted)
    //     {
    //         musicStarted = true;
    //         AudioManager.instance.PlayBGM(musicToPlay);
    //     }
    // }

    public void StartBattleCamera(float offset)
    {
        verticalOffset = offset;
    }

    public void EndBattleCamera()
    {
        verticalOffset = 0;
    }
}
