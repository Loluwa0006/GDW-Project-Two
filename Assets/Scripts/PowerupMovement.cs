using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMovement : MonoBehaviour
{
    [SerializeField] float power;
    Rigidbody2D fireball;
    // Start is called before the first frame update
    void Start()
    {
        fireball = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        fireball.AddForce(power * transform.right, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Platforms"))
        {
            fireball.AddForce(power * -transform.right, ForceMode2D.Force);
        }
    }
}
