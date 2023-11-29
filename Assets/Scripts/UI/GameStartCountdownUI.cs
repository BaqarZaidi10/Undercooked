using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopUp"; // Animation trigger parameter

    [SerializeField] private TextMeshProUGUI countdownText; // Reference to the TextMeshProUGUI component

    private Animator animator; // Reference to the Animator component
    private int previousCountdownNumber; // Previous countdown number to detect changes

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Start()
    {
        GameManager_.Instance.OnStateChanged += GameManager_OnStateChanged; // Subscribe to the game state change event
        Hide(); // Initially hide the countdown UI
    }

    private void OnDestroy()
    {
        GameManager_.Instance.OnStateChanged -= GameManager_OnStateChanged; // Unsubscribe from the game state change event
    }

    private void Update()
    {
        int countdownNumber;

        // Determine which countdown timer to use based on the game state
        if (GameManager_.Instance.IsCountdownToStartActive())
        {
            countdownNumber = Mathf.CeilToInt(GameManager_.Instance.GetCountdownToStartTimer());
        }
        else
        {
            countdownNumber = Mathf.CeilToInt(GameManager_.Instance.GetCountdownToRestartTimer());
        }

        countdownText.text = countdownNumber.ToString(); // Update the text to display the countdown

        // Check if the countdown number has changed
        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber; // Update the previous countdown number
            animator.SetTrigger(NUMBER_POPUP); // Trigger the "NumberPopUp" animation
            SoundManager.Instance.PlayCountdownSound(); // Play the countdown sound effect
        }
    }

    // Event handler for the game state change event
    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        // Show or hide the countdown UI based on the game state
        if (GameManager_.Instance.IsCountdownToStartActive() || GameManager_.Instance.IsCountdownToRestartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    // Method to show the countdown UI
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Method to hide the countdown UI
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
