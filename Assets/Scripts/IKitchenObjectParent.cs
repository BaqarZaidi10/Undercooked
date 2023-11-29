using UnityEngine;

// Interface for objects that can act as parents for kitchen objects
public interface IKitchenObjectParent
{
    // Returns the transform that a kitchen object should follow
    Transform GetKitchenObjectFollowTransform();

    // Sets the kitchen object associated with this parent
    void SetKitchenObject(KitchenObject kitchenObject);

    // Gets the kitchen object associated with this parent
    KitchenObject GetKitchenObject();

    // Clears the association with the kitchen object
    void ClearKitchenObject();

    // Checks if this parent currently has a kitchen object associated with it
    bool HasKitchenObject();
}
