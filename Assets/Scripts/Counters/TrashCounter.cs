using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    // Event triggered when any object is trashed
    public static event EventHandler OnAnyObjectTrashed;

    // Reset static data method to clear event subscriptions
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    // Override method to handle interaction with the trash counter
    public override void Interact(PlayerController player)
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
