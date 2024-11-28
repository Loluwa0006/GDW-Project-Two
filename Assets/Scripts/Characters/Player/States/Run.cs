using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


[System.Serializable]
public class Run : PlayerMoveState { 
    public override bool conditionsMet()
    {
        return playerInput.actions["Move"].ReadValue<Vector2>().x != 0 && playerInput.actions["Sprint"].IsPressed();
    }
}
