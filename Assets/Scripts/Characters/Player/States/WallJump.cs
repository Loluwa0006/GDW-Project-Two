using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : Jump
{

    [SerializeField] float wallRaycastLength = 1.0f;
    [SerializeField] float JumpBoost = 4.0f;
    [SerializeField] LayerMask wallMask;

    public override void onEnter()
    {
        base.onEnter();
        //Subtract jumpBoost because we want to go in the opposite direction
        //i.e. if wall is to the left, we jump to the right
        player.rb.velocity = new Vector2(player.rb.velocity.x -  JumpBoost * moveInput.x, player.rb.velocity.y);
    }

    public override bool conditionsMet()
    {
     
        RaycastHit2D touchingWall = Physics2D.Raycast(player.rb.position, new Vector2(moveInput.x, 0), wallRaycastLength);
        Debug.DrawLine(player.rb.position, new Vector2(moveInput.x + player.rb.position.x, moveInput.y + player.rb.position.y), Color.red) ; 

        Debug.Log("move dir = " + moveInput.x.ToString());
        if (touchingWall.collider != null)
        {
            if (touchingWall.collider.transform.CompareTag("Wall"))
            {
                Debug.Log("wall collider layer mask = " + (touchingWall.collider.gameObject.layer.ToString()));
                return stateMachine.playerInput.actions["Jump"].IsPressed();
            }
        }
        else
        {
            Debug.Log("NO COLLIDER??? WAH WAH WAH");
        }

        return false;
    }
    }
