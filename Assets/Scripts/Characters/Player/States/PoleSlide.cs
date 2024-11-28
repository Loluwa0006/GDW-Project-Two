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
                player.rb.velocity = new Vector2(0, -SlideSpeed);
                break;
            case PoleSlideState.DONE:
                Debug.Log("You won!");
                player.rb.velocity = Vector2.zero;
                break;
        }
    }

    public override bool conditionsMet()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.rb.position, new Vector2(Mathf.Sign(player.rb.velocity.x), 0), 0.8f, PoleLayer);
        return hit.collider != null;
    }


}
