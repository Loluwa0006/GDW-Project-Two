using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Events;


[System.Serializable]
public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public Collider2D hurtbox;

   public enum PowerType { 
        SMALL,
        NORMAL,
        FIREFLOWER,
        MUSHROOM,
        STAR,
        BUBBLE,
    }

    [SerializeField] PowerType currentPower = PowerType.NORMAL;

    [SerializeField] StateMachine stateMachine;

    [SerializeField] TMP_Text stateTracker;
    [SerializeField] TMP_Text velocityTracker;

    [SerializeField] Transform SpawnPoint;

    [SerializeField] LayerMask bubbleMask;
    [SerializeField] LayerMask cameraBoundsMask;

    [SerializeField] float InteractDistance = 1.25f;
    public LevelManager levelManager;

    public Transform groundChecker;

    int coins = 0;

    bool Invlv = false;
    float invlv_timer = 2.5f;
    float invlv_tracker = 0.0f;
    // Start is called before the first frame update

    bool debugEnabled = false;

    public UnityEvent<PlayerController> releasedInteract;
    public UnityEvent playerDied;
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
    {
        canInteract();
        if (currentPower == PowerType.BUBBLE)
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
        if (stateMachine.playerInput.actions["Interact"].WasReleasedThisFrame()) {
            releasedInteract.Invoke(this);
        }
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
        if (currentPower == PowerType.SMALL)
        {
            onPlayerDefeated();
        }
        currentPower = PowerType.SMALL;



    }

    public void becomeBubble()
    {
          
              stateMachine.gameObject.SetActive(false);
              currentPower = PowerType.BUBBLE;

          gameObject.layer = bubbleMask;
          rb.velocity = new Vector2(0, 1);
          Debug.Log("u are a bubble now.");

          
        
    }
    public void onPlayerDefeated()
    {
        playerDied.Invoke();
        hurtbox.enabled = false;
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

        

        currentPower = PowerType.SMALL;
        hurtbox.enabled = true;
        //turn hurtbox off and on again to ensure player doesn't get hurt while respawning
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

    public PowerType GetCurrentPower()
    {
        return currentPower;
    }
    
public void initPlayer( Transform spawnpoint, LevelManager manager)
    {
        SpawnPoint = spawnpoint;
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

    private void canInteract()
    {
        if (!stateMachine.playerInput.actions["Interact"].IsPressed())
        {
            return;
        }
        int facing = (stateMachine.getCurrentState().getFacing());
        RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(facing, 0), InteractDistance, LayerMask.GetMask("Item"));
        if (hit)
        {
            Debug.Log("Interact is pressed");
            BaseItem item = hit.transform.GetComponent<BaseItem>();
            if (item)
            {
                item.onPlayerInteract(this);
            }
        }
    }
}



