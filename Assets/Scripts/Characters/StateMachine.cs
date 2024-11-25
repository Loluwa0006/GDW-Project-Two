using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable] 
public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<BaseState> states = new List<BaseState>();

   [SerializeField] BaseState initalState = null;

    public PlayerInput playerInput;

    public PlayerController player;

    private BaseState currentState;


    [SerializeField] List<BaseState> inactiveProcessing = new List<BaseState>();

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        currentState = initalState;
        foreach (BaseState state in states)
        {
            state.setMachine(this, player);
            if (state.hasInactiveProcess)
            {
                inactiveProcessing.Add(state);
            }
            //init states by giving them references to the machine for switching states and accessing player info
        }
        Debug.Log(playerInput);
        Debug.Log(playerInput.actions);
    }

    // Update is called once per frame
   public void UpdateStateMachine()
    {
     //   Debug.Log("Should be running update func on state " + currentState.name);
        currentState.UpdateState(); 
        //every frame, do the update method for the current state only
    }

    public void FixedUpdateStateMachine()
    {
        currentState.FixedUpdateState();
        foreach (BaseState state in states)
        {
            state.inactiveUpdate();
        }
        Debug.Log(playerInput.actions["Move"].ReadValue<Vector2>());
    }


    public bool changeState(String new_state, Dictionary<string, Variables> msg)
    {
        if (new_state == currentState.name) { return false; }
       BaseState desiredState = hasState(new_state);
        if (desiredState != null)
        {
            currentState.onExit();
            desiredState.onEnter(msg);
            currentState = desiredState;
            return true;
        }
        return false;
    }

    public bool changeState(String new_state)
    {
        if (new_state == currentState.name) { return false; }
        Debug.Log("Changing state from " + currentState.name + " to state " + new_state);
        BaseState desiredState = hasState(new_state);
        if (desiredState != null)
        {
            currentState.onExit();
            desiredState.onEnter();
            currentState = desiredState;
            return true;
        }
        return false;
    }

    public bool changeStateIfAvailable(String new_state, Dictionary<string, Variables> msg )
    {

        BaseState desiredState = hasState(new_state);
        if (desiredState != null)
        {
            if (desiredState.conditionsMet())
            {
                currentState.onExit();
                currentState = desiredState;
                currentState.onEnter(msg);
                return true;
            }
        }
        return false;
    }

    public bool changeStateIfAvailable(String new_state)
    {

        BaseState desiredState = hasState(new_state);
        if (desiredState != null)
        {
            if (desiredState.conditionsMet())
            {
                currentState.onExit();
                currentState = desiredState;
                currentState.onEnter();
                return true;
            }
        }
        return false;
    }

    public bool stateAvailable(String new_state)
    {
        BaseState desiredState = hasState(new_state);
       if (desiredState != null) {
            return desiredState.conditionsMet();
        }
        return false;

    }

    public BaseState hasState(String new_state)
    {
        foreach (BaseState state in states)
        {
            if (state.name == new_state)
            {
                return state;
            }
        }
        Debug.LogError("could not find state " + new_state);
        return null;
    }

    public BaseState getCurrentState()
    {
        return currentState;
    }
}