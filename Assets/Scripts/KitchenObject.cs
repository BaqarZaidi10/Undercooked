using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    // Reference to the ScriptableObject containing data for this kitchen object
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Reference to the parent object that holds this kitchen object
    private IKitchenObjectParent kitchenObjectParent;

    // Get the ScriptableObject associated with this kitchen object
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    // Set the parent for this kitchen object
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // Clear the previous kitchen object from the parent
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        // Set the new parent for this kitchen object
        this.kitchenObjectParent = kitchenObjectParent;

        // Display an error if the parent already has a kitchen object
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
        }

        // Set this kitchen object as the child of the parent and reset its position
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    // Get the parent object that holds this kitchen object
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    // Destroy this kitchen object and clear it from its parent
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    // Try to get a PlateKitchenObject from this kitchen object
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    // Static method to spawn a new kitchen object based on a ScriptableObject and parent
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        // Instantiate the prefab associated with the kitchen object
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        // Get the KitchenObject component from the instantiated transform
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        // Set the parent for the spawned kitchen object
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        // Return the spawned kitchen object
        return kitchenObject;
    }
}
