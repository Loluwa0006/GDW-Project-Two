using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class higherJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.activateBoost();
                Destroy(gameObject);
            }
            {
                
            }
        }
    }

}
