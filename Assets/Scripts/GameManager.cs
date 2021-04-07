using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    WAIT, GAMEPLAY, DIE
}
public class GameManager : MonoBehaviour
{
    //[HideInInspector]
    public GameState currentState;

    [Header("Player Config")]
    public Transform    player;
    private Rigidbody   rb; 
    private Animator    anim;

    public float movementSpeed;
    public float changeWaySpeed;
    public float jumpForce;

    private bool        isWalk;
    private bool        isGrounded;
    public Transform    groundCheck;
    public LayerMask    whatIsGround;

    private Vector3 movement;

    [Header("Waypoints Config")]
    public int idWaypoint = 1;
    public Transform[] waypoints;
    private Vector3 targetPosition;

    void Start()
    {
        rb = player.gameObject.GetComponent<Rigidbody>();
        anim = player.gameObject.GetComponent<Animator>();

        targetPosition = waypoints[idWaypoint].position;
    }

    void Update()
    {
        UpdateAnimator();

        if(currentState != GameState.GAMEPLAY) { return; } 

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            InputX(1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            InputX(-1);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        targetPosition = new Vector3(waypoints[idWaypoint].position.x, player.position.y, player.position.z);

        player.position = Vector3.MoveTowards(player.position, targetPosition, changeWaySpeed * Time.deltaTime);
        movement = new Vector3(0, rb.velocity.y, movementSpeed);

        rb.velocity = movement;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, whatIsGround);
    }

    public void  Jump()
    {
        if(isGrounded == false) { return; }
        rb.AddForce(Vector3.up * jumpForce);
    }

    public void InputX(int value)
    {
        idWaypoint += value;
        if(idWaypoint >= waypoints.Length)
        {
            idWaypoint = waypoints.Length - 1;
        }
        else if(idWaypoint < 0)
        {
            idWaypoint = 0;
        }
    }

    void UpdateAnimator()
    {
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isGrounded", isGrounded);
    }
}
