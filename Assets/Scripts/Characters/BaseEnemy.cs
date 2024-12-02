using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{

    protected Vector2 CurrentSpeed = Vector2.zero;
    protected Collider2D boxCollider;
    protected Rigidbody2D rb;
    protected LayerMask groundMask;

    [SerializeField] protected Color RaycastHitColor = Color.red;
    [SerializeField] protected Color RaycastMissColor = Color.gray;

    // Start is called before the first frame update
    private void Awake()
    {
        boxCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");
    }
    public virtual void Damage(int amount)
    {
        Destroy(gameObject);
    }

    public virtual void Damage(int amount, PlayerController player)
    {
        Destroy(gameObject);
    }

    public virtual bool isGrounded(bool checkBack = false)
    {

        Vector2 raycastPos = rb.position;
        if (!checkBack)
        {
            raycastPos.x += ((boxCollider.bounds.size.x / 2.0f) * Mathf.Sign(CurrentSpeed.x));
        }
        else
        {
            raycastPos.x -= ((boxCollider.bounds.size.x / 2.0f) * Mathf.Sign(CurrentSpeed.x));

        }
        RaycastHit2D hit = Physics2D.Raycast(raycastPos, new Vector2(0, -1), 1.0f, groundMask);
        Color rayColor;
        if (hit)
        {
            rayColor = RaycastHitColor;
          //  Debug.Log("You're grounded");
        }
        else
        {
            rayColor = RaycastMissColor;
            //Debug.Log("You're not grounded");
        }
        Debug.DrawRay(transform.position, new Vector2(transform.position.x, transform.position.y - 1), rayColor);
        return hit;
    }

}
