using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KoopaShell : BaseItem
{
    // Start is called before the first frame update

    [SerializeField] float shellSpeed = 25.0f;
    [SerializeField] float wallRaycastLength = 1.25f;
    [SerializeField] float fallForce = 10.0f;

    [SerializeField] LayerMask bounceMask;
    [SerializeField] KoopaController koopaPrefab;


    [SerializeField] float turtleTimer = 30.0f;
    float turtleTracker = 0.0f;

    PlayerController playerOwner = null;

    public bool transformsIntoKoopa = false;

    [SerializeField] Collider2D hurtbox;
    [SerializeField] Collider2D interactBox;


    Transform originalParent;


    enum ShellState
    {
            MOVING, 
            STATIONARY,
            HELD
    }

    ShellState currentState = ShellState.STATIONARY;
    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = ShellState.STATIONARY;
        hurtbox = GetComponent<Collider2D>();
        originalParent = transform.parent;
  
    }

    private void Update()
    {
        Vector2 newSpeed = rb.velocity;
       turtleTracker += Time.deltaTime;
        if (transformsIntoKoopa)
        {
            if (turtleTracker >= turtleTimer)
            {
                KoopaController koopa = Instantiate(koopaPrefab);
                koopa.transform.position = transform.position;
                Destroy(gameObject);
            }
        }

      
        if (!IsGrounded())
        {
            newSpeed.y = -fallForce;
        }
        else
        {
            newSpeed.y = 0;
            //applying downward force while grounded makes it awkward to move horizontally
        }
        rb.velocity = newSpeed;
    }

    private void LateUpdate()
    {
        Vector2 newSpeed = rb.velocity;
        if (currentState == ShellState.MOVING)
        {
            if (touchingWall())
            {
                newSpeed.x *= -1;
                Debug.Log("boucing");
            }
        }
        rb.velocity = newSpeed;
    }

    public override void onPlayerInteract(PlayerController player)
    {
        interactBox.enabled = false;
        transform.parent = player.transform;
        currentState = ShellState.HELD;
        player.releasedInteract.AddListener(onPlayerReleased);
        Debug.Log("Shell picked up by player " + player.name);
    }

    public bool IsGrounded()
    {
        // Debug.Log("Checking if grounded");
        hurtbox.enabled = false;
        //Disable collision on self during duration of raycast to make sure ray doesn't collide with ourself
        RaycastHit2D Ray = Physics2D.Raycast(rb.position, Vector2.down, 1.25f, LayerMask.GetMask("Ground"));
        if (Ray.collider != null)
        {
            hurtbox.enabled = true;
            return true;
        }


        hurtbox.enabled = true;
        return false;
    }

    public override void onPlayerReleased(PlayerController player)
    {
        currentState = ShellState.MOVING;
        transform.parent = originalParent;
        rb.velocity = new Vector2(shellSpeed * player.getStateMachine().getCurrentState().getFacing(), rb.velocity.y);
    }

    public override void Damage(int amount, PlayerController player)
    {
        if (player.GetCurrentPower() == PlayerController.PowerType.STAR)
        {
            Destroy(gameObject);
            return;
        }
        
        switch (currentState)
        {
            case ShellState.STATIONARY:
                Debug.Log("Stomped on stationary shell");
                Vector2 newSpeed = rb.velocity;
                if (player.transform.position.x < transform.position.x)
                {
                    newSpeed.x = shellSpeed;
                }
                else
                {
                    newSpeed.x = -shellSpeed;
                }
                rb.velocity = newSpeed;
                currentState = ShellState.MOVING;
                break;
            case ShellState.MOVING:
                rb.velocity = Vector2.zero;
                currentState = ShellState.STATIONARY;
                break;
            case ShellState.HELD:
                if (player != playerOwner)
                {
                    Destroy(gameObject);
                    return;
                }
                break;
        }
    }
    public bool touchingWall()
    {
        hurtbox.enabled = false;
        interactBox.enabled = false;
        int facing = (int) (Mathf.Sign(rb.velocity.x));

        RaycastHit2D touchingWall = Physics2D.Raycast(rb.position, new Vector2(facing, 0), wallRaycastLength, bounceMask);
        Debug.DrawLine(rb.position, new Vector2(rb.position.x + facing, rb.position.y), Color.red);


        hurtbox.enabled = true;
        interactBox.enabled = true;
        return touchingWall;
    }


}
