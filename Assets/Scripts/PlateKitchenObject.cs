using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    // Event triggered when an ingredient is added to the plate
    public event EventHandler<OnIngreOnIngredientAddedEventArgs> OnIngredientAdded;

    // Event arguments class for the OnIngredientAdded event
    public class OnIngreOnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    // List of valid KitchenObjectSOs that can be added to the plate
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    // List to store the current KitchenObjectSOs on the plate
    private List<KitchenObjectSO> kitchenObjectSOList;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize the list to store KitchenObjectSOs
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    // Attempt to add an ingredient to the plate
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // Check if the ingredient is not valid
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Not a valid ingredient
            return false;
        }

        // Check if the plate already has this type of ingredient
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Already has this type of ingredient
            return false;
        }
        else
        {
            // Add the ingredient to the plate's list
            kitchenObjectSOList.Add(kitchenObjectSO);

            // Trigger the OnIngredientAdded event
            OnIngredientAdded?.Invoke(this, new OnIngreOnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            // Return true to indicate successful addition
            return true;
        }
    }

    // Get the list of KitchenObjectSOs on the plate
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
