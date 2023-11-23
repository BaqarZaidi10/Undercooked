using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{

    // Reference to the PlatesCounter associated with this visual
    [SerializeField] private PlatesCounter platesCounter;

    // Reference to the point above the counter where plates will be spawned
    [SerializeField] private Transform counterTopPoint;

    // Reference to the prefab used for the visual representation of a plate
    [SerializeField] private Transform plateVisualPrefab;

    // List to store instantiated plate visual GameObjects
    private List<GameObject> plateVisualGameObjectList;

    // Called when the script is first loaded
    private void Awake()
    {
        // Initialize the list to store plate visual GameObjects
        plateVisualGameObjectList = new List<GameObject>();
    }

    // Called at the start of the script's execution
    private void Start()
    {
        // Subscribe to the OnPlateSpawned and OnPlateRemoved events of the associated PlatesCounter
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    // Event handler for the OnPlateRemoved event
    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        // Get the last plate visual GameObject from the list
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];

        // Remove it from the list
        plateVisualGameObjectList.Remove(plateGameObject);

        // Destroy the plate visual GameObject
        Destroy(plateGameObject);
    }

    // Event handler for the OnPlateSpawned event
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        // Instantiate a new plate visual GameObject from the prefab
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        // Offset each plate vertically to stack them
        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        // Add the instantiated plate visual GameObject to the list
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
