using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    // Serializable struct to associate KitchenObjectSO with its corresponding GameObject
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    // Reference to the PlateKitchenObject that this visual represents
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // List of KitchenObjectSOs and their corresponding GameObjects
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;

    // Start is called before the first frame update
    private void Start()
    {
        // Subscribe to the PlateKitchenObject's OnIngredientAdded event
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        // Set all GameObjects in the list to be initially inactive
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList)
        {
            kitchenObjectSO_GameObject.gameObject.SetActive(false);
        }
    }

    // Event handler for the PlateKitchenObject's OnIngredientAdded event
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngreOnIngredientAddedEventArgs e)
    {
        // Iterate through the list of KitchenObjectSO_GameObjects
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList)
        {
            // Check if the KitchenObjectSO matches the added ingredient
            if (kitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                // Activate the corresponding GameObject to visually represent the added ingredient
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
    }
}
