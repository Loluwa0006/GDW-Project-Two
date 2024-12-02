//using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaController : BaseEnemy
{
    [SerializeField] KoopaShell shellPrefab;


    [SerializeField] float MoveSpeed = 5.0f;
    [SerializeField] float AccelerationSpeed = 0.75f;
    [SerializeField] float VisionLength = 50.0f;

    [SerializeField] float ChaseAcceleration = 1.0f;
    [SerializeField] float ChaseSpeed = 15.0f;
    [SerializeField] float chaseDuration = 8.0f;

    [SerializeField] LayerMask playerMask;
  

    float chaseTracker = 0.0f;

    bool wasGroundedLastFrame;


    Vector2 target = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        CurrentSpeed = new Vector2(MoveSpeed, 0);


    }

    // Update is called once per frame
    void Update()
    {
        wasGroundedLastFrame = isGrounded();
        CurrentSpeed = rb.velocity;
        CurrentSpeed.y = -0.1f; //see goomba for reasoning
        bool seesPlayer = SeesPlayer();

        if (!isGrounded())
        {

            if (target == Vector2.down)
            {
                CurrentSpeed.x *= -1;
                rb.velocity = CurrentSpeed;
                return; //exit before applying gravity 

            }

        }

        if (target != Vector2.zero)
        {
            if (target.x > rb.position.x)
            {
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





    private bool SeesPlayer()
    {
        Vector2 RaycastPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(Mathf.Sign(CurrentSpeed.x), 0), VisionLength, playerMask);
        Color rayColor;
        if (hit)
        {
            rayColor = RaycastHitColor;
            target = hit.point;
            chaseTracker = 0.0f;
        }
        else
        {
            rayColor = RaycastMissColor;
        }
        Debug.DrawLine(RaycastPos, RaycastPos + (CurrentSpeed.normalized * VisionLength), rayColor);
        return hit;
    }



    public override void Damage(int amount)
    {
        KoopaShell shell = Instantiate(shellPrefab);
        shell.transformsIntoKoopa = true;
        shell.transform.position = transform.position;
        Destroy(gameObject);
    }

}
