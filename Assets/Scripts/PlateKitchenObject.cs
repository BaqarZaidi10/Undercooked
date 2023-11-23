using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    // Event triggered when an ingredient is added to the plate
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    // Custom EventArgs class for the OnIngredientAdded event
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    // List of valid kitchen objects that can be added to the plate
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    // List of kitchen objects currently on the plate
    private List<KitchenObjectSO> kitchenObjectSOList;

    // Called when the script instance is being loaded
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>(); // Initialize the list of kitchen objects on the plate
    }

    // Try to add an ingredient to the plate
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // Check if the ingredient is valid for this plate
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Not a valid ingredient
            return false;
        }

        // Check if the plate already has this type of ingredient
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Already has this type
            return false;
        }
        else
        {
            // Add the ingredient to the plate's list
            kitchenObjectSOList.Add(kitchenObjectSO);

            // Invoke the event signaling that an ingredient has been added
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true; // Successfully added the ingredient
        }
    }

    // Get the list of kitchen objects currently on the plate
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
