using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup"; // Animation trigger name
    [SerializeField] private Image backgroundImage; // Reference to the background image
    [SerializeField] private Image iconImage; // Reference to the icon image
    [SerializeField] private TextMeshProUGUI messageText; // Reference to the message text
    [SerializeField] private Color successColor; // Color for success background
    [SerializeField] private Color failColor; // Color for fail background
    [SerializeField] private Sprite successSprite; // Sprite for success icon
    [SerializeField] private Sprite failSprite; // Sprite for fail icon

    public static event Action<GameObject, bool> delivered;

    private Animator animator; // Reference to the animator component

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the animator component on awake
    }

    private void Start()
    {
        // Subscribe to recipe success and failure events
        DeliveryManager.Instance.OnRecipeSuccessed += DeliveryManager_OnRecipeSuccessed;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        // Initially hide the delivery result UI
        gameObject.SetActive(false);
    }

    // Event handler for recipe failure
    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        // Show the delivery result UI with fail details
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP); // Trigger the popup animation
        backgroundImage.color = failColor; // Set background color to fail color
        iconImage.sprite = failSprite; // Set icon sprite to fail sprite
        messageText.text = "DELIVERY\nFAILED"; // Set message text
        delivered?.Invoke(DeliveryCounter.Instance.currentPlayer, false); //Trigger event so that player loses points.
    }

    // Event handler for recipe success
    private void DeliveryManager_OnRecipeSuccessed(object sender, EventArgs e)
    {
        // Show the delivery result UI with success details
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP); // Trigger the popup animation
        backgroundImage.color = successColor; // Set background color to success color
        iconImage.sprite = successSprite; // Set icon sprite to success sprite
        messageText.text = "DELIVERY\nSUCCESS"; // Set message text
        delivered?.Invoke(DeliveryCounter.Instance.currentPlayer, true); //Trigger event so that player gains points.
    }
}
