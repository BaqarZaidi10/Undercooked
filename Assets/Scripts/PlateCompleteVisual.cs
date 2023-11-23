using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{

    // A struct to associate KitchenObjectSO with its corresponding GameObject
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    // Reference to the PlateKitchenObject associated with this visual
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // List of KitchenObjectSO and their corresponding GameObjects for visualization
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    // Called at the start of the script's execution
    private void Start()
    {
        // Subscribe to the OnIngredientAdded event of the associated PlateKitchenObject
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        // Set all associated GameObjects to be initially inactive
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    // Event handler for the OnIngredientAdded event
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        // Activate the GameObject corresponding to the added ingredient
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
