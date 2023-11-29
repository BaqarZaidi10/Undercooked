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

    // Reference to the KitchenObjectSO for plates
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    // Timer variables for spawning plates
    private float spawnPlateTimer = 4f;
    private float spawnPlateTimerMax = 4f;

    // Counter for the number of plates spawned
    private int plateSpawnedAmount;

    // Maximum number of plates that can be spawned
    private int platesSpawnedAmountMax = 4;

    // Called every frame
    private void Update()
    {
        // Check if the game is currently playing
        if (GameManager_.Instance.IsGamePlaying())
        {
            // Increment the spawn timer
            spawnPlateTimer += Time.deltaTime;

            // Check if it's time to spawn a plate
            if (spawnPlateTimer > spawnPlateTimerMax)
            {
                spawnPlateTimer = 0f;

                // Check if the maximum number of plates hasn't been reached
                if (plateSpawnedAmount < platesSpawnedAmountMax)
                {
                    // Increment the plate counter
                    plateSpawnedAmount++;

                    // Trigger the event for plate spawned
                    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    // Event handler for the GameManager OnGameStart event
    private void GameManager_OnGameStart(object sender, EventArgs e)
    {
        // Reset the spawn timer when the game starts
        spawnPlateTimer = 4f;
    }

    // Override of the Interact method from BaseCounter
    public override void Interact(PlayerController player)
    {
        // Check if the player is not carrying any kitchen object
        if (!player.HasKitchenObject())
        {
            // Check if there is at least one plate available
            if (plateSpawnedAmount > 0)
            {
                // Decrement the plate counter
                plateSpawnedAmount--;

                // Spawn a kitchen object (plate) for the player
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                // Trigger the event for plate removed
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
