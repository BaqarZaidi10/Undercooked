using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundsEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefSO audioClipRefsSO;

    private float volume = 1f;

    private void Awake()
    {
        // Ensure there is only one instance of SoundManager
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Load sound effects volume from PlayerPrefs
        float defaultVolume = 1f;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, defaultVolume);
    }

    private void Start()
    {
        // Subscribe to events on Start
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeSuccessed += DeliveryManager_OnRecipeSuccessed;
            DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        }

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        BaseCounter.OnAnyObjectObjectPlacedHere += BaseCounter_OnAnyObjectObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        CharacterSelectionSingleUI.OnReadySelection += CharacterSelectionSingleUI_OnReadySelection;
        CharacterSelectionSingleUI.OnReadyUnselection += CharacterSelectionSingleUI_OnReadyUnselection;
        CharacterSelectionSingleUI.OnClick += CharacterSelectionSingleUI_OnClick;
        CharacterSelectionSingleUI.OnPlayerRemoval += CharacterSelectionSingleUI_OnPlayerRemoval;
        NewPlayerSingleUI.OnPlayerAdditon += NewPlayerSingleUI_OnPlayerAdditon;
        GameControlsManager.OnControlAddedPlaySound += GameControlsManager_OnControlAddedPlaySound;
        GameControlsManager.OnControlRemovedPlaySound += GameControlsManager_OnControlRemovedPlaySound;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events on OnDestroy
        GameControlsManager.OnControlAddedPlaySound -= GameControlsManager_OnControlAddedPlaySound;
        GameControlsManager.OnControlRemovedPlaySound -= GameControlsManager_OnControlRemovedPlaySound;
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        // Play a single sound clip at a specified position with a given volume
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        // Play a random sound from an array at a specified position with a given volume
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * volume);
    }

    private void GameControlsManager_OnControlRemovedPlaySound(object sender, EventArgs e)
    {
        // Play sound when a player control is removed
        Camera camera = Camera.main;
        PlaySound(audioClipRefsSO.removePlayer, camera.transform.position, volume = .4f);
    }

    private void GameControlsManager_OnControlAddedPlaySound(object sender, EventArgs e)
    {
        // Play sound when a new player control is added
        Camera camera = Camera.main;
        PlaySound(audioClipRefsSO.newControlConnected, camera.transform.position, volume = .4f);
    }

    private void CharacterSelectionSingleUI_OnPlayerRemoval(object sender, EventArgs e)
    {
        // Play sound when a player is removed in character selection
        Camera camera = Camera.main;
        PlaySound(audioClipRefsSO.removePlayer, camera.transform.position, volume = .4f);
    }

    private void NewPlayerSingleUI_OnPlayerAdditon(object sender, EventArgs e)
    {
        // Play sound when a new player is added in character selection
        Camera camera = Camera.main;
        PlaySound(audioClipRefsSO.addNewPlayer, camera.transform.position, volume = .4f);
    }

    private void CharacterSelectionSingleUI_OnClick(object sender, EventArgs e)
    {
        // Play sound on button click in character selection
        Camera camera = Camera.main;
        PlaySound(audioClipRefsSO.click, camera.transform.position, volume = .4f);
    }

    private void CharacterSelectionSingleUI_OnReadyUnselection(object sender, EventArgs e)
    {
        // Play sound when a player unselects ready in character selection
        Camera camera = Camera.main;
        PlaySound(audioClipRefsSO.cancel, camera.transform.position, volume = .4f);
    }

    private void CharacterSelectionSingleUI_OnReadySelection(object sender, EventArgs e)
    {
        // Play sound when a player selects ready in character selection
        Camera camera = Camera.main;
        PlaySound(audioClipRefsSO.validation, camera.transform.position, volume = .4f);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        // Play sound when an object is trashed
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectObjectPlacedHere(object sender, EventArgs e)
    {
        // Play sound when an object is placed on a base counter
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        // Play sound when any cutting action occurs
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        // Play sound when a recipe delivery fails
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccessed(object sender, EventArgs e)
    {
        // Play sound when a recipe delivery succeeds
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    public void PlayFoottepsSound(Vector3 position, float volume)
    {
        // Play footstep sound at a specified position with a given volume
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void PlayPickedSomethingSound(Vector3 position, float volume)
    {
        // Play sound when something is picked up at a specified position with a given volume
        PlaySound(audioClipRefsSO.objectPickup, position);
    }

    public void PlayCountdownSound()
    {
        // Play countdown sound at the center of the screen
        PlaySound(audioClipRefsSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        // Play warning sound at a specified position
        PlaySound(audioClipRefsSO.warning[0], position);
    }

    public void ChangeVolume()
    {
        // Change and save the volume level
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        // Get the current volume level
        return volume;
    }
}
