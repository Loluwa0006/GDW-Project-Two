using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class Walk : PlayerMoveState { 

    public override bool conditionsMet()
    {
     //   Debug.Log("Checking conditions for walk");
        if (stateMachine.playerInput.actions["Move"].ReadValue<Vector2>().x != 0)
        {

        //   Debug.Log("direction pressed");
            if (!stateMachine.playerInput.actions["Sprint"].IsPressed() )
            {
                return true;
            }
            else
            {
           //     Debug.Log("sprint is pressed");
            }
        }
      //  Debug.Log("direction ! pressed");
        return false;
    }
}
