using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


[System.Serializable]
public class PlayerMoveState : BaseState
{

    [SerializeField] protected float Acceleration = 0.5f;
    [SerializeField] protected float MaxSpeed = 2.5f;



    // Update is called once per frame
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        //  float horiz = stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x;

       
        //check if player wants to leave move state
        if (stateMachine.changeStateIfAvailable("Slide")) { return;  }
        if (stateMachine.changeStateIfAvailable("Dash")) { return; }
        if (stateMachine.changeStateIfAvailable("Fall")) { return; }
        if (stateMachine.changeStateIfAvailable("Jump")) { return; }


        //otherwise do move state stuff
        float horiz = stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x;
        Vector2 newVelocity = player.rb.velocity;


        newVelocity.x += Acceleration * horiz * Time.deltaTime;
        newVelocity.x = Mathf.Clamp(newVelocity.x, -MaxSpeed, MaxSpeed);
        player.rb.velocity = newVelocity;
        if (horiz == 0)
        {
            stateMachine.changeState("Idle");
        }
        else if (!stateMachine.playerInput.actions["Sprint"].IsPressed())
        {
            stateMachine.changeState("Walk");
        }
        else
        {
            stateMachine.changeState("Run");
        }

    }

    public float getMaxSpeed()
    {
        return MaxSpeed;
    }
    
}
