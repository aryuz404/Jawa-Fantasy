using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator myAnim;
    public Joystick joystick;

    private float horizontalMove = 0f;
    private float verticalMove = 0f;


    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 10f;


    public Rigidbody2D rb;
    private BoxCollider2D coll;


    private enum MovementState { idle, walk, jump, fall }

    
    public string areaTransitionName;
    
    public static PlayerMovement instance;

    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //joystick = FindFirstObjectByType<FixedJoystick>();
        FindJoystick();

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

        
    }

    // Update is called once per frame
    void Update()
    {
        // horizontalMove = joystick.Horizontal;
        // rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if(joystick.Horizontal >= .2f)
        {
            horizontalMove = speed;
            //AudioManager.instance.PlaySFX(1);
        }
        else if(joystick.Horizontal <= -.2f)
        {
            horizontalMove = -speed;
            //AudioManager.instance.PlaySFX(1);
        }
        else
        {
            horizontalMove = 0f;
            //AudioManager.instance.StopSFX();
        }

        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

        verticalMove = joystick.Vertical;

        // if(Input.GetButtonDown("Jump") && IsGrounded())
        // {
        //     rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        // }

        if(verticalMove >= .5f && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        AnimationState();
    }

    public void FindJoystick()
    {
        //joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<FixedJoystick>();
        joystick = FindFirstObjectByType<FixedJoystick>();
        Debug.Log("joystick found");
    }

    private void AnimationState()
    {
        MovementState state;

        if(horizontalMove > 0f)
        {
            state = MovementState.walk;
            transform.eulerAngles = new Vector2(0, 0);
        }
        else if(horizontalMove < 0f)
        {
            state = MovementState.walk;
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > 1f && !IsGrounded())
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -1f && !IsGrounded())
        {
            state = MovementState.fall;
        }

        myAnim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;

    }
}
