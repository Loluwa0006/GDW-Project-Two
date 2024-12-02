using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaScanner : MonoBehaviour
{
    [SerializeField] PiranhaController piranha;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (piranha.currentState != PiranhaController.PiranhaState.IDLE) { return; }

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) { return; }
        piranha.startAttack(collision);
    }
}
