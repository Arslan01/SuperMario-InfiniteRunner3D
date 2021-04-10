using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    WAIT, GAMEPLAY, DIE
}
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

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

    [Header("Stage Blocks")]
    private int blocks;
    public int startBlocks;
    public float blockSize;
    public GameObject[] stageBlock;
    public Transform stagePosition;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        blocks = 1;
    }

    void Start()
    {
        rb = player.gameObject.GetComponent<Rigidbody>();
        anim = player.gameObject.GetComponent<Animator>();

        targetPosition = waypoints[idWaypoint].position;

        for(int i = 0; i < startBlocks; i++)
        {
            NewBlock();
        }


        if(FadeInOut._instance != null) 
        {
            FadeInOut._instance.Fade();
            StartCoroutine("StartStage");
        }
        else
        {
            currentState = GameState.GAMEPLAY;
            anim.SetBool("isWalk", true);
        }
        
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

    IEnumerator StartStage()
    {
        yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        currentState = GameState.GAMEPLAY;
        anim.SetBool("isWalk", true);
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
        anim.SetBool("isGrounded", isGrounded);
    }

    public void NewBlock()
    {
        int idBlock = Random.Range(0, stageBlock.Length);
        GameObject temp = Instantiate(stageBlock[idBlock], stagePosition.position + Vector3.forward * (blocks * blockSize), Quaternion.identity, stagePosition);
        blocks++;
    }

    public void Die()
    {
        currentState = GameState.DIE;
        anim.SetTrigger("Die");
        StartCoroutine("Died");
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(3);
        FadeInOut._instance.Fade();
    }
}
