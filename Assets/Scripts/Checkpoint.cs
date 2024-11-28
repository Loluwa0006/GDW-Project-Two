using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool checkpointReached = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkpointReached) return;
        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        controller.setCheckpoint(transform.position);
        checkpointReached = true;
    }
}
