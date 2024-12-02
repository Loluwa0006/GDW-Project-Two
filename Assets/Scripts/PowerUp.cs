using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] int extraLives = 1;
    [SerializeField] AudioClip collect;
    [SerializeField] AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(audioSource != null && collect != null)
            {
                audioSource.PlayOneShot(collect);
            }
            LevelManager levelManager = FindAnyObjectByType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.changeLives(extraLives);
            }

            Destroy(gameObject);
        }
            
    }
}
