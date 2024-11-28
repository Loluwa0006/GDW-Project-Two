using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Cinemachine;


public class LevelManager : MonoBehaviour
{

    [SerializeField] TMP_Text coinTracker;
    [SerializeField] PlayerController PlayerPrefab;
    [SerializeField] Transform StartPos ;
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] Camera levelCamera;
    [SerializeField] float PlayerSpawnerSpacerAmount = 1.5f;

    public int NumberOfPlayers = 1;


    List <PlayerController> players = new List<PlayerController> ();

    int TotalCoins = 0;
    // Start is called before the first frame update
    void Start()
    {
        NumberOfPlayers = Math.Clamp(NumberOfPlayers, 1, 4);
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            PlayerController newPlayer = Instantiate(PlayerPrefab);
            Vector3 spawnPos = StartPos.position;
            spawnPos.x += (PlayerSpawnerSpacerAmount * i);
            newPlayer.transform.position = spawnPos;
            players.Add(newPlayer);
        }


        cinemachineCamera.m_Follow = players[0].transform;
        //Follow player one always


    }

    // Update is called once per frame
    void Update()
    {

        foreach (PlayerController player in players)
        {
           if (player == players[0]) { continue; }
            //First player determines position of camera, all other players are restricted by it
            Vector2 newPosition = player.transform.position;
            newPosition.x = Mathf.Clamp(newPosition.x, Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.x, Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y, Screen.height);
            player.transform.position = newPosition;
        }
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
}
