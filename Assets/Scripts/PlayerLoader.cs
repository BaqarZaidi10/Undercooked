using UnityEngine;
using System;

public class PlayerLoader : MonoBehaviour
{
    int playersLoaded = 0;
    // Singleton instance for the PlayerLoader class
    public static PlayerLoader Instance { get; private set; }

    // Event to notify when player instantiation is completed
    public static event EventHandler OnPlayerInstantiationCompleted;

    // ResetStaticData is used to reset static data (e.g., event handlers) when needed
    public static void ResetStaticData()
    {
        OnPlayerInstantiationCompleted = null;
    }

    // Reference to the player prefab
    [SerializeField] private Transform playerPrefab;

    // Array of spawn points for player instantiation
    [SerializeField] private Transform[] spawnPoints;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Set the singleton instance to this script
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Instantiate players when the scene starts
        InstantiatePlayers();
    }

    // Instantiate players based on the defined control schemes and spawn points
    private void InstantiatePlayers()
    {
        // Generate player input actions using the GameControlsManager
        GameControlsManager.Instance.GeneratePlayerInputActions();

        // Get all control scheme parameters from the GameControlsManager
        ControlSchemeParameters[] allControlSchemesParameters = GameControlsManager.Instance.GetAllControlSchemeParameters();

        // Iterate through each control scheme
        for (int i = 0; i < allControlSchemesParameters.Length; i++)
        {
            // Check if the control scheme has associated player input actions
            if (allControlSchemesParameters[i].playerInputActions != null)
            {
                // Instantiate a player using the specified prefab, position, rotation, input actions, and visual material
                GameObject playerInstance = ExtensionMethods.InstantiatePlayer(
                    playerPrefab.gameObject,
                    spawnPoints[i].position,
                    spawnPoints[i].rotation,
                    allControlSchemesParameters[i].playerInputActions,
                    allControlSchemesParameters[i].playerVisualMaterial
                );
                playersLoaded++;
                playerInstance.name = $"p{playersLoaded}";

                // Set the player instance in the control scheme parameters
                allControlSchemesParameters[i].playerInstance = playerInstance;
            }
        }

        // Invoke the event to signal that player instantiation is completed
        OnPlayerInstantiationCompleted?.Invoke(this, EventArgs.Empty);
    }
}
