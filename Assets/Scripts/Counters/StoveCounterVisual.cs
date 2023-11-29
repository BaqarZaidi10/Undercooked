using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    // Reference to the associated StoveCounter
    [SerializeField] private StoveCounter stoveCounter;

    // Reference to the visual GameObject when the stove is on
    [SerializeField] private GameObject stoveOnGameObject;

    // Reference to the particles GameObject
    [SerializeField] private GameObject particlesGameObject;

    // Subscribe to the OnStateChanged event when the script starts
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    // Event handler for the StoveCounter's state change
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // Determine whether to show the visual elements based on the StoveCounter's state
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        // Set the active state of the stoveOnGameObject and particlesGameObject
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
