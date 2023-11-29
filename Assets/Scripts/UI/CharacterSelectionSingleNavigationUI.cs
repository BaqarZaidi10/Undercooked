using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionSingleNavigationUI : LobbyElementsNavigationUI
{
    // Serialized fields for UI buttons
    [SerializeField] private Button nextSkinButton;
    [SerializeField] private Button previousSkinButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button nextControlButton;
    [SerializeField] private Button previousControlButton;
    [SerializeField] private Button removePlayerButton;

    // Start is called before the first frame update
    private void Start()
    {
        // Subscribe to the event triggered when lobby navigation is loaded
        LobbyNavigationUI.OnLobbyNavigationLoaded += LobbyNavigationUI_OnLobbyNavigationLoaded;
    }

    // Called when the script instance is being disabled
    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        LobbyNavigationUI.OnLobbyNavigationLoaded -= LobbyNavigationUI_OnLobbyNavigationLoaded;
    }

    // Event handler for lobby navigation loaded event
    private void LobbyNavigationUI_OnLobbyNavigationLoaded(object sender, EventArgs e)
    {
        // Determine the buttons to navigate to on Up and Down based on the state of removePlayerButton
        Selectable selectOnUp_NextSkinButton = readyButton;
        Selectable selectOnDown_ReadyButton = nextSkinButton;
        if (removePlayerButton.gameObject.activeSelf)
        {
            selectOnUp_NextSkinButton = removePlayerButton;
            selectOnDown_ReadyButton = removePlayerButton;
        }

        // Explicitly set the navigation for each button

        // Next Skin Button navigation settings
        nextSkinButton.navigation = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            selectOnDown = nextControlButton,
            selectOnLeft = previousSkinButton,
            selectOnRight = defaultSelectOnRight_RightSideSelectable(),
            selectOnUp = selectOnUp_NextSkinButton
        };

        // Previous Skin Button navigation settings
        previousSkinButton.navigation = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            selectOnDown = previousControlButton,
            selectOnLeft = defaultSelectOnLeft_LeftSideSelectable(),
            selectOnRight = nextSkinButton,
            selectOnUp = selectOnUp_NextSkinButton
        };

        // Next Control Button navigation settings
        nextControlButton.navigation = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            selectOnDown = readyButton,
            selectOnLeft = previousControlButton,
            selectOnRight = defaultSelectOnRight_RightSideSelectable(),
            selectOnUp = nextSkinButton
        };

        // Previous Control Button navigation settings
        previousControlButton.navigation = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            selectOnDown = readyButton,
            selectOnLeft = defaultSelectOnLeft_LeftSideSelectable(),
            selectOnRight = nextControlButton,
            selectOnUp = previousSkinButton
        };

        // Ready Button navigation settings
        readyButton.navigation = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            selectOnDown = selectOnDown_ReadyButton,
            selectOnLeft = defaultSelectOnLeft_LeftSideSelectable(),
            selectOnRight = defaultSelectOnRight_RightSideSelectable(),
            selectOnUp = nextControlButton
        };

        // Remove Player Button navigation settings
        removePlayerButton.navigation = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            selectOnDown = nextSkinButton,
            selectOnLeft = previousSkinButton,
            selectOnRight = nextSkinButton,
            selectOnUp = readyButton
        };
    }
}
