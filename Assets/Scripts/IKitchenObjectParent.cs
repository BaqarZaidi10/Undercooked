using UnityEngine;

public interface IKitchenObjectParent
{

    // Get the transform that this kitchen object should follow
    Transform GetKitchenObjectFollowTransform();

    // Set the kitchen object associated with this parent
    void SetKitchenObject(KitchenObject kitchenObject);

    // Get the kitchen object associated with this parent
    KitchenObject GetKitchenObject();

    // Clear the reference to the kitchen object from this parent
    void ClearKitchenObject();

    // Check if this parent currently has a kitchen object
    bool HasKitchenObject();
}
