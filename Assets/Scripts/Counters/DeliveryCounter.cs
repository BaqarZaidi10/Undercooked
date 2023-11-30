using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    // Singleton instance of the DeliveryCounter
    public static DeliveryCounter Instance { get; private set; }

    public GameObject currentPlayer;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Set the singleton instance to this object
        Instance = this;
        currentPlayer = null;
    }

    // Override of the Interact method from BaseCounter
    public override void Interact(PlayerController player)
    {
        // Check if the player is carrying a kitchen object
        if (player.HasKitchenObject())
        {
            // Attempt to get a PlateKitchenObject from the player's kitchen object
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // DeliveryCounter only accepts plates
                // Deliver the recipe associated with the plate to the DeliveryManager
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                // Destroy the kitchen object (plate) carried by the player
                player.GetKitchenObject().DestroySelf();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentPlayer = other.gameObject;
        print(currentPlayer.name);
    }

    private void OnTriggerExit(Collider other)
    {
        currentPlayer = null;
    }
}
