using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System.Linq;

// This struct holds parameters related to control schemes
public struct ControlSchemeParameters
{
    public InputControlScheme controlScheme;          // The control scheme definition
    public bool isAvailableForNewPlayer;              // Indicates whether this control scheme is available for a new player
    public PlayerInputActions playerInputActions;     // PlayerInputActions associated with this control scheme
    public Material playerVisualMaterial;             // Visual material for the player
    public GameObject playerInstance;                 // Instance of the player GameObject

    // Constructor to initialize the struct
    public ControlSchemeParameters(InputControlScheme controlScheme, bool isAvailableForNewPlayer, PlayerInputActions playerInputActions, Material playerVisualMaterial, GameObject playerInstance)
    {
        this.controlScheme = controlScheme;
        this.isAvailableForNewPlayer = isAvailableForNewPlayer;
        this.playerInputActions = playerInputActions;
        this.playerVisualMaterial = playerVisualMaterial;
        this.playerInstance = playerInstance;
    }
}

// Manager class for handling game controls
public class GameControlsManager : MonoBehaviour
{
    public static GameControlsManager Instance { get; private set; }

    // Events for notifying changes in control availability and playing sounds
    public static event EventHandler OnAvailableControlsChange;
    public static event EventHandler OnControlAddedPlaySound;
    public static event EventHandler OnControlRemovedPlaySound;

    // Reset static data when needed
    public static void ResetStaticData()
    {
        OnAvailableControlsChange = null;
        OnControlAddedPlaySound = null;
        OnControlRemovedPlaySound = null;
    }

    [SerializeField] private int numberOfPlayersMax = 2;

    private static ControlSchemeParameters[] allControlSchemesParameters;  // Array holding all control scheme parameters
    private List<InputDevice> connectedDevices;                             // List of currently connected input devices
    private List<string> availableControlSchemesWithConnectedDevices;      // List of available control schemes based on connected devices
    private List<string> supportedDevicesNotConnected;                     // List of supported devices that are not currently connected
    private int numberOfPlayers;                                           // Number of active players
    private PlayerInputActions defaultPlayerInputActions;                  // Default player input actions

    // Reset data when reloading scenes
    public void ResetDontDestroyOnLoadData()
    {
        Destroy(Instance);
    }

    private void Awake()
    {
        // Ensure only one instance of GameControlsManager exists
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Register event handlers for character parameter selection and deselection
        LobbyUI.Instance.OnCharacterParametersSelected += LobbyUI_OnCharacterParametersSelected;
        LobbyUI.Instance.OnCharacterParametersUnselected += LobbyUI_OnCharacterParametersUnselected;

        numberOfPlayers = 0;
        connectedDevices = InputSystem.devices.ToList();

        defaultPlayerInputActions = new PlayerInputActions();

        // Create control scheme parameters for all default control schemes
        CreateAllControlSchemesParameters();
    }

    private void Start()
    {
        // Register the onDeviceChange event to track changes in connected input devices
        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
    }

