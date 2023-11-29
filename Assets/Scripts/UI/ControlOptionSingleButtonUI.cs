using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ControlOptionSingleButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI controlOptionText; // Text component displaying the control option

    // Set the text for the control option
    public void SetControlOptionText(string controlOptionText)
    {
        this.controlOptionText.text = controlOptionText;
    }

    // Get the text of the control option
    public string GetControlOption()
    {
        return this.controlOptionText.text;
    }
}
