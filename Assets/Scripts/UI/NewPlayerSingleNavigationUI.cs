using System;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerSingleNavigationUI : LobbyElementsNavigationUI
{
    // UI element for adding a new player
    [SerializeField] private Button addNewPlayerButton;

    // Start is called before the first frame update
    private void Start()
    {
        // Subscribe to the event when the lobby navigation is loaded
        LobbyNavigationUI.OnLobbyNavigationLoaded += LobbyNavigationUI_OnLobbyNavigationLoaded;
    }

    // Called when the object becomes disabled or inactive
    private void OnDisable()
    {
        // Unsubscribe from the event when the object is disabled
        LobbyNavigationUI.OnLobbyNavigationLoaded -= LobbyNavigationUI_OnLobbyNavigationLoaded;
    }

    // Event handler for the OnLobbyNavigationLoaded event
    private void LobbyNavigationUI_OnLobbyNavigationLoaded(object sender, EventArgs e)
    {
        // Set the navigation properties for the addNewPlayerButton
        addNewPlayerButton.navigation = new Navigation()
        {
            // Explicit mode allows custom navigation settings
            mode = Navigation.Mode.Explicit,
            // No navigation on the "Down" direction
            selectOnDown = null,
            // Set navigation for the "Left" direction
            selectOnLeft = defaultSelectOnLeft_LeftSideSelectable(),
            // Set navigation for the "Right" direction
            selectOnRight = defaultSelectOnRight_RightSideSelectable(),
            // No navigation on the "Up" direction
            selectOnUp = null
        };
    }
}
