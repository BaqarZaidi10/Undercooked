using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    // Event triggered when a plate is spawned
    public event EventHandler OnPlateSpawned;

    // Event triggered when a plate is removed
    public event EventHandler OnPlateRemoved;

    // The kitchen object scriptable object for plates
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    // Timer variables for spawning plates
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    // Counters for the plates spawned and the maximum allowed plates
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    // Called every frame
    private void Update()
    {
        // Increment the spawn timer
        spawnPlateTimer += Time.deltaTime;

        // Check if it's time to spawn a plate
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            // Reset the timer
            spawnPlateTimer = 0f;

            // Check if the game is playing and the maximum plates haven't been reached
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                // Increment the spawned plates counter
                platesSpawnedAmount++;

                // Trigger the OnPlateSpawned event
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Interaction method when a player interacts with the counter
    public override void Interact(Player player)
    {
        // Check if the player is empty-handed
        if (!player.HasKitchenObject())
        {
            // Check if there is at least one plate available
            if (platesSpawnedAmount > 0)
            {
                // Decrement the spawned plates counter
                platesSpawnedAmount--;

                // Spawn a plate in the player's hands
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                // Trigger the OnPlateRemoved event
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
