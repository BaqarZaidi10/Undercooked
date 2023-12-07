using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;

public class GameManager_ : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager_ Instance { get; private set; }

    // UI element for handling device removal
    [SerializeField] private DeviceRemovedUI deviceRemovedUI;

    // Events to notify state changes and other events
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler<EventArgsOnDeviceLost> OnDeviceLost;

    // Custom EventArgs class for device loss event
    public class EventArgsOnDeviceLost : EventArgs
    {
        public string deviceName;
    }

    // Enum representing different game states
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
        CountdownToRestart
    }

    private State state; // Current game state
    private int currentRound = 1;
    private float countdownToStartTimer = 3f; // Timer for the countdown to start
    private float countdownToRestartTimer; // Timer for the countdown to restart
    private float countdownToRestartTimerMax = 3f; // Maximum time for countdown to restart

    [SerializeField] private float gamePlayingTimerMax = 60f; // Maximum time for the game playing state
    private float gamePlayingTimer; // Timer for the game playing state
    private bool isGamePaused = false; // Flag indicating if the game is paused
    private InputUser inputUserChanged; // InputUser affected by device changes

    public bool waitingForScore;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    // Start is called before the first frame update
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        PlayerLoader.OnPlayerInstantiationCompleted += PlayerLoader_OnPlayerInstantiationCompleted;
    }

    // Event handler for the interact action
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Event handler for the pause action
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseMenu();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;

            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;

                if (countdownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    DeliveryManager.Instance.NewRecipe();
                    DeliveryCounter.Instance.ResetCounterSpace();
                    ScoreUI.instance.p1RoundScore = 0;
                    ScoreUI.instance.p2RoundScore = 0;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:

                if (!waitingForScore)
                    gamePlayingTimer -= Time.deltaTime;

                if (gamePlayingTimer < 0 || DeliveryCounter.Instance.counterSpace <= 0)
                {
                    gamePlayingTimer = gamePlayingTimerMax;

                    if (currentRound >= 3)
                    {
                        waitingForScore = true;
                        ScorePopup.Instance.PopupScores();
                        StartCoroutine(GameOverCoroutine());
                    }
                    else
                    {
                        waitingForScore = true;
                        ScorePopup.Instance.PopupScores();
                        StartCoroutine(StartNextRoundCoroutine());
                    }
                }
                break;

            case State.CountdownToRestart:
                countdownToRestartTimer -= Time.deltaTime;

                if (countdownToRestartTimer < 0)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                break;
        }
    }

    private IEnumerator StartNextRoundCoroutine()
    {
        yield return new WaitForSeconds(8);

        waitingForScore = false;
        currentRound += 1;
        countdownToStartTimer = 3;
        state = State.CountdownToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(8);

        waitingForScore = false;
        state = State.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    // Event handler for player instantiation completion
    private void PlayerLoader_OnPlayerInstantiationCompleted(object sender, EventArgs e)
    {
        InputUser.onChange += InputUser_OnChange;
    }

    // Event handler for input user changes
    private void InputUser_OnChange(InputUser inputUser, InputUserChange inputUserChange, InputDevice inputDevice)
    {
        if (inputUserChange == InputUserChange.DeviceLost)
        {
            TogglePauseDeviceRemoved();

            OnDeviceLost?.Invoke(this, new EventArgsOnDeviceLost
            {
                deviceName = inputDevice.name
            });

            inputUserChanged = inputUser;

            deviceRemovedUI.OnRemovePlayer += DeviceRemovedUI_OnRemovePlayer;
        }

        if (inputUserChange == InputUserChange.DeviceRegained && inputUserChanged == inputUser)
        {
            countdownToRestartTimer = countdownToRestartTimerMax;
            TogglePauseDeviceRemoved();
            state = State.CountdownToRestart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);

            deviceRemovedUI.OnRemovePlayer -= DeviceRemovedUI_OnRemovePlayer;
        }
    }

    // Event handler for removing a player due to device removal
    private void DeviceRemovedUI_OnRemovePlayer(object sender, EventArgs e)
    {
        int playerParametersIndex = Array.FindIndex(GameControlsManager.Instance.GetAllControlSchemeParameters(), parameters => parameters.controlScheme == inputUserChanged.controlScheme);
        GameObject playerInstance = GameControlsManager.Instance.GetAllControlSchemeParameters()[playerParametersIndex].playerInstance;
        Destroy(playerInstance);

        countdownToRestartTimer = countdownToRestartTimerMax;
        TogglePauseDeviceRemoved();
        state = State.CountdownToRestart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);

        deviceRemovedUI.OnRemovePlayer -= DeviceRemovedUI_OnRemovePlayer;
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }

    // Check if the game is currently in the playing state
    public bool IsGamePlaying()
    {
        return (state == State.GamePlaying);
    }

    // Check if the game is currently in the countdown to start state
    public bool IsCountdownToStartActive()
    {
        return (state == State.CountdownToStart);
    }

    // Check if the game is currently in the countdown to restart state
    public bool IsCountdownToRestartActive()
    {
        return (state == State.CountdownToRestart);
    }

    // Get the remaining time for the countdown to start
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    // Get the remaining time for the countdown to restart
    public float GetCountdownToRestartTimer()
    {
        return countdownToRestartTimer;
    }

    // Check if the game is currently in the game over state
    public bool IsGameOver()
    {
        return (state == State.GameOver);
    }

    // Get the normalized value of the game playing timer
    public float GetGamePlayingTimerNormalized()
    {
        return (1 - gamePlayingTimer / gamePlayingTimerMax);
    }

    // Toggle the pause menu
    public void TogglePauseMenu()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    // Toggle the pause menu for device removal
    public void TogglePauseDeviceRemoved()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
