using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    // Reference to the PlatesCounter script
    [SerializeField] private PlatesCounter platesCounter;

    // Reference to the counter top point transform
    [SerializeField] private Transform counterTopPoint;

    // Reference to the plate visual prefab
    [SerializeField] private Transform plateVisualPrefab;

    // List to store instantiated plate visual game objects
    private List<GameObject> plateVisualGameObjectList;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Initialize the list
        plateVisualGameObjectList = new List<GameObject>();
    }

    // Called before the first frame update
    private void Start()
    {
        // Subscribe to the PlatesCounter events
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlatesSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlatesRemoved;
    }

    // Event handler for the OnPlatesRemoved event
    private void PlatesCounter_OnPlatesRemoved(object sender, EventArgs e)
    {
        // Get the last plate visual game object from the list
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];

        // Remove the plate visual game object from the list
        plateVisualGameObjectList.Remove(plateGameObject);

        // Destroy the plate visual game object
        Destroy(plateGameObject);
    }

    // Event handler for the OnPlatesSpawned event
    private void PlatesCounter_OnPlatesSpawned(object sender, EventArgs e)
    {
        // Instantiate a new plate visual transform
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        // Set the local position of the plate visual based on the count
        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        // Add the plate visual game object to the list
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