    private void InputSystem_onDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        // Handle input device changes
        switch (inputDeviceChange)
        {
            default:
                break;
            case InputDeviceChange.Added:
                // Update the list of connected devices and invoke events
                connectedDevices = InputSystem.devices.ToList();
                OnAvailableControlsChange?.Invoke(this, EventArgs.Empty);
                OnControlAddedPlaySound?.Invoke(this, EventArgs.Empty);
                break;
            case InputDeviceChange.Removed:
                // Update the list of connected devices and invoke events
                connectedDevices = InputSystem.devices.ToList();
                OnAvailableControlsChange?.Invoke(this, EventArgs.Empty);
                OnControlRemovedPlaySound?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    private void LobbyUI_OnCharacterParametersSelected(object sender, LobbyUI.EventArgsOnCharacterParametersSelected e)
    {
        // Handle character parameter selection in the lobby
        int indexSelectedControl = Array.FindIndex(allControlSchemesParameters, scheme => scheme.controlScheme.bindingGroup == e.selectedControlName);
        allControlSchemesParameters[indexSelectedControl].isAvailableForNewPlayer = false;
        allControlSchemesParameters[indexSelectedControl].playerVisualMaterial = e.selectedSkinMaterial;
    }

    private void LobbyUI_OnCharacterParametersUnselected(object sender, LobbyUI.EventArgsOnCharacterParametersUnselected e)
    {
        // Handle character parameter deselection in the lobby
        int indexSelectedControl = Array.FindIndex(allControlSchemesParameters, scheme => scheme.controlScheme.bindingGroup == e.unselectedControlName);
        allControlSchemesParameters[indexSelectedControl].isAvailableForNewPlayer = true;
    }

    private void CreateAllControlSchemesParameters()
    {
        // Create control scheme parameters for all default control schemes
        allControlSchemesParameters = new ControlSchemeParameters[defaultPlayerInputActions.controlSchemes.Count];
        for (int i = 0; i < defaultPlayerInputActions.controlSchemes.Count; i++)
        {
            allControlSchemesParameters[i].controlScheme = defaultPlayerInputActions.controlSchemes[i];
            allControlSchemesParameters[i].isAvailableForNewPlayer = true;
            allControlSchemesParameters[i].playerInputActions = null;
            allControlSchemesParameters[i].playerInstance = null;
        }
    }

    public void GeneratePlayerInputActions()
    {
        // Generate new PlayerInputActions for available control schemes
        for (int i = 0; i < allControlSchemesParameters.Length; i++)
        {
            if (allControlSchemesParameters[i].isAvailableForNewPlayer == false)
            {
                // Create new PlayerInputActions and associate with relevant input device
                PlayerInputActions newPlayerInputActions = new PlayerInputActions();
                InputUser newInputUser = new InputUser();

                foreach (InputDevice connectedDevice in connectedDevices)
                {
                    if (allControlSchemesParameters[i].controlScheme.SupportsDevice(connectedDevice))
                    {
                        newInputUser = InputUser.PerformPairingWithDevice(connectedDevice);
                    }
                }

                if (newInputUser.pairedDevices.Count == 0)
                {
                    Debug.LogError("Trying to generate new PlayerInputActions but there is no relevant supported device connected");
                }

                // Associate actions with the new input user and activate the control scheme
                newInputUser.AssociateActionsWithUser(newPlayerInputActions);
                newInputUser.ActivateControlScheme(allControlSchemesParameters[i].controlScheme.bindingGroup);
                newPlayerInputActions.Enable();

                // Increment the number of players and update control scheme parameters
                numberOfPlayers++;
                allControlSchemesParameters[i].playerInputActions = newPlayerInputActions;

                // Initialize player input actions in the GameInput class
                GameInput.Instance.InitializePlayerInputActions(newPlayerInputActions);
            }
        }
    }

    public List<string> GetAvailableControlSchemesWithConnectedDevices()
    {
        // Get a list of available control schemes based on connected devices
        availableControlSchemesWithConnectedDevices = new List<string>();
        foreach (InputDevice connectedDevice in connectedDevices)
        {
            for (int i = 0; i < allControlSchemesParameters.Length; i++)
            {
                if (allControlSchemesParameters[i].controlScheme.SupportsDevice(connectedDevice) && allControlSchemesParameters[i].isAvailableForNewPlayer)
                {
                    availableControlSchemesWithConnectedDevices.Add(allControlSchemesParameters[i].controlScheme.bindingGroup);
                }
            }
        }
        return availableControlSchemesWithConnectedDevices;
    }

    private string GetDeviceNameFromDeviceRequirement(InputControlScheme.DeviceRequirement deviceRequirement)
    {
        // Extract the device name from the device requirement
        return deviceRequirement.ToString().Substring(deviceRequirement.ToString().IndexOf("<") + 1, deviceRequirement.ToString().IndexOf(">") - 1);
    }

    public List<string> GetSupportedDevicesNotConnected()
    {
        // Get a list of supported devices that are not currently connected
        supportedDevicesNotConnected = new List<string>();

        // Add all supported devices to supportedDevicesNotConnected
        foreach (ControlSchemeParameters schemeParameters in allControlSchemesParameters)
        {
            for (int j = 0; j < schemeParameters.controlScheme.deviceRequirements.Count; j++)
            {
                string deviceName = GetDeviceNameFromDeviceRequirement(schemeParameters.controlScheme.deviceRequirements[j]);
                if (!supportedDevicesNotConnected.Contains(deviceName))
                {
                    supportedDevicesNotConnected.Add(deviceName);
                }
            }
        }

        // Remove the connected devices which belong to supportedDevicesNotConnected
        foreach (InputDevice connectedDevice in connectedDevices)
        {
            if (supportedDevicesNotConnected.Contains(connectedDevice.name))
            {
                supportedDevicesNotConnected.Remove(connectedDevice.name);
            }

            // Unity uses different names for Gamepads in ControlSchemes.DeviceRequirements and InputSystem.devices.
            // The ugly solution I found was to split the Gamepad case apart.
            if (connectedDevice is Gamepad && supportedDevicesNotConnected.Contains("Gamepad"))
            {
                supportedDevicesNotConnected.Remove("Gamepad");
            }
        }
        return supportedDevicesNotConnected;
    }

    public ControlSchemeParameters[] GetAllControlSchemeParameters()
    {
        // Return all control scheme parameters
        return allControlSchemesParameters;
    }

    public bool IsNumberOfPlayersMaxReached()
    {
        // Check if the maximum number of players is reached
        return !(numberOfPlayers < numberOfPlayersMax);
    }

    public int GetMaxNumberOfPlayers()
    {
        // Return the maximum number of players
        return numberOfPlayersMax;
    }
}
