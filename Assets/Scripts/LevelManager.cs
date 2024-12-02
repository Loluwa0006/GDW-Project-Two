using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
//using Cinemachine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class LevelManager : MonoBehaviour
{

    [SerializeField] TMP_Text coinTracker;
    [SerializeField] TMP_Text livesTracker;
    [SerializeField] TMP_Text scoreTracker;
    [SerializeField] PlayerController PlayerPrefab;
   // [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] CameraController cameraController;
    [SerializeField] Camera levelCamera;
    [SerializeField] float PlayerSpawnerSpacerAmount = 1.5f;

    public Transform StartPos;

    public PlayerController playerOne;

    public int NumberOfPlayers = 1;
    
    public int remainingLives = 15;

    public int goodTimeInSeconds = 180;

    List <PlayerController> players = new List<PlayerController> ();


   public UnityEvent<float> scoreChanged;

    float score = 0;
    int TotalCoins = 0;
    Vector3 screenBounds;

    
    // Start is called before the first frame update
    void Start()
    {
        scoreChanged = new UnityEvent<float>();
        scoreChanged.AddListener(updateScore);
        livesTracker.text = "Lives: " + remainingLives.ToString();
        playerOne.transform.position = StartPos.position;
        /* 
         * NumberOfPlayers = (Mathf.Clamp(NumberOfPlayers, 1, 4));
         //Max players is 4
         /*

         foreach (Gamepad pad in Gamepad.all)
         {
             Debug.Log(pad.name);
         }
         for (int i = 0; i < NumberOfPlayers; i++)
         {
             PlayerController newPlayer = Instantiate(PlayerPrefab);
             Vector3 spawnPos = StartPos.position;
             spawnPos.x += (PlayerSpawnerSpacerAmount * i);
             newPlayer.transform.position = spawnPos;
             newPlayer.initPlayer(livesTracker, StartPos, this);
             newPlayer.name = "Player " + (i + 1).ToString();
             players.Add(newPlayer);

         }



         int gamepadIndex = 0;
         /*
             foreach (PlayerController player in players)
             {
                 if (player == players[0])
                 {
                     if (!playerOneUsesKeyboard)
                     {
                         player.getStateMachine().playerInput.SwitchCurrentControlScheme(Gamepad.all[0]);
                         gamepadIndex++;
                     continue;
                     }
                 }
                 if (gamepadIndex <= Gamepad.all.Count - 1)
             {
                 player.getStateMachine().playerInput.SwitchCurrentControlScheme(Gamepad.all[gamepadIndex]);
                 gamepadIndex++;

             }
                 else
             {
                 Debug.Log("No controller connected for player " + player.name);
             }

         }
         */

        /*
        foreach (Gamepad pad in Gamepad.all)
        {
            if (playerOneUsesKeyboard && pad == Gamepad.all[0])
            {
                gamepadIndex++;
                continue;
            }
            players[gamepadIndex].getStateMachine().playerInput.SwitchCurrentControlScheme(pad);
            gamepadIndex++;
        }

        if (playerOneUsesKeyboard && Gamepad.all.Count < players.Count -1)
        {
            Debug.Log("You need " + (players.Count - Gamepad.all.Count).ToString());
        }
        
        cameraController.playerMachine = players[0].getStateMachine();
        cinemachineCamera.m_Follow = players[0].transform;
        //Follow player one always
       */

       // cameraController.playerMachine = player.getStateMachine();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (PlayerController player in players)
        {
         
           if (player == players[0]) { continue; }
            //First player determines position of camera, all other players are restricted by it
            Vector2 newPosition = player.transform.position;
            newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.x, -screenBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, screenBounds.y, -screenBounds.y);
            player.transform.position = newPosition;
        } */ 

    }

    public void AddCoins(int amount = 1)
    {
        TotalCoins += amount;
        if (TotalCoins < 10)
        {
            coinTracker.text = "0" + TotalCoins.ToString();
        }
        else
        {
            coinTracker.text = TotalCoins.ToString();
        }
    }

    public void changeLives(int amount)
    {
        remainingLives += amount;
        livesTracker.text = "Lives: " + remainingLives.ToString();
        if (remainingLives < 0)
        {
            Debug.Log("You lose!");
            Time.timeScale = 0;
        }
    }

    void updateScore(float amount)
    {
        score += amount;
        scoreTracker.text = Math.Round(score).ToString();

    }

}
