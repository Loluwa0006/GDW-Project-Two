using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera CineCam;
    public CinemachineFramingTransposer composer;
    public StateMachine playerMachine;

    const float AIRBORNE_VERTICAL_DEADZONE = 1.5f;

    [SerializeField] float turnSpeed = 3.0f;

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
        
        composer = CineCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    private void LateUpdate()
    {
        if (playerMachine == null) { return; }
        BaseState currentState = playerMachine.getCurrentState();
        if (!currentState.IsGrounded())
        {
            composer.m_DeadZoneHeight = AIRBORNE_VERTICAL_DEADZONE;
        }
        else
        {
            composer.m_DeadZoneHeight = 0;
        }
        if (currentState.facingRight())
        {
            Debug.Log("Moving camera right");
            Mathf.Lerp(composer.m_TrackedObjectOffset.x, Mathf.Abs(composer.m_TrackedObjectOffset.x), turnSpeed); 
           // composer.m_TrackedObjectOffset.x = Mathf.Abs(composer.m_TrackedObjectOffset.x);
        }
        else if (!currentState.facingRight())
        {
            Debug.Log("Moving camera left");
            Mathf.Lerp(composer.m_TrackedObjectOffset.x, Mathf.Abs(composer.m_TrackedObjectOffset.x) * -1, turnSpeed);
        }
    }
}
