using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Jump : BaseState
{
    // Start is called before the first frame update
    [SerializeField] float jumpHeight = 5.0f;
    [SerializeField] float jumpTimeToPeak = 0.5f;
    [SerializeField] float bufferWindow = 0.4f;


    [SerializeField] protected float CoyoteDuration = 0.1f;
    protected float CoyoteTracker = 0.0f;

    public float decel_rate = 0.97f;


    public float air_accel = 0.3f;
    public float max_speed = 5.0f;

    protected float jumpVelocity = 0.0f;
    protected float jumpGravity = 0.0f;

    float bufferTracker = 0.0f;




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
        base.FixedUpdateState();
        checkForBuffer();
        if (stateMachine.changeStateIfAvailable("Dash")) { return; }
        if (stateMachine.changeStateIfAvailable("WallJump")) { return;  }

        Vector2 new_velocity =  player.rb.velocity;
        
        if (Math.Abs(new_velocity.x) < max_speed || Math.Sign(moveInput.x) != Math.Sign(new_velocity.x))
        {//If you're not moving as fast as you can, you can add speed. Otherwise, the 2nd part lets you change direction.
            //If you're above max_speed, then we allow the player to maintain their speed, but
            //you cannot accelerate.
            
            new_velocity.x += moveInput.x * air_accel;
            new_velocity.x = Mathf.Clamp(new_velocity.x, -max_speed, max_speed);
        }
        else if (Math.Abs(new_velocity.x) > max_speed)
        {
            new_velocity.x *= decel_rate;
        }

        new_velocity.y += jumpGravity * Time.deltaTime;

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

        if (CoyoteTracker <= CoyoteDuration)
        {
            return bufferTracker > 0 || playerInput.actions["Jump"].IsPressed();
        }
        return false;

    }

    public override void inactiveUpdate()
    {
        base.inactiveUpdate();
        checkForBuffer();
        CoyoteManager();
       
    }

    public void CoyoteManager()
    {

        if (!IsGrounded())
        {
            CoyoteTracker += Time.deltaTime;

        }
        else
        {
            CoyoteTracker = 0.0f;
        }
       // Debug.Log(CoyoteTracker + " is current coyote time");
    }

    public bool checkForBuffer()
    {
        if (playerInput.actions["Jump"].WasPerformedThisFrame())
        {
            bufferTracker = bufferWindow;
            return true;
        }
        else
        {
            bufferTracker -= Time.deltaTime;
        }
        return false;

    }
    public float getMaxSpeed()
    {
        return max_speed;
    }
}

