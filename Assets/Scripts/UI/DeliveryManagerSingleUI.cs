using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText; // Text displaying the recipe name
    [SerializeField] private Transform iconContainer; // Container for recipe icons
    [SerializeField] private Transform iconTemplate; // Reference to the template for a single recipe icon

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false); // Deactivate the template on awake
    }

    // Set the recipe SO for the UI element
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        // Set the text to display the recipe name
        recipeNameText.text = recipeSO.recipeName;

        // Loop through all children in the icon container
        foreach (Transform child in iconContainer)
        {
            // Skip the template
            if (child == iconTemplate) continue;

            // Destroy other UI elements
            Destroy(child.gameObject);
        }

        // Loop through kitchen object SOs in the recipe and create UI elements
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            // Instantiate a recipe icon from the template
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);

            // Set the sprite of the recipe icon
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
