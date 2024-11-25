using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


[System.Serializable]
public class Run : PlayerMoveState { 
    public override bool conditionsMet()
    {
        return stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x != 0 && stateMachine.playerInput.actions["Sprint"].IsPressed();
    }
}
