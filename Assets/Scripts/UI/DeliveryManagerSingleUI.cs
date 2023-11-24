using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        // Deactivate the icon template since it's only a reference
        iconTemplate.gameObject.SetActive(false);
    }

    // Sets up the UI elements based on the provided RecipeSO
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        // Display the recipe name on the UI
        recipeNameText.text = recipeSO.recipeName;

        // Clear existing icons in the container
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // Instantiate and display icons for each KitchenObjectSO in the recipe
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);

            // Ensure the instantiated icon is visible
            iconTransform.gameObject.SetActive(true);

            // Set the sprite of the icon based on the KitchenObjectSO
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
