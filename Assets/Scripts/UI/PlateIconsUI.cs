using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    // Reference to the PlateKitchenObject
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // Reference to the template for the icons
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        // Deactivate the template to avoid it being visible initially
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Subscribe to the OnIngredientAdded event of the PlateKitchenObject
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngreOnIngredientAddedEventArgs e)
    {
        // Update the visual representation when ingredients are added
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Iterate through existing child transforms (icons) in the container
        foreach (Transform child in transform)
        {
            // Skip the template to avoid destroying it
            if (child == iconTemplate) continue;

            // Destroy each existing child (icon) in the container
            Destroy(child.gameObject);
        }

        // Iterate through the kitchen object SOs in the PlateKitchenObject
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            // Instantiate a new icon based on the template
            Transform iconTransform = Instantiate(iconTemplate, transform);

            // Activate the icon
            iconTransform.gameObject.SetActive(true);

            // Set the kitchen object SO for the icon
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
