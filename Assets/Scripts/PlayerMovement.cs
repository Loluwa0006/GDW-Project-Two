using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    

    [SerializeField] float power;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float highForce = 30;
    [SerializeField] float highForceDuration = 20;

    private bool isGrounded;
    private bool isBoosted;
    

    

    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            characterJump();
            
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            body.AddForce(power * -transform.right, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            body.AddForce(power * transform.right, ForceMode2D.Force);
        }
        
        

    }

    void characterJump()
    {
        float Force = isBoosted ? highForce : jumpForce;
        body.velocity = new Vector2(body.velocity.x, Force);

        isGrounded = false;
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain")|| 
            collision.gameObject.CompareTag("Platforms")|| 
            collision.gameObject.CompareTag("Blocks"))
        {
            isGrounded = true;
        }
    }

    public void activateBoost ()
    {
        isBoosted = true;
        Invoke("deactivateBoost", highForceDuration);
    }

    void deactivateBoost ()
    {
        isBoosted = false;
    }

}
