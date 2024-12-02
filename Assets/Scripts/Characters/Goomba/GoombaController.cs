//using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : BaseEnemy
{

    [SerializeField] float JumpHeight = 5.0f;
    [SerializeField] float JumpTimeToPeak = 0.5f;
    [SerializeField] float jumpTimeToDecent = 0.4f;
    [SerializeField] float maxFallSpeed = 15f;

    [SerializeField] float MoveSpeed = 5.0f;
    [SerializeField] float AccelerationSpeed = 0.75f;
    [SerializeField] float VisionLength = 50.0f;

    [SerializeField] float ChaseAcceleration = 1.0f;
    [SerializeField] float ChaseSpeed = 15.0f;
    [SerializeField] float chaseDuration = 8.0f;

    [SerializeField] LayerMask playerMask;


    float jumpVelocity = 0.0f;
    float jumpGravity = 0.0f;
    float fallGravity = 0.0f;
    float chaseTracker = 0.0f;

    bool wasGroundedLastFrame;


    Vector2 target = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        CurrentSpeed = new Vector2(MoveSpeed, 0);
        jumpGravity = (-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak);
        fallGravity = (2.0f * JumpHeight) / (jumpTimeToDecent * jumpTimeToDecent);
        jumpVelocity = (2.0f * JumpHeight) / JumpTimeToPeak;


    }

    // Update is called once per frame
    void Update()
    {
        wasGroundedLastFrame = isGrounded();
        CurrentSpeed = rb.velocity;
        bool seesPlayer = SeesPlayer();
        CurrentSpeed.y = -0.1f; //this is to make sure the goomba is grounded
       if (!isGrounded())
        {

            if (target != Vector2.zero)
            {
                GoombaJump();
                //chase player by jumping
            }
            else
            {
                CurrentSpeed.x *= -1;
                rb.velocity = CurrentSpeed; 
                return; //exit before applying gravity 

            }
            CurrentSpeed.y += getGravity();
        }

        if (target != Vector2.zero)
        {
            if (target.x > rb.position.x) {
                CurrentSpeed.x += ChaseAcceleration; 
                    }
            else
            {
                CurrentSpeed.x -= ChaseAcceleration;
            }
            CurrentSpeed.x = Mathf.Clamp(CurrentSpeed.x, -ChaseSpeed, ChaseSpeed);
            Debug.Log("Chasing player rn with speed of " + CurrentSpeed.ToString());
            if (!seesPlayer)
            {
                chaseTracker += Time.deltaTime;
                if (chaseTracker >= chaseDuration)
                {
                    target = Vector2.zero;
                }
            }
        }
       else
        {
            CurrentSpeed.x += AccelerationSpeed * Mathf.Sign(CurrentSpeed.x);
            CurrentSpeed.x = Mathf.Clamp(CurrentSpeed.x, -MoveSpeed, MoveSpeed);
        }
      //  CurrentSpeed.y = Mathf.Clamp(CurrentSpeed.y, -maxFallSpeed, jumpVelocity);


        rb.velocity = CurrentSpeed;


    }

   
    public void GoombaJump()
    {
        CurrentSpeed = new Vector2(rb.velocity.x, jumpVelocity);
        Debug.Log("Jumpin");
    }

  

    private bool SeesPlayer()
    {
        Vector2 RaycastPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(Mathf.Sign(CurrentSpeed.x), 0), VisionLength, LayerMask.GetMask("Player", "Wall" ,"Ground" ) );
        Color rayColor;
        if (hit)
        {
            rayColor = RaycastHitColor;
            target = hit.point;
            chaseTracker = 0.0f;
            return (hit.transform.GetComponent<PlayerController>() != null);
        }
        else
        {
            rayColor = RaycastMissColor;
        }
        Debug.DrawLine(RaycastPos, RaycastPos + (CurrentSpeed.normalized * VisionLength), rayColor);
        return false;
    }

    float getGravity()
    {
        if (CurrentSpeed.y <= 0)
        {
            return fallGravity;
        }
        return jumpGravity;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) { return;  }
        player.Damage();
        Debug.Log("tryna hit player");
    }


}
