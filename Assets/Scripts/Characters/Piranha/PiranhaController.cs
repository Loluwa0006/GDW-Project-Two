using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiranhaController : MonoBehaviour
{

    [SerializeField] float AttackRange = 25.0f;
    [SerializeField] float AttackSpeed = 4.0f;
    [SerializeField] float RetractSpeed = 1.5f;
    Vector2 startPos;
    Rigidbody2D rb;
    Collider2D hitbox;
    Vector2 AttackVector;
    SpriteRenderer sprite;


   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
      startPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();

    }

   public enum PiranhaState
    {
        IDLE,
        ATTACKING,
        RETRACTING,
    }
   public PiranhaState currentState = PiranhaState.IDLE;

  


    public void startAttack(Collider2D collision)
    {
       
        rb.gameObject.SetActive(true);
        
        AttackVector =  (collision.transform.position - transform.position ).normalized * AttackRange;
        currentState = PiranhaState.ATTACKING;
        hitbox.enabled = true;
        sprite.enabled = true;
    }

    private void disablePlant()
    {
        transform.position = startPos; 
        transform.rotation = Quaternion.identity;
        rb.position = startPos;
        rb.velocity = Vector2.zero;
        sprite.enabled = false;
        hitbox.enabled = false;
        currentState = PiranhaState.IDLE;
    }

    private void FixedUpdate()
    {
        string stateName = "Idle";
        switch (currentState)
        {


            case PiranhaState.ATTACKING:

                if (Vector2.Distance(rb.position, startPos) >= AttackRange)
                {
                rb.velocity = rb.velocity.normalized * AttackRange;
                    currentState = PiranhaState.RETRACTING;
                    hitbox.enabled = false;
                    return;

                }
                stateName = "Attacking";
            rb.velocity += AttackVector.normalized * AttackSpeed * Time.deltaTime;
            rb.velocity = Vector2.ClampMagnitude(AttackVector, AttackSpeed);
                break;


            case PiranhaState.RETRACTING:
                rb.velocity = Vector2.zero;

                rb.position = Vector2.MoveTowards(rb.position, startPos, Time.deltaTime * RetractSpeed);
                if (Vector3.Distance(startPos, rb.position ) <= 2)
                {
                    disablePlant();
                    
                }
                stateName = "Retracting";
                break;

            
        }
        Debug.Log("Current state is " + stateName);
        Debug.Log("Current speed is " + rb.velocity.ToString());

    }
}
