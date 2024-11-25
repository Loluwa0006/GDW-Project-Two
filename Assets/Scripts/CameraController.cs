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

    public GameObject CinemachineObject;

    const float AIRBORNE_VERTICAL_DEADZONE = 1.5f;

    private void Awake()
    {
       
            CineCam = GetComponent<CinemachineVirtualCamera>();
        
        
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
