using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class AddNewPlayerScreenUI : MonoBehaviour
{
    // Singleton instance of the class
    public static AddNewPlayerScreenUI Instance { get; private set; }

    // Serialized fields for UI elements
    [SerializeField] private Transform container;
    [SerializeField] private Transform buttonTemplate;
    [SerializeField] private Button resumeButton;
    [SerializeField] private TextMeshProUGUI connectControlText;
    [SerializeField] private Transform maxPlayerReachedScreen;
    [SerializeField] private Button resumeButtonMaxPlayerReached;

    // Event for control option selection
    public event EventHandler<EventArgsOnControlOptionSelection> OnControlOptionSelection;
    public class EventArgsOnControlOptionSelection : EventArgs
    {
        public string controlSchemeSelected;
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Instance = this;
        resumeButton.onClick.AddListener(Hide);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Toggle the pause menu when the screen is loaded
        GameManager_.Instance.TogglePauseMenu();

        // Update the visual elements of the screen
        UpdateVisual();
    }

    // Update the visual elements of the screen
    private void UpdateVisual()
    {
        Debug.Log($"{GameControlsManager.Instance.GetMaxNumberOfPlayers()} ");

        // Check if the maximum number of players is reached
        if (GameControlsManager.Instance.IsNumberOfPlayersMaxReached())
        {
            // Display the max player reached screen
            maxPlayerReachedScreen.gameObject.SetActive(true);
            resumeButtonMaxPlayerReached.onClick.RemoveAllListeners();
            resumeButtonMaxPlayerReached.onClick.AddListener(Hide);
        }
        else
        {
            // Hide the max player reached screen
            maxPlayerReachedScreen.gameObject.SetActive(false);

            // Start a coroutine to update the visual elements
            StartCoroutine(UpdateVisualCoroutine());
        }
    }

    // Coroutine to update visual elements with a delay
    private IEnumerator UpdateVisualCoroutine()
    {
        // Delete existing buttons
        DeleteButtons();

        // Wait for the completion of the Destroy method in DeleteButtons()
        yield return new WaitForEndOfFrame();

        // Get available control schemes with connected devices
        List<string> availableControlSchemes = GameControlsManager.Instance.GetAvailableControlSchemesWithConnectedDevices();

        // Check if control schemes are available
        bool isControlSchemeAvailable = availableControlSchemes.Count != 0;

        if (isControlSchemeAvailable)
        {
            // Calculate the number of new buttons to instantiate
            int numberOfControlSchemesAvailable = availableControlSchemes.Count;
            int numberOfNewButtonsToInstantiate = numberOfControlSchemesAvailable - 1;

            // Instantiate new buttons
            for (int i = 0; i < numberOfNewButtonsToInstantiate; i++)
            {
                Instantiate(buttonTemplate, container);
            }

            // Set the control option text for each button
            int availableControlSchemesIndex = numberOfControlSchemesAvailable - 1;
            foreach (Transform child in container)
            {
                child.GetComponent<ControlOptionSingleButtonUI>().SetControlOptionText(availableControlSchemes[availableControlSchemesIndex]);
                availableControlSchemesIndex--;
            }

            // Add click listeners to the new buttons
            AddListenerToNewButtons();

            // Update the message to the player to connect devices
            UpdateMessageToPlayerToConnectDevices();
        }
        else
        {
            // Display a message when there are no additional control options available
            buttonTemplate.GetComponent<ControlOptionSingleButtonUI>().SetControlOptionText("There is no additional control option available.");

            // Update the message to the player to connect devices
            UpdateMessageToPlayerToConnectDevices();
        }

        // Return null to end the coroutine
        yield return null;
    }

    // Update the message to the player to connect devices
    private void UpdateMessageToPlayerToConnectDevices()
    {
        // Notify player more controls are available
        if (GameControlsManager.Instance.GetSupportedDevicesNotConnected() == null)
        {
            connectControlText.gameObject.SetActive(false);
        }
        else if (GameControlsManager.Instance.GetSupportedDevicesNotConnected().Count == 1)
        {
            connectControlText.gameObject.SetActive(true);
            string deviceName = GameControlsManager.Instance.GetSupportedDevicesNotConnected()[0];
            connectControlText.text = "Connect a " + deviceName + " to enable " + deviceName + " controls.";
        }
        else
        {
            connectControlText.gameObject.SetActive(true);
            string deviceNames = GameControlsManager.Instance.GetSupportedDevicesNotConnected()[0];
            for (int i = 1; i < GameControlsManager.Instance.GetSupportedDevicesNotConnected().Count; i++)
            {
                deviceNames += " or " + GameControlsManager.Instance.GetSupportedDevicesNotConnected()[i];
            }
            connectControlText.text = "Connect a " + deviceNames + " to enable " + deviceNames + " controls.";
        }
    }

    // Add click listeners to the new buttons
    private void AddListenerToNewButtons()
    {
        foreach (Transform child in container)
        {
            child.GetComponent<Button>().onClick.AddListener(() => SelectControlOption(child.GetComponent<ControlOptionSingleButtonUI>().GetControlOption()));
        }
    }

    // Remove click listeners from buttons
    private void RemoveListenerToButtons()
    {
        foreach (Transform child in container)
        {
            child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    // Delete existing buttons
    private void DeleteButtons()
    {
        // Remove click listeners from buttons
        RemoveListenerToButtons();

        // Destroy existing buttons, excluding the template
        foreach (Transform child in container)
        {
            if (child == buttonTemplate)
            {
                continue;
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Select a control option and invoke the event
    private void SelectControlOption(string controlSchemeSelected)
    {
        // Invoke the event for control option selection
        OnControlOptionSelection?.Invoke(this, new EventArgsOnControlOptionSelection
        {
            controlSchemeSelected = controlSchemeSelected
        });

        // Hide the screen
        Hide();
    }

    // Hide the screen
    private void Hide()
    {
        // Toggle the pause menu
        GameManager_.Instance.TogglePauseMenu();

        // Deactivate the screen
        gameObject.SetActive(false);
    }

    // Show the screen and update visual elements
    public void ShowAndUpdateVisual()
    {
        gameObject.SetActive(true);
        GameManager_.Instance.TogglePauseMenu();
        UpdateVisual();
    }
}
