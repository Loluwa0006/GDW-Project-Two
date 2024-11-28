using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseState : MonoBehaviour
{
    protected StateMachine stateMachine;
    protected PlayerController player;
   protected PlayerInput playerInput;
   public bool hasInactiveProcess = false;



    protected Vector2 moveInput = Vector2.zero;


    // Start is called before the first frame update

  

    virtual public void onEnter()
    {

    }
    virtual public void onEnter(Dictionary<string, Variables> msg) 
    {

    }

    // Update is called once per frame
    virtual public void UpdateState()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        if (moveInput.x > 0)
        {
            player.transform.localScale = new Vector3(1,1, 1);
        }
        else if (moveInput.x < 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    virtual public void FixedUpdateState()
    {

    }

    virtual public void inactiveUpdate()
        //This is for states that need to run logic in the background when not focused
        //i.e. cooldown for slide 
        //functions similarly to FixedUpdate
    {

    }

    virtual public void onExit()
    {

    }

    public void setMachine(StateMachine machine, PlayerController player)
    {

        stateMachine = machine;
        this.player = player;
        playerInput = machine.playerInput;

      //  Debug.Log(name + " has been init with machine " + machine.name + " and player " + player.name);
    }

    public virtual bool conditionsMet()
    {
        return true;
    }

    public bool IsGrounded()
    {
       // Debug.Log("Checking if grounded");
        RaycastHit2D Ray = Physics2D.Raycast(player.transform.position, Vector2.down, 1.25f, LayerMask.GetMask("Ground"));
        if (Ray.collider != null)
        {
           // Debug.Log("hit somethin");
            return true;
        }
       //  Debug.Log("hit nothin");
        return false;
    }
}