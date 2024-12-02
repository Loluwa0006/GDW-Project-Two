using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entity " + collision.name + " entered collision box");
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) Destroy(collision.gameObject);
        else player.onPlayerDefeated();
    }
}
