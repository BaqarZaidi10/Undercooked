using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BaseCounter class implements IKitchenObjectParent interface and extends MonoBehaviour
public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    // Event triggered when any object is placed on a counter
    public static event EventHandler OnAnyObjectPlacedHere;

    // Static method to reset static data, clearing the event handlers
    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    // Reference to the point on the counter where kitchen objects should be placed
    [SerializeField] private Transform counterTopPoint;

    // Reference to the kitchen object currently on the counter
    private KitchenObject kitchenObject;

    // Called when the player interacts with the counter
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    // Called when the player interacts with the counter in an alternate way
    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate();");
    }

    // Returns the transform where the kitchen object should follow
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    // Sets the kitchen object currently on the counter
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            // Trigger the OnAnyObjectPlacedHere event
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    // Gets the kitchen object currently on the counter
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Clears the kitchen object from the counter
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // Checks if the counter currently has a kitchen object
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
