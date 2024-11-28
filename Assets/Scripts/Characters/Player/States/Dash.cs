using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;
using System;

public class Dash : BaseState
{
    [SerializeField] float dash_duration = 0.4f;
    [SerializeField] float dash_speed = 50f;
    [SerializeField] float MaxExitSpeed = 15.0f;
    float dash_dir;


    float timer = 0.0f;

    bool groundDash = false;
 

    // Start is called before the first frame update
 

    public override void onEnter()
    {
        groundDash = IsGrounded();
        dash_dir = Math.Sign(playerInput.actions["Move"].ReadValue<Vector2>().x);
        player.rb.velocity = new Vector2(dash_speed * dash_dir, 0);
    }

    // Update is called once per frame
    public override void FixedUpdateState()
    {
        player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
        if (groundDash)
        {
            if (stateMachine.changeStateIfAvailable("Jump"))
            {
                return;
                //Can only jump cancel dash if its a ground dash
            }
            if (stateMachine.changeStateIfAvailable("Slide"))
            {
                return;
            }
        }
    


    }

    public override void UpdateState()
    {
        base.UpdateState();
        timer += Time.deltaTime;
     
        if (timer >= dash_duration)
        {
         
     
            if (groundDash)
            {
                if (stateMachine.changeStateIfAvailable("Jump"))
                {
                    return;
                    //Can only jump/slide cancel dash if its a ground dash
                }
                if (stateMachine.changeStateIfAvailable("Slide"))
                {
                    return;
                }
            }
            else
            {

              
                if (stateMachine.changeStateIfAvailable("WallJump")) { return; }
                if (stateMachine.changeStateIfAvailable("Fall"))
                {

                    return;
                }
            }
          

            Vector2 exitSpeed;
            exitSpeed.x = Mathf.Clamp(player.rb.velocity.x, -MaxExitSpeed, MaxExitSpeed);
            exitSpeed.y = 0;
            player.rb.velocity = exitSpeed;

            if (stateMachine.changeStateIfAvailable("Run")) {
                
                return;
            }
            if (stateMachine.changeStateIfAvailable("Walk")) {

                return;
            }
            player.rb.velocity = Vector2.zero;
            stateMachine.changeState("Idle");


        }
    }

    public override void onExit()
    {
        base.onExit();
        timer = 0.0f;
    }

    public override bool conditionsMet()
    {
      //  Debug.Log(playerInput.actions.ToString());
      ///  Debug.Log(playerInput.actions["Dash"].ToString());
       // Debug.Log(playerInput.actions["Dash"].IsPressed().ToString());
        return playerInput.actions["Dash"].IsPressed() && playerInput.actions["Move"].ReadValue<Vector2>().x != 0;
    }
}



