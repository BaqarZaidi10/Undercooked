using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ContainerCounter class extends BaseCounter
public class ContainerCounter : BaseCounter
{

    // Event triggered when the player grabs an object from the counter
    public event EventHandler OnPlayerGrabbedObject;

    // Reference to the KitchenObjectSO associated with this counter
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Called when the player interacts with the container counter
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carrying anything
            // Spawn a KitchenObject based on the specified KitchenObjectSO and attach it to the player
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            // Trigger the OnPlayerGrabbedObject event
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
