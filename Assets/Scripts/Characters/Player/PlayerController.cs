using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;


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

    [SerializeField] int RemainingLives = 3;

    public Transform groundChecker;

    int coins = 0;

    bool Invlv = false;
    float invlv_timer = 2.5f;
    float invlv_tracker = 0.0f;
    // Start is called before the first frame update

    int healthPoints = 2;

    LevelManager levelManager;

    bool debugEnabled = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); ;
        InputSystem.settings.defaultDeadzoneMax = 0.924f;
        //fixes bug with InputSystem not reading inputs for some states.
        transform.position = StartPoint.transform.position;
        SpawnPoint.position = StartPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
        livesTracker.text = "Lives: " + RemainingLives.ToString();
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
            addLives();
        }
        levelManager.AddCoins(amount);
    }

    public void damage()
    {
        if (Invlv)
        {
            return;
        }
            Invlv = true;
        healthPoints -= 1;
        currentPower = PowerType.SMALL;
        if (healthPoints <= 0)
        {
            onPlayerDefeated();
        }


        }
    public void onPlayerDefeated()
    {
        Debug.Log("You died!");
        if (RemainingLives > 0)
        {
            if (levelManager.NumberOfPlayers > 1)
            {
                stateMachine.gameObject.SetActive(false);
                currentPower = PowerType.BUBBLE;

            }
            transform.position = SpawnPoint.position;
        }
        else
        {
            gameObject.SetActive(false);
        }
        if (currentPower.Equals(PowerType.SMALL))
        {

        }
        RemainingLives -= 1;
        currentPower = PowerType.NORMAL;
        livesTracker.text = "Lives: " + RemainingLives.ToString();
        healthPoints = 2;

    }

    public void setCheckpoint(Vector2 pos)
    {
        SpawnPoint.transform.position = pos;
        addLives();
    }

    public void addLives(int amount = 1)
    {
        RemainingLives += amount;
        livesTracker.text = "Lives: " + RemainingLives.ToString();
    }


    public StateMachine getStateMachine()
    {
        return stateMachine;
    }

}



