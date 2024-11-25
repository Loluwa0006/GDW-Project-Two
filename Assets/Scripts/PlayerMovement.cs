using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    

    [SerializeField] float power;
    [SerializeField] float jumpForce = 10;

    private bool isGrounded;
    

    

    
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
        body.velocity = new Vector2(body.velocity.x, jumpForce);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Destroy(collision.gameObject);
        }
    }

}
