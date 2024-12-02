using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoleSlide : BaseState
{

    [SerializeField] LayerMask PoleLayer;

    [SerializeField] float SlideSpeed = 0.4f;
    [SerializeField] float ScoreIncrease = 0.1f;
    [SerializeField] float SlideDelay = 1.5f;

    float DelayTracker = 0.0f;

    enum PoleSlideState
    {
        STATIONARY,
        SLIDING,
        DONE
    }

    PoleSlideState CurrentState = PoleSlideState.STATIONARY;
    // Start is called before the first frame update
    public override void onEnter()
    {
        base.onEnter();
        CurrentState = PoleSlideState.STATIONARY;
        player.rb.velocity = Vector2.zero;
        DelayTracker = 0.0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        switch (CurrentState)
        {
            case PoleSlideState.STATIONARY:
                DelayTracker += Time.deltaTime;
                if (DelayTracker >= SlideDelay)
                {
                    CurrentState = PoleSlideState.SLIDING;
                }
                break;
            case PoleSlideState.SLIDING:
                player.rb.velocity = new Vector2(0, -SlideSpeed * Time.deltaTime);
                player.levelManager.scoreChanged.Invoke(ScoreIncrease);
                if (IsGrounded())
                {
                    CurrentState = PoleSlideState.DONE;
                }

                break;
            case PoleSlideState.DONE:
                float bonus = player.levelManager.goodTimeInSeconds - Time.realtimeSinceStartup;

                player.levelManager.scoreChanged.Invoke(bonus);
                Debug.Log("You won!");
                player.rb.velocity = Vector2.zero;
                Time.timeScale = 0.0f;
                break;
        }
    }

    public override bool conditionsMet()
    {
        return false;
        //Flagpole object will force player into this state, and there shouldn't be any
        //situation where the player enters it other wise.
    }


}
