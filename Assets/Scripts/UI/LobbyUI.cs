using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Structure to represent the availability of a skin
public struct SkinAvailability
{
    public string skinName;
    public bool isAvailable;

    public SkinAvailability(string skinName, bool isAvailable)
    {
        this.skinName = skinName;
        this.isAvailable = isAvailable;
    }
}

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    // Serialized fields for inspector reference
    [SerializeField] private CharacterSkinsListSO skins;
    [SerializeField] private LobbyCountdownUI lobbyCountdownUI;
    [SerializeField] private CharacterSelectionSingleUI characterSelectionUITemplate;
    [SerializeField] private NewPlayerSingleUI newPlayerUITemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI connectControlText;

    // Events for various UI interactions
    public event EventHandler<EventArgsOnSkinLocked> OnSkinLocked;
    public class EventArgsOnSkinLocked : EventArgs
    {
        public int skinLockedIndex;
        public Transform origin;
    }

    public event EventHandler<EventArgsOnControlOptionLocked> OnControlOptionSelected;
    public class EventArgsOnControlOptionLocked : EventArgs
    {
        public string selectedControlName;
        public Transform origin;
    }

    public event EventHandler<EventArgsOnControlOptionUnlocked> OnControlOptionUnselected;
    public class EventArgsOnControlOptionUnlocked
    {
        public string unselectedControlName;
        public Transform origin;
    }

    public event EventHandler<EventArgsOnCharacterParametersSelected> OnCharacterParametersSelected;
    public class EventArgsOnCharacterParametersSelected : EventArgs
    {
        public string selectedControlName;
        public Material selectedSkinMaterial;
    }

    public event EventHandler<EventArgsOnCharacterParametersUnselected> OnCharacterParametersUnselected;
    public class EventArgsOnCharacterParametersUnselected : EventArgs
    {
        public string unselectedControlName;
    }

    public event EventHandler OnUIChanged;

    private SkinAvailability[] allSkinAvailability;
    private List<CharacterSelectionSingleUI> players;
    private List<NewPlayerSingleUI> newPlayerUIs;
    private int numberOfPlayersReady;
    private float countdownNumber;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Instance = this;

        countdownNumber = 0f;
        numberOfPlayersReady = 0;
        players = new List<CharacterSelectionSingleUI>();
        players.Add(characterSelectionUITemplate);

        newPlayerUIs = new List<NewPlayerSingleUI>();
        newPlayerUIs.Add(newPlayerUITemplate);

        // Initialize skin availability
        allSkinAvailability = new SkinAvailability[skins.characterSkinSOList.Count];
        for (int i = 0; i < skins.characterSkinSOList.Count; i++)
        {
            allSkinAvailability[i].skinName = skins.characterSkinSOList[i].skinName;
            allSkinAvailability[i].isAvailable = true;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Add event listeners
        characterSelectionUITemplate.OnPlayerReady += characterSelectionTemplate_OnPlayerReady;
        characterSelectionUITemplate.OnPlayerNotReady += characterSelectionTemplate_OnPlayerNotReady;
        GameControlsManager.OnAvailableControlsChange += GameControlsManager_OnAvailableControlsChange;

        // Display connection status
        DisplayConnectDevices();

        // Instantiate new player UI elements on load
        InstantiateNewPlayerUIOnLoad();

        // Invoke UI changed event after one frame
        StartCoroutine(InvokeCoroutine());
    }

    // OnDestroy is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Remove event listeners
        GameControlsManager.OnAvailableControlsChange -= GameControlsManager_OnAvailableControlsChange;
    }

    // Event handler for available controls change
    private void GameControlsManager_OnAvailableControlsChange(object sender, EventArgs e)
    {
        DisplayConnectDevices();
    }

    // Instantiate new player UI elements on load
    private void InstantiateNewPlayerUIOnLoad()
    {
        // Number of new player UI elements to instantiate
        int numberOfNewPlayerUIToInstantiate = 2;

        // Instantiate new player UI elements
        while (numberOfNewPlayerUIToInstantiate > 1)
        {
            NewPlayerSingleUI newPlayerUI = Instantiate(newPlayerUITemplate, container);
            newPlayerUIs.Add(newPlayerUI);
            numberOfNewPlayerUIToInstantiate--;
        }

        // Disable the template
        newPlayerUITemplate.gameObject.SetActive(false);

        // Add event handlers to new player UI elements
        foreach (NewPlayerSingleUI newPlayerUI in newPlayerUIs)
        {
            newPlayerUI.OnAddNewPlayer += NewPlayerUI_OnAddNewPlayer;
        }
    }

    // Coroutine to invoke UI changed event after one frame
    private IEnumerator InvokeCoroutine()
    {
        yield return new WaitForEndOfFrame();
        OnUIChanged?.Invoke(this, EventArgs.Empty);
    }

    // Event handler for adding a new player
    private void NewPlayerUI_OnAddNewPlayer(object sender, NewPlayerSingleUI.EventArgsOnAddNewPlayer e)
    {
        // Instantiate a new character selection UI element
        Transform characterSelectionUI = Instantiate(characterSelectionUITemplate.transform, container);

        // Add the new character selection UI element to the list
        players.Add(characterSelectionUI.GetComponent<CharacterSelectionSingleUI>());
        int indexInLayerGroup = players.Count - 1;
        characterSelectionUI.SetSiblingIndex(indexInLayerGroup);

        // Show remove player button and add event handlers
        characterSelectionUI.GetComponent<CharacterSelectionSingleUI>().ShowRemovePlayerButton();
        characterSelectionUI.GetComponent<CharacterSelectionSingleUI>().OnPlayerReady += characterSelectionTemplate_OnPlayerReady;
        characterSelectionUI.GetComponent<CharacterSelectionSingleUI>().OnPlayerNotReady += characterSelectionTemplate_OnPlayerNotReady;
        characterSelectionUI.GetComponent<CharacterSelectionSingleUI>().OnRemovePlayer += characterSelectionTemplate_OnRemovePlayer;

        // Remove event handler from the new player UI element
        e.transform.GetComponent<NewPlayerSingleUI>().OnAddNewPlayer -= NewPlayerUI_OnAddNewPlayer;

        // Disable or destroy the new player UI element based on the template
        if (e.transform == newPlayerUITemplate.transform)
        {
            e.transform.gameObject.SetActive(false);
        }
        else
        {
            Destroy(e.transform.gameObject);
        }

        // Wait 1 frame for the Destroy to act before firing event
        StartCoroutine(InvokeCoroutine());
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if all players are ready
        if (numberOfPlayersReady == players.Count)
        {
            countdownNumber += Time.deltaTime;
            lobbyCountdownUI.Show();
        }
        else
        {
            lobbyCountdownUI.Hide();
            countdownNumber = 0f;
        }

        // Load the game scene after a countdown
        if (countdownNumber >= 2.5f)
        {
            Loader.Load(Loader.Scene.GameScene);
        }
    }

    // Get the availability status of all skins
    public SkinAvailability[] GetAllSkinsAvailability()
    {
        return allSkinAvailability;
    }

    // Get the list of character skins
    public List<CharacterSkinSO> GetSkinsSO()
    {
        return skins.characterSkinSOList;
    }

    // Event handler for player ready
    private void characterSelectionTemplate_OnPlayerReady(object sender, CharacterSelectionSingleUI.EventArgsOnPlayerReady e)
    {
        // Update skin availability
        allSkinAvailability[e.currentSkinDisplayedIndex].isAvailable = false;

        // Invoke events for locked skin, selected control, and selected character parameters
        OnSkinLocked?.Invoke(this, new EventArgsOnSkinLocked
        {
            skinLockedIndex = e.currentSkinDisplayedIndex,
            origin = e.origin
        });

        OnCharacterParametersSelected?.Invoke(this, new EventArgsOnCharacterParametersSelected
        {
            selectedControlName = e.controlOptionSelected,
            selectedSkinMaterial = skins.characterSkinSOList[e.currentSkinDisplayedIndex].Material
        });

        OnControlOptionSelected?.Invoke(this, new EventArgsOnControlOptionLocked
        {
            selectedControlName = e.controlOptionSelected,
            origin = e.origin
        });

        numberOfPlayersReady++;
    }

    // Event handler for player not ready
    private void characterSelectionTemplate_OnPlayerNotReady(object sender, CharacterSelectionSingleUI.EventArgsOnPlayerNotReady e)
    {
        // Update skin availability
        allSkinAvailability[e.currentSkinDisplayedIndex].isAvailable = true;

        // Invoke events for unselected character parameters and unlocked control option
        OnCharacterParametersUnselected?.Invoke(this, new EventArgsOnCharacterParametersUnselected
        {
            unselectedControlName = e.controlOptionSelected,
        });

        OnControlOptionUnselected?.Invoke(this, new EventArgsOnControlOptionUnlocked
        {
            unselectedControlName = e.controlOptionSelected,
            origin = e.origin
        });

        numberOfPlayersReady--;
    }

    // Event handler for removing a player
    private void characterSelectionTemplate_OnRemovePlayer(object sender, EventArgs e)
    {
        // Get the character selection UI element
        CharacterSelectionSingleUI characterSelectionSingleUI = sender as CharacterSelectionSingleUI;

        // Remove the character selection UI element from the list and remove event handlers
        players.Remove(characterSelectionSingleUI.GetComponent<CharacterSelectionSingleUI>());
        characterSelectionSingleUI.GetComponent<CharacterSelectionSingleUI>().OnPlayerReady -= characterSelectionTemplate_OnPlayerReady;
        characterSelectionSingleUI.GetComponent<CharacterSelectionSingleUI>().OnPlayerNotReady -= characterSelectionTemplate_OnPlayerNotReady;
        characterSelectionSingleUI.GetComponent<CharacterSelectionSingleUI>().OnRemovePlayer -= characterSelectionTemplate_OnRemovePlayer;

        // Destroy the character selection UI element
        Destroy(characterSelectionSingleUI.gameObject);

        // Instantiate a new player UI element
        Transform newPlayerUI = Instantiate(newPlayerUITemplate.transform, container);
        newPlayerUI.SetAsLastSibling();
        newPlayerUI.gameObject.SetActive(true);
        newPlayerUIs.Add(newPlayerUI.GetComponent<NewPlayerSingleUI>());
        newPlayerUI.GetComponent<NewPlayerSingleUI>().OnAddNewPlayer += NewPlayerUI_OnAddNewPlayer;

        // Wait 1 frame for the Destroy to act before firing event
        StartCoroutine(InvokeCoroutine());
    }

    // Display information about connecting devices
    private void DisplayConnectDevices()
    {
        // Notify players if more controls are available
        if (GameControlsManager.Instance.GetSupportedDevicesNotConnected().Count == 0)
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

    // Get the lobby countdown time
    public float GetLobbyCountdown()
    {
        return countdownNumber;
    }

    // Get the number of players not ready
    public int GetNumberOfPlayersNotReady()
    {
        return players.Count - numberOfPlayersReady;
    }
}
