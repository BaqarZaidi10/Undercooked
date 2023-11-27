using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        // Deactivate the icon template since it's only a reference
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Subscribe to the event triggered when an ingredient is added to the plate
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        // Update the visual representation of the plate's ingredients
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Clear existing icons in the container
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // Instantiate and display icons for each KitchenObjectSO in the plate
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);

            // Ensure the instantiated icon is visible
            iconTransform.gameObject.SetActive(true);

            // Set the KitchenObjectSO for the icon
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
