using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : BaseState
{
    // Start is called before the first frame update
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] float jumpTimeToPeak = 0.5f;
    public float decel_rate = 0.97f;


    public float air_accel = 0.3f;
    public float max_speed = 5.0f;

    protected float jumpVelocity = 0.0f;
    protected float jumpGravity = 0.0f;



    private void Awake()
    {
        jumpVelocity = (2.0f * jumpHeight) / jumpTimeToPeak;
        jumpGravity = (-2.0f * jumpHeight) / (jumpTimeToPeak * jumpTimeToPeak);
    }

   

    public override void onEnter()
    {
        base.onEnter();
        player.rb.velocity = new Vector2 (player.rb.velocity.x, jumpVelocity);
    }

    public override void FixedUpdateState()
    {
        if (stateMachine.changeStateIfAvailable("Dash")) { return; }
        if (stateMachine.changeStateIfAvailable("WallJump")) { return;  }

        Vector2 new_velocity =  player.rb.velocity;
        float horiz = stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x;
        if (Math.Abs(new_velocity.x) < max_speed || Math.Sign(horiz) != Math.Sign(new_velocity.x))
        {//If you're not moving as fast as you can, you can add speed. Otherwise, the 2nd part lets you change direction.
            //If you're above max_speed, then we allow the player to maintain their speed, but
            //you cannot accelerate.
            
            new_velocity.x += horiz * air_accel;
            new_velocity.x = Mathf.Clamp(new_velocity.x, -max_speed, max_speed);
        }
        else if (Math.Abs(new_velocity.x) > max_speed)
        {
            new_velocity.x *= decel_rate;
        }

        new_velocity.y += jumpGravity * Time.deltaTime;

       // {
           // Debug.Log("Adding gravity!");
     //   }
        
        player.rb.velocity = new_velocity;
    
        
       if (player.rb.velocity.y < 0)
        {
            stateMachine.changeState("Fall");
            return;
        }  
    }

  


    public float GetJumpHeight()
    {
        return jumpHeight;
    }

    public float GetJumpVelocity()
    {
        return jumpVelocity;
    }
    public override bool conditionsMet()
    {
       
        return IsGrounded() && stateMachine.playerInput.actions["Jump"].IsPressed() ;
    }
    public float getMaxSpeed()
    {
        return max_speed;
    }
}

