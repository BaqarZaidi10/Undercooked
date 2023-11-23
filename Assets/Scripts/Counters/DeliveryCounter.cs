using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    // Singleton instance of the DeliveryCounter
    public static DeliveryCounter Instance { get; private set; }

    // Called when the script instance is loaded
    private void Awake()
    {
        // Set the singleton instance
        Instance = this;
    }

    // Interaction method when a player interacts with the delivery counter
    public override void Interact(Player player)
    {
        // Check if the player is carrying a kitchen object
        if (player.HasKitchenObject())
        {
            // Check if the carried object is a PlateKitchenObject
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Only accepts Plates

                // Deliver the recipe associated with the plate
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                // Destroy the plate in the player's hands
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
