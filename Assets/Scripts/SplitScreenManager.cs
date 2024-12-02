//using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplitScreenManager : MonoBehaviour
{

    public PlayerInputManager playerInputManager;

    LevelManager levelManager;
    int numberOfPlayers = 1;


    // Start is called before the first frame update


    // Update is called once per frame

    private void Awake()
    {
        levelManager = GetComponent<LevelManager>();
    }
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    public void AddPlayer(PlayerInput player)
    {

        Debug.Log("PLEASE UNITY I BEG OF YOU SPARE MY FIRST BORNE");
        Transform playerParent = player.transform.parent.transform.parent;
        Transform playerController = player.transform.parent;
        //StateMachine has playerinput, so you need to get the parent player, then get player's parent, splitscreenplayer object, and then you can get the cinemachine
        Debug.Log("parent name is " + playerParent.name);
        
        playerController.position = levelManager.StartPos.position;

        numberOfPlayers++;



        // playerParent.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = newLayer;
        CinemachineCamera cam = playerParent.GetComponentInChildren<CinemachineCamera>();
        /*
        if (cam!= null)
        {
            Debug.Log("Hurray a cinemachine cam was found");
            cam.OutputChannel = (OutputChannels) numberOfPlayers + 1;
            CinemachineBrain brain = playerParent.GetComponentInChildren<CinemachineBrain>();
            brain.ChannelMask = cam.OutputChannel;
            
        } */

        //playerParent.GetComponentInChildren<CinemachineCamera>().OutputChannel = (OutputChannels) numberOfPlayers;

}
}
