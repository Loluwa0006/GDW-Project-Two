using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
//using Cinemachine;

public class CameraController : MonoBehaviour
{
   // public CinemachineVirtualCamera CineCam;
   // public CinemachineFramingTransposer composer;
    public StateMachine playerMachine;

    const float AIRBORNE_VERTICAL_DEADZONE = 3.5f;

    [SerializeField] CinemachineCamera cineCamera;

     ScreenComposerSettings composer;
    private void Awake()
    {
            //CineCam = GetComponent<CinemachineVirtualCamera>();

       /* if (CineCam == null)
        {
            CineCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            Debug.Log(" using game object.Find()");
        }
        if (CineCam == null)
        {
            CineCam = gameObject.GetComponent<CinemachineVirtualCamera>();
            Debug.Log("using game object.getComponent");
        }
        if (CineCam == null)
        {
            CineCam = FindAnyObjectByType<CinemachineVirtualCamera>();
            Debug.Log("using find any object by type");

        }

        if (CineCam == null)
        {
            CineCam = CinemachineObject.GetComponent<CinemachineVirtualCamera>();
            Debug.Log("using cinemachine object.getComponent");
        } 
       */
        
        //composer = CineCam.GetCinemachineComponent<CinemachineFramingTransposer>();
       // lookaheadFactor = composer.m_TrackedObjectOffset.x;

    }
    private void LateUpdate()
    {
        if (playerMachine == null) { return; }
        BaseState currentState = playerMachine.getCurrentState();
        if (!currentState.IsGrounded())
        {
            composer.DeadZone.Size.y = 0.0f;
         //   Debug.Log("enabling camera scroll");
        }
        else
        {
            composer.DeadZone.Size.y = 3.5f;
          //  Debug.Log("nerfing camera scroll");

        }
        /*
        if (currentState.facingRight())
        {
            Debug.Log("Moving camera right");
           composer.m_TrackedObjectOffset.x = Mathf.Lerp(composer.m_TrackedObjectOffset.x, lookaheadFactor, turnSpeed); 
           // composer.m_TrackedObjectOffset.x = Mathf.Abs(composer.m_TrackedObjectOffset.x);
        }
        else if (!currentState.facingRight())
        {
            Debug.Log("Moving camera left");
            composer.m_TrackedObjectOffset.x = Mathf.Lerp(composer.m_TrackedObjectOffset.x, -lookaheadFactor, turnSpeed);
            // composer.m_TrackedObjectOffset.x = Mathf.Abs(composer.m_TrackedObjectOffset.x); * -1;

        } */
    }
}
