using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : PlayerMoveState {

    [SerializeField] protected float decel_rate = 0.97f;


    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        if (stateMachine.changeStateIfAvailable("Jump")) { return; }
        player.rb.velocity = new Vector2(player.rb.velocity.x * decel_rate, 0);

    }
    public override bool conditionsMet()
    {
   
        return stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x == 0 && IsGrounded();
    }
}

