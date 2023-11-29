using TMPro;
using UnityEngine;

public class LobbyCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopUp"; // Animation trigger parameter

    [SerializeField] private TextMeshProUGUI countdownText; // Reference to the TextMeshProUGUI component

    private Animator animator; // Reference to the Animator component
    private int previousCountdownNumber; // Previous countdown number to detect changes
    private float countdownNumber; // Current countdown number

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        Hide(); // Initially hide the countdown UI
    }

    private void Update()
    {
        countdownNumber = LobbyUI.Instance.GetLobbyCountdown(); // Get the current lobby countdown from LobbyUI
        int intCountdownNumber = Mathf.CeilToInt(countdownNumber); // Convert the countdown number to an integer (ceiling value)
        countdownText.text = intCountdownNumber.ToString(); // Update the text to display the countdown

        // Check if the countdown number has changed
        if (previousCountdownNumber != intCountdownNumber)
        {
            previousCountdownNumber = intCountdownNumber; // Update the previous countdown number
            animator.SetTrigger(NUMBER_POPUP); // Trigger the "NumberPopUp" animation
            SoundManager.Instance.PlayCountdownSound(); // Play the countdown sound effect
        }
    }

    // Method to show the countdown UI
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // Method to hide the countdown UI
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
