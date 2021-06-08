using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState
{
    WAIT, GAMEPLAY, DIE
}

public enum ItemType
{
    COIN, REDCOIN, STAR
}
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    //[HideInInspector]
    public GameState currentState;

    [Header("HUD")]
    public Text scoreTxt;
    public Text distanceTxt;

    private int score;
    private int distance;

    [Header("Player Config")]
    public Transform    player;
    public Transform    collectableTarget;
    public Transform    powerUpSensor;

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

    [Header("PowerUps")]
    public bool isPowerUp;
    public float percPowerUp;
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

        powerUpSensor.gameObject.SetActive(false);

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
        powerUpSensor.position = player.position;

        movement = new Vector3(0, rb.velocity.y, movementSpeed);

        rb.velocity = movement;

        distance = Mathf.RoundToInt(Vector3.Distance(player.position, stagePosition.position));

        UpdateHud();
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
        AudioManager._instance.PlayFX(AudioManager._instance.fxJump);
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
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("distance", distance);

        AudioManager._instance.PlayMusic(AudioManager._instance.LostLife, false);

        if(score > PlayerPrefs.GetInt("Score"))
        {
            PlayerPrefs.SetInt("Score", score);
        }

        if(distance > PlayerPrefs.GetInt("Distance"))
        {
            PlayerPrefs.SetInt("Distance", distance);
        }

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        currentState = GameState.DIE;
        anim.SetTrigger("Die");
        StartCoroutine("Died");
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(3);
        FadeInOut._instance.Fade();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        FadeInOut._instance.GoScene("GameOver");
    }

    public void GetItem(ItemType type)
    {

        switch(type)
        {
            case ItemType.COIN:

                score += 100;
                if (AudioManager._instance != null)
                {
                    AudioManager._instance.PlayFX(AudioManager._instance.fxCoin);
                }
                break;

            case ItemType.REDCOIN:

                score += 500;
                if (AudioManager._instance != null)
                {
                    AudioManager._instance.PlayFX(AudioManager._instance.fxCoin);
                }
                break;

            case ItemType.STAR:
                if (AudioManager._instance != null)
                {
                    AudioManager._instance.PlayFX(AudioManager._instance.fxCoin);
                    AudioManager._instance.PlayMusic(AudioManager._instance.PowerUp, false);
                }
                isPowerUp = true;
                StartCoroutine("SuperStar");
                score += 1500;

                break;
        }
    }

    void UpdateHud()
    {
        scoreTxt.text = score.ToString("N0");
        distanceTxt.text = distance.ToString("N0") + " m";
    }

    IEnumerator SuperStar()
    {
        powerUpSensor.gameObject.SetActive(true);
        yield return new WaitUntil(() => !AudioManager._instance.Music.isPlaying);
        isPowerUp = false;
        powerUpSensor.gameObject.SetActive(false);
        AudioManager._instance.PlayMusic(AudioManager._instance.LoopStage, true);
    }
}
