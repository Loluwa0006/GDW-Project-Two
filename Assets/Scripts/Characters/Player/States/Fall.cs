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
    [SerializeField] protected float CoyoteDuration = 0.1f;

    [SerializeField] LayerMask stompMask;

   [SerializeField] protected float stompRayLength = 0.75f;
    [SerializeField] protected int stompScore = 100;
    protected float CoyoteTracker = 0.0f;

    public Jump jumpState;
    float fallGravity;

    private void Awake()
    {
        fallGravity = (2.0f * jumpState.GetJumpHeight() / (jumpTimeToDecent * jumpTimeToDecent));
        // Debug.Log(fallGravity);

    }





    public override void FixedUpdateState()
    {

        stompEnemies();
        stompTeammates(); //epic
       if (stateMachine.changeStateIfAvailable("WallJump")) { return;  }
        Vector2 new_velocity = player.rb.velocity;
        float horiz = playerInput.actions["Move"].ReadValue<Vector2>().x;

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

    public override void inactiveUpdate()
    {
        base.inactiveUpdate();
        
            if (!IsGrounded())
            {
                CoyoteTracker += Time.deltaTime;

            }
            else
            {
                CoyoteTracker = 0.0f;
            }
        //Debug.Log(CoyoteTracker + " is current coyote time");
    }

    public override bool conditionsMet()
    {
        return CoyoteTracker >= CoyoteDuration;
    }

    public float getFallSpeed()
    {
        return fallGravity;
    }

    public float getMaxFallSpeed()
    {
        return maxFallSpeed;
    }

    private void stompEnemies()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.rb.position, new Vector2(0, -1), stompRayLength, stompMask);
        if (hit)
        {
            BaseEnemy enemy = hit.transform.GetComponent<BaseEnemy>();
            if (enemy)
            {
               
                enemy.Damage(1);
            }
            else
            {
                BaseItem item = hit.transform.GetComponent<BaseItem>();
                if (item)
                {
                    item.Damage(1);
                }
            }
            Dictionary<string, object> jumpArgs = new Dictionary<string, object>();
            jumpArgs["Bounce"] = true;
            stateMachine.changeState("Jump", jumpArgs);
            player.levelManager.scoreChanged.Invoke(stompScore);
        }
    }

    private void stompTeammates()
    {
        player.hurtbox.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(player.rb.position, new Vector2(0, -1), stompRayLength, LayerMask.GetMask("Player"));
        if (hit)
        {
   
            
            Dictionary<string, object> jumpArgs = new Dictionary<string, object>();
            jumpArgs["Bounce"] = true;
            stateMachine.changeState("Jump", jumpArgs);
            PlayerController stompedPlayer = hit.collider.transform.GetComponent<PlayerController>();
               Vector2 newSpeed = stompedPlayer.rb.velocity;
            if (newSpeed.y > 0)
            {
                newSpeed.y = 0;
            }
            
        }
        player.hurtbox.enabled = true;
    }


}
