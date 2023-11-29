using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    // Event triggered when any object is placed on this counter
    public static event EventHandler OnAnyObjectObjectPlacedHere;

    // Reset static data method, clearing the event subscribers
    public static void ResetStaticData()
    {
        OnAnyObjectObjectPlacedHere = null;
    }

    // Reference to the transform representing the top point of the counter
    [SerializeField] private Transform counterTopPoint;

    // Reference to the currently placed kitchen object
    private KitchenObject kitchenObject;

    // Method called when interacting with the counter (to be overridden in derived classes)
    public virtual void Interact(PlayerController player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    // Method called when alternate interacting with the counter (to be overridden in derived classes)
    public virtual void InteractAlternate(PlayerController player)
    {
        // Debug.LogError("BaseCounter.InteractAlternate();");
    }

    // Get the transform to follow for the kitchen object
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    // Set the currently placed kitchen object
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        // Invoke the event when any object is placed on this counter
        if (kitchenObject != null)
        {
            OnAnyObjectObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    // Get the currently placed kitchen object
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Clear the currently placed kitchen object
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // Check if there is a kitchen object placed on this counter
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
