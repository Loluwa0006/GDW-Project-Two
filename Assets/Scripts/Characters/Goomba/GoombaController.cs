using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : MonoBehaviour
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
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask groundMask;

    [SerializeField] Color RaycastHitColor;
    [SerializeField] Color RaycastMissColor;

    float jumpVelocity = 0.0f;
    float jumpGravity = 0.0f;
    float fallGravity = 0.0f;

    Vector2 CurrentSpeed = Vector2.zero;
    Vector2 target = Vector2.zero;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        CurrentSpeed = new Vector2(MoveSpeed, 0);
        jumpGravity = (-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak);
        fallGravity = (2.0f * JumpHeight) / (jumpTimeToDecent * jumpTimeToDecent);

    }

    // Update is called once per frame
    void Update()
    {
        bool seesPlayer = SeesPlayer();
       if (!isGrounded())
        {
            if (seesPlayer)
            {

                GoombaJump();
            }
            else
            {
                CurrentSpeed.x *= -1;
            }
        }

       if (seesPlayer)
        {
            CurrentSpeed.x += ChaseAcceleration * Mathf.Sign(target.x);
            CurrentSpeed.x = Mathf.Clamp(CurrentSpeed.x, -ChaseSpeed, ChaseSpeed);
            Debug.Log("Chasing player rn");
        }
       else
        {
            CurrentSpeed.x += AccelerationSpeed * Mathf.Sign(CurrentSpeed.x);
            CurrentSpeed.x = Mathf.Clamp(CurrentSpeed.x, -MoveSpeed, MoveSpeed);
            Debug.Log("Big chillin");
        }
        CurrentSpeed.y += getGravity();
        CurrentSpeed.y = Mathf.Clamp(CurrentSpeed.y, -maxFallSpeed, jumpVelocity);


        rb.velocity = CurrentSpeed;

    }

    public void GoombaJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }


    private bool SeesPlayer()
    {
        Vector2 RaycastPos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(RaycastPos, rb.velocity.normalized, VisionLength, playerMask);
        Color rayColor;
        if (hit)
        {
            rayColor = RaycastHitColor;
            target = hit.point;
        }
        else
        {
            rayColor = RaycastMissColor;
        }
        Debug.DrawLine(RaycastPos, RaycastPos + (rb.velocity.normalized * VisionLength), rayColor);
        return hit;
    }

    float getGravity()
    {
        if (rb.velocity.y < 0)
        {
            return fallGravity;
        }
        return jumpGravity;
    }
    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 1.0f, groundMask);
        Color rayColor;
        if (hit)
        {
            rayColor = RaycastHitColor;
        }
        else
        {
            rayColor = RaycastMissColor;
        }
        Debug.DrawRay(transform.position, new Vector2(transform.position.x, transform.position.y - 1), rayColor);
        return hit;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        player.damage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        Destroy(gameObject);
    }
}
