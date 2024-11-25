using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slide : BaseState
{


    [SerializeField] float slideDuration = 0.7f;
    [SerializeField] float slideAcceleration = 0.7f;
    [SerializeField] float slideDeceleration = 0.8f;
    [SerializeField] float maxSpeed = 40f;
    [SerializeField] float slideCooldown = 0.4f;
    float cooldownTracker = 0.0f;
    float slideTracker = 0.0f;
    int slideDirection = 0;


   [SerializeField] Fall fallState;

    enum slideState
    {
        ACCELERATING,
        COASTING,
        DECELERATING
    }
    /* way this works:
     player enters state in acceleration
    player continues to gain speed until they reach max speed
    once they do, go to coast state, timer starts
    once timer ends, decelerate
    once sign of speed != slide_direction, or speed = 0, kick player out of state
     
    player doesn't decelerate while in the air though,
    but they're gonna fall until they aren't airborne anymore
     */
    slideState currentState = slideState.ACCELERATING;

    // Start is called before the first frame update
    public override void onEnter()
    {
        if (Math.Abs(player.rb.velocity.x) < maxSpeed)
        {
            currentState = slideState.ACCELERATING;
        }
        else
        {
            currentState = slideState.COASTING;
        }
        slideDirection = Math.Sign(stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x);
    }

    public override void UpdateState()
    {
        if (stateMachine.changeStateIfAvailable("Dash")) { return; }
        if (stateMachine.changeStateIfAvailable("Jump")) { return; }


        base.UpdateState();
        Vector2 newVelocity = player.rb.velocity;
        Vector2 move = stateMachine.playerInput.actions["Move"].ReadValue<Vector2>();
        if (!IsGrounded())
        {
            newVelocity.y -= fallState.getFallSpeed();
            newVelocity.y = Mathf.Clamp(newVelocity.y, -fallState.getMaxFallSpeed(), 0.0f);
        }

        if (move.y >= 0 ) //Not holding down, so exit.
        {
            if (stateMachine.changeStateIfAvailable("Walk")) { return; }
            if (stateMachine.changeStateIfAvailable("Run")) { return; }
            if (stateMachine.changeStateIfAvailable("Dash")) { return; }
            if (stateMachine.changeStateIfAvailable("Jump")) { return; }
            if (stateMachine.changeStateIfAvailable("Fall")) { return; }
            stateMachine.changeState("Idle");
            return;
        }

        switch (currentState)
        {
            case slideState.ACCELERATING:

                if (Math.Abs(newVelocity.x) >= maxSpeed)
                {
                    newVelocity.x = maxSpeed * slideDirection;
                    currentState = slideState.COASTING;
                    break;
                }
                newVelocity.x += slideAcceleration * slideDirection;
                break;
            case slideState.COASTING:
                slideTracker += Time.deltaTime;
                if (slideTracker >= slideDuration)
                {
                    currentState = slideState.DECELERATING;
                }
                break;
            case slideState.DECELERATING:
                if (IsGrounded())
                {
                    newVelocity.x -= slideDeceleration * slideDirection;
                }
                if (slideDirection  == -1 && player.rb.velocity.x >= 0 || slideDirection == 1 && player.rb.velocity.x <=0 )
                {
                    player.rb.velocity = new Vector2(0, player.rb.velocity.y);
                    if (stateMachine.changeStateIfAvailable("Walk")) { return; }
                    if (stateMachine.changeStateIfAvailable("Run")) { return; }
                    if (stateMachine.changeStateIfAvailable("Dash")) { return; }
                    if (stateMachine.changeStateIfAvailable("Jump")) { return; }
                    stateMachine.changeState("Idle");
                    return;
                }
                break;
        }
        player.rb.velocity = newVelocity;
      //  Debug.Log(currentState.ToString());

    }

    public override void inactiveUpdate()
    {
        cooldownTracker -= Time.deltaTime;
    }

    public override void onExit()
    {
        currentState = slideState.ACCELERATING;
        slideTracker = 0.0f;
        cooldownTracker = slideCooldown;
    }

    public override bool conditionsMet()
    {
        bool holdingDown = stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().y < 0 || stateMachine.playerInput.actions["Crouch"].IsPressed();
        return holdingDown && IsGrounded() && Math.Abs(player.rb.velocity.x) >= fallState.jumpState.getMaxSpeed() && cooldownTracker <= 0f  && moveInput.x != 0;
        //kinda ugly. needs fixing
    }
}
