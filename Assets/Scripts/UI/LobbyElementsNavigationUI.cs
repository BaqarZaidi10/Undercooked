using UnityEngine;
using UnityEngine.UI;

public class LobbyElementsNavigationUI : MonoBehaviour
{
    // Serialized field for inspector reference
    [SerializeField] protected Selectable firstSelected;

    // Method to get the default selectable on the right (next sibling on the right side)
    protected Selectable defaultSelectOnRight_RightSideSelectable()
    {
        // Get the next sibling's first selected from the LobbyNavigationUI
        return LobbyNavigationUI.Instance.GetNextSibling(transform.GetSiblingIndex()).GetComponent<LobbyElementsNavigationUI>().GetFirstSelected();
    }

    // Method to get the default selectable on the left (previous sibling on the left side)
    protected Selectable defaultSelectOnLeft_LeftSideSelectable()
    {
        // Get the previous sibling's first selected from the LobbyNavigationUI
        return LobbyNavigationUI.Instance.GetPreviousSibling(transform.GetSiblingIndex()).GetComponent<LobbyElementsNavigationUI>().GetFirstSelected();
    }

    // Method to get the first selected selectable
    public Selectable GetFirstSelected()
    {
        return firstSelected;
    }
}
