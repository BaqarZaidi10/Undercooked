using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{

    // Singleton pattern to ensure only one instance of KitchenGameManager exists
    public static KitchenGameManager Instance { get; private set; }

    // Events to notify subscribers about state changes, game pause, and game unpause
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    // Enumeration representing different states of the game
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    // Current state of the game
    private State state;
    private float countdownToStartTimer = 3f; // Initial countdown timer value
    private float gamePlayingTimer; // Timer for the main gameplay
    private float gamePlayingTimerMax = 120f; // Maximum time for the gameplay
    private bool isGamePaused = false; // Flag to track whether the game is paused

    // Called when the script instance is being loaded
    private void Awake()
    {
        Instance = this; // Set the singleton instance
        state = State.WaitingToStart; // Initialize the game state to WaitingToStart
    }

    // Called before the first frame update
    private void Start()
    {
        // Subscribe to relevant input events
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    // Event handler for player interaction input
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Event handler for player pause input
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                // No action during WaitingToStart state
                break;
            case State.CountdownToStart:
                // Countdown to start the game
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                // Update the main gameplay timer
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                // No action during GameOver state
                break;
        }
    }

    // Check if the game is currently in the GamePlaying state
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    // Check if the game is currently in the CountdownToStart state
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    // Get the remaining time for the countdown to start
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    // Check if the game is currently in the GameOver state
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    // Get the normalized value of the main gameplay timer
    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    // Toggle the game pause state
    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            // Pause the game
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Unpause the game
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
