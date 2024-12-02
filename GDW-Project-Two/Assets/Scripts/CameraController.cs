using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public CinemachineVirtualCamera CineCam;
    public CinemachineFramingTransposer composer;
    protected StateMachine playerMachine;

    const float AIRBORNE_VERTICAL_DEADZONE = 1.5f;

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
        playerMachine = player.getStateMachine();
    }
    private void LateUpdate()
    {
        if (!playerMachine.getCurrentState().IsGrounded())
        {
            composer.m_DeadZoneHeight = AIRBORNE_VERTICAL_DEADZONE;
        }
        else
        {
            composer.m_DeadZoneHeight = 0;
        }
    }
}
