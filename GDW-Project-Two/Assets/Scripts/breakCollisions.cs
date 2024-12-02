using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCollision : MonoBehaviour
{
    public GameObject PowerUp;
    Rigidbody2D playerBody;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        PowerUp.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Blocks")
        {
            Debug.Log("Player collieded with blocks");

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Powerup")
        {
            Debug.Log("Player collieded with powerup block");

            Destroy(collision.gameObject);

            PowerUp.gameObject.SetActive(true);

        }
    }
}
