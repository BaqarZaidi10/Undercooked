using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container; // Container for recipe UI elements
    [SerializeField] private Transform recipeTemplate; // Reference to the template for a single recipe UI

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false); // Deactivate the template on awake
    }

    private void Start()
    {
        // Subscribe to recipe spawned and completed events
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

        // Update the visual representation of recipes
        UpdateVisual();
    }

    // Event handler for recipe spawned
    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        // Update the visual representation of recipes
        UpdateVisual();
    }

    // Event handler for recipe completed
    private void DeliveryManager_OnRecipeCompleted(object sender, EventArgs e)
    {
        // Update the visual representation of recipes
        UpdateVisual();
    }

    // Update the visual representation of recipes
    private void UpdateVisual()
    {
        // Loop through all children in the container
        foreach (Transform child in container)
        {
            // Skip the template
            if (child == recipeTemplate)
            {
                continue;
            }
            else
            {
                // Destroy other UI elements
                Destroy(child.gameObject);
            }
        }

        // Loop through waiting recipe SOs and create UI elements
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            // Instantiate a recipe UI element from the template
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);

            // Set the recipe SO for the UI element
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
