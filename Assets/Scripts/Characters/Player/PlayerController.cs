using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;


[System.Serializable]
public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public Collider2D hurtbox;


    enum PowerType { 
        SMALL,
        NORMAL,
        FIREFLOWER,
        MUSHROOM,
        STAR,
        BUBBLE,
    }

    PowerType currentPower = PowerType.SMALL;

    [SerializeField] StateMachine stateMachine;

    [SerializeField] TMP_Text stateTracker;
    [SerializeField] TMP_Text velocityTracker;
    [SerializeField] TMP_Text livesTracker;

    [SerializeField] Transform SpawnPoint;
    [SerializeField] Transform StartPoint;


    [SerializeField] LayerMask bubbleMask;
    [SerializeField] LayerMask cameraBoundsMask;
    [SerializeField] LevelManager levelManager;


    public Transform groundChecker;

    int coins = 0;

    bool Invlv = false;
    float invlv_timer = 2.5f;
    float invlv_tracker = 0.0f;
    // Start is called before the first frame update

    int healthPoints = 2;



    bool debugEnabled = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); ;
        hurtbox = GetComponent<Collider2D>();
        //fixes bug with InputSystem not reading inputs for some states.
      //  transform.position = StartPoint.transform.position;
      //  SpawnPoint.position = StartPoint.transform.position;
    }

    // Update is called once per frame

    void BubbleLogic()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, rb.velocity.normalized, 1.25f, cameraBoundsMask);
            if (hit)
        {
            rb.velocity = rb.velocity.magnitude * hit.normal;
        }

    }
    void Update()
    { if (currentPower == PowerType.BUBBLE)
        {
            BubbleLogic();
            return;
        }
        if (Invlv)
        {
            invlv_tracker += Time.deltaTime;
            if (invlv_tracker >= invlv_timer)
            {
                invlv_tracker = 0.0f;
                Invlv = false;
            }
        }

        stateMachine.UpdateStateMachine();
        if (debugEnabled)
        {
            stateTracker.text = "State: " + stateMachine.getCurrentState().name;
            velocityTracker.text = "Velocity: " + rb.velocity.ToString();
        }
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdateStateMachine();
    }

    public void addCoin(int amount = 1)
    {
        coins += amount;
        if (coins % 100 == 0)
        {
            levelManager.changeLives(1);
        }
        levelManager.AddCoins(amount);
    }

    public void Damage()
    {
        if (Invlv)
        {
            return;
        }
            Invlv = true;
        healthPoints -= 1;
        currentPower = PowerType.SMALL;
        Debug.Log("owie im getting hit and now only have " +  healthPoints.ToString());
        if (healthPoints <= 0)
        {
            onPlayerDefeated();
        }


        }
    public void onPlayerDefeated()
    {
        Debug.Log("Player " + name + " died ");
        levelManager.changeLives(-1);

       /* if (levelManager.NumberOfPlayers > 1)
            {
                stateMachine.gameObject.SetActive(false);
                currentPower = PowerType.BUBBLE;
            
            gameObject.layer = bubbleMask;
            rb.velocity = new Vector2(0, 1);
            Debug.Log("u are a bubble now.");

            }
          */  
            
                transform.position = SpawnPoint.position;
            Debug.Log("go back to spawn ");

        

        currentPower = PowerType.NORMAL;
        healthPoints = 2;

    }

    public void setCheckpoint(Vector2 pos)
    {
        SpawnPoint.transform.position = pos;
        levelManager.changeLives(1);
    }

  


    public StateMachine getStateMachine()
    {
        return stateMachine;
    }
    
public void initPlayer(TMP_Text l_tracker, Transform spawnpoint, LevelManager manager)
    {
        livesTracker = l_tracker;
        SpawnPoint = spawnpoint;
        StartPoint = spawnpoint;
        levelManager = manager;
       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController bubbleChar = collision.GetComponent<PlayerController>();
        if (bubbleChar == null) { return; }
        else if (bubbleChar.currentPower != PowerType.BUBBLE)
        {
            return;
        }
        bubbleChar.currentPower = PowerType.SMALL;
    }
}



