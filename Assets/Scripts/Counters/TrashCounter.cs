using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    // Event triggered when any object is trashed
    public static event EventHandler OnAnyObjectTrashed;

    // Reset static data method (hiding the one in the base class)
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    // Override the Interact method from the base class
    public override void Interact(Player player)
    {
        // Check if the player is holding a kitchen object
        if (player.HasKitchenObject())
        {
            // Destroy the kitchen object held by the player
            player.GetKitchenObject().DestroySelf();

            // Trigger the OnAnyObjectTrashed event
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
