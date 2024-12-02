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
    int facing = 1;




    // Start is called before the first frame update
    [SerializeField] AudioSource enterAudio;
    [SerializeField] AudioSource exitAudio;
    [SerializeField] AudioSource updateAudio;
    [SerializeField] AudioSource fixedUpdateAudio;


    virtual public void onEnter()
    {
        if (enterAudio != null)
        {
            enterAudio.Play();
        }
    }
    virtual public void onEnter(Dictionary<string, object> msg) 
    {
        if (enterAudio != null)
        {
            enterAudio.Play();
        }
    }

    // Update is called once per frame
    virtual public void UpdateState()
    {
        if (updateAudio != null)
        {
            updateAudio.Play();
        }
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        if (moveInput.x > 0)
        {
            player.transform.localScale = new Vector3(1,1, 1);
            facing = 1;
        }
        else if (moveInput.x < 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
            facing = -1;
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
        if (exitAudio != null)
        {
            exitAudio.Play();
        }
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
        player.hurtbox.enabled = false;
        //Disable collision on self during duration of raycast to make sure ray doesn't collide with ourself
        RaycastHit2D Ray = Physics2D.Raycast(player.transform.position, Vector2.down, 1.25f, LayerMask.GetMask("Ground"));
        if (Ray.collider != null )
        {
            player.hurtbox.enabled = true;
            return true;
        }


        player.hurtbox.enabled = true;
        return false;
    }

    public int getFacing()
    {
        return (facing < 0) ? -1 : 1;
    }
}