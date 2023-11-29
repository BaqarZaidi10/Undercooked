using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyNavigationUI : MonoBehaviour
{
    // Event triggered when lobby navigation is loaded
    public static event EventHandler OnLobbyNavigationLoaded;

    // Static method to reset static data
    public static void ResetStaticData()
    {
        OnLobbyNavigationLoaded = null;
    }

    // Singleton instance
    public static LobbyNavigationUI Instance { get; private set; }

    // Serialized field for inspector reference
    [SerializeField] private Transform container;

    // Number of direct children in the container
    private int numberOfDirectChildren;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Implementing the singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Add event handler for UI changes in the LobbyUI
        LobbyUI.Instance.OnUIChanged += LobbyUI_OnUIChanged;

        // Set the selected game object using EventSystem
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(container.GetChild(0).GetComponent<LobbyElementsNavigationUI>().GetFirstSelected().gameObject);
    }

    // Event handler for UI changes in the LobbyUI
    private void LobbyUI_OnUIChanged(object sender, System.EventArgs e)
    {
        // Update the number of direct children and trigger the navigation loaded event
        numberOfDirectChildren = container.childCount;
        OnLobbyNavigationLoaded?.Invoke(this, EventArgs.Empty);
    }

    // Get the next sibling in the container based on the current sibling index
    public Transform GetNextSibling(int siblingIndex)
    {
        // Calculate the index of the next sibling
        int nextSiblingIndex = (siblingIndex + 1) % numberOfDirectChildren;

        // Return the next sibling if it is active, otherwise, recursively find the next active sibling
        return container.GetChild(nextSiblingIndex).gameObject.activeSelf ? container.GetChild(nextSiblingIndex) : GetNextSibling(nextSiblingIndex);
    }

    // Get the previous sibling in the container based on the current sibling index
    public Transform GetPreviousSibling(int siblingIndex)
    {
        // Calculate the index of the previous sibling
        int previousSiblingIndex = siblingIndex > 0 ? siblingIndex - 1 : numberOfDirectChildren - 1;

        // Return the previous sibling if it is active, otherwise, recursively find the previous active sibling
        return container.GetChild(previousSiblingIndex).gameObject.activeSelf ? container.GetChild(previousSiblingIndex) : GetPreviousSibling(previousSiblingIndex);
    }
}
