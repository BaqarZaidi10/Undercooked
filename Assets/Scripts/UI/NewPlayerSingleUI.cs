using System;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerSingleUI : MonoBehaviour
{
    // Event triggered when a new player is added
    public static event EventHandler OnPlayerAdditon;

    // Static method to reset static data (unsubscribe from events)
    public static void ResetStaticData()
    {
        OnPlayerAdditon = null;
    }

    // UI elements
    [SerializeField] private Button addNewPlayerButton;

    // Event triggered when a new player is added, carrying additional data
    public event EventHandler<EventArgsOnAddNewPlayer> OnAddNewPlayer;

    // EventArgs class for OnAddNewPlayer event, containing a Transform
    public class EventArgsOnAddNewPlayer : EventArgs
    {
        public Transform transform;
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Add a listener for the button click event
        addNewPlayerButton.onClick.AddListener(NotifyAddNewPlayer);
        // Set the button as selected
        addNewPlayerButton.Select();
    }

    // Notify subscribers that a new player is added
    private void NotifyAddNewPlayer()
    {
        // Invoke the OnAddNewPlayer event with the transform information
        OnAddNewPlayer?.Invoke(this, new EventArgsOnAddNewPlayer
        {
            transform = this.transform
        });

        // Invoke the OnPlayerAdditon event without additional data
        OnPlayerAdditon?.Invoke(this, EventArgs.Empty);
    }
}
