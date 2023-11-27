using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{

    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Subscribe to the game state change event
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        // Hide the countdown UI initially
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        // Check if the countdown to start is active and show or hide the UI accordingly
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        // Get the countdown number and update the UI text
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        // Check if the countdown number has changed, trigger animation, and play sound
        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void Show()
    {
        // Set the GameObject to active to show the UI
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // Set the GameObject to inactive to hide the UI
        gameObject.SetActive(false);
    }
}
