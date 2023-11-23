using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{

    // Reference to the StoveCounter associated with this visual
    [SerializeField] private StoveCounter stoveCounter;

    // Reference to the GameObject representing the stove when it's turned on
    [SerializeField] private GameObject stoveOnGameObject;

    // Reference to the GameObject containing particles, presumably indicating activity
    [SerializeField] private GameObject particlesGameObject;

    // Called at the start of the script's execution
    private void Start()
    {
        // Subscribe to the OnStateChanged event of the associated StoveCounter
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    // Event handler for the OnStateChanged event
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // Determine whether to show the visual elements based on the stove counter state
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        // Set the active state of the stove visual GameObject
        stoveOnGameObject.SetActive(showVisual);

        // Set the active state of the particles visual GameObject
        particlesGameObject.SetActive(showVisual);
    }
}
