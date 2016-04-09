﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    #region Singleton structure
    public static GameController instance = null;

    void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public Player1 player1;
    public Player2 player2;
    public bool player1Alive = true;
    public bool player2Alive = true;
    public float roundTime;
    public float roundTimer = 0f;
    public bool gameEnd = false;

    void Awake()
    {
        InitializeSingleton();
    }

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player1>();
        player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player2>();
        Reset();
    }

    void Update()
    {
        if (!gameEnd)
        {
            TimerCountdown();
            CheckEndConditions();
        }
    }

    void Reset()
    {
        roundTimer = roundTime;
    }

    void TimerCountdown()
    {
        roundTimer -= Time.deltaTime;
    }

    void CheckEndConditions()
    {
        if (player1.active && player2.active)
        {
            player1.Life -= player1.maxLife;
        }
        if (player1.active && !player2.active)
        {
            player1.Life += player1.healthGain * Time.deltaTime;
        }
        if (!player1.active)
        {
            player1.Life -= player1.healthLoss * Time.deltaTime;
        }
        if (roundTimer <= 0f)
        {
            player2.Life -= player2.maxLife;
        }
        if (player1.Life <= 0)
        {
            player1.Life = 0;
            player1Alive = false;
        }
        if (player2.Life <= 0)
        {
            player2.Life = 0;
            player2Alive = false;
        }
        if (!player1Alive)
        {
            StartCoroutine(EndGame());
        }
        if (!player2Alive)
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        if(player1Alive)
        {
            print("Player 1 wins");
        }
        else
        {
            print("Player 2 wins");
        }

        gameEnd = true;
        yield return new WaitForSeconds(1.5f);
        gameEnd = false;
        StartNewGame();
    }

    public void StartNewGame()
    {
        Reset();
        player1.Reset();
        player2.Reset();
    }
}
