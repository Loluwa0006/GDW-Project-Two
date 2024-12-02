using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class Fall : BaseState
{
    // Start is called before the first frame update

    [SerializeField] float jumpTimeToDecent = 0.4f;
    [SerializeField] float maxFallSpeed = 150f;


    public Jump jumpState;
    float fallGravity;

    private void Awake()
    {
        fallGravity = (2.0f * jumpState.GetJumpHeight() / (jumpTimeToDecent * jumpTimeToDecent));
        // Debug.Log(fallGravity);

    }





    public override void FixedUpdateState()
    {

       if (stateMachine.changeStateIfAvailable("WallJump")) { return;  }
        Vector2 new_velocity = player.rb.velocity;
        float horiz = stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x;

        if (Math.Abs(new_velocity.x) < jumpState.max_speed || Math.Sign(horiz) != Math.Sign(new_velocity.x))
        {
            new_velocity.x += horiz * jumpState.air_accel;
            new_velocity.x = Mathf.Clamp(new_velocity.x, -jumpState.max_speed, jumpState.max_speed);
        }
        else if (Math.Abs(new_velocity.x) > jumpState.max_speed)
        {
            new_velocity.x *= jumpState.decel_rate;
        }

        new_velocity.y -= fallGravity * Time.deltaTime;
        if (new_velocity.y < -maxFallSpeed) { new_velocity.y = -maxFallSpeed; } 

        player.rb.velocity = new_velocity;
      
        if (IsGrounded())
        {
            if (stateMachine.changeStateIfAvailable("Dash")) { return; }
            if (stateMachine.changeStateIfAvailable("Slide")) { return; }

            if (stateMachine.changeStateIfAvailable("Idle")) { return;  }
            if (stateMachine.changeStateIfAvailable("Run")) { return; }
            if (stateMachine.changeStateIfAvailable("Walk")) { return; }
          /* Since we need to leave the state, go to idle no matter what */  stateMachine.changeState("Idle");
            return;
        }
    }

    public override bool conditionsMet()
    {
        return !IsGrounded();
    }

    public float getFallSpeed()
    {
        return fallGravity;
    }

    public float getMaxFallSpeed()
    {
        return maxFallSpeed;
    }


}
