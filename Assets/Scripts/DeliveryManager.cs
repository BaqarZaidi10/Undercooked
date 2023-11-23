using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    // Events for various recipe-related actions
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    // Singleton pattern to ensure only one instance of DeliveryManager exists
    public static DeliveryManager Instance { get; private set; }

    // Reference to the ScriptableObject containing the list of recipes
    [SerializeField] private RecipeListSO recipeListSO;

    // List of recipes waiting to be delivered
    private List<RecipeSO> waitingRecipeSOList;

    // Timer for spawning new recipes
    private float spawnRecipeTimer;

    // Maximum time between spawning new recipes
    private float spawnRecipeTimerMax = 4f;

    // Maximum number of recipes that can be waiting at a time
    private int waitingRecipesMax = 4;

    // Number of successfully delivered recipes
    private int successfulRecipesAmount;

    // Called when the script instance is being loaded
    private void Awake()
    {
        Instance = this; // Set the singleton instance
        waitingRecipeSOList = new List<RecipeSO>(); // Initialize the list of waiting recipes
    }

    // Update is called once per frame
    private void Update()
    {
        // Decrement the spawn timer
        spawnRecipeTimer -= Time.deltaTime;

        // Spawn a new recipe if the conditions are met
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Check if the game is currently playing and if the maximum number of waiting recipes hasn't been reached
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                // Randomly select a recipe from the recipe list
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];

                // Add the selected recipe to the list of waiting recipes
                waitingRecipeSOList.Add(waitingRecipeSO);

                // Invoke the event signaling that a new recipe has been spawned
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Process the delivery of a recipe based on the contents of a delivered plate
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            // Check if the number of ingredients in the delivered plate matches the waiting recipe
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Check if each ingredient in the recipe is present in the plate
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // Check if the recipe ingredient is found in the plate
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // If the ingredient is found, set the flag to true and break out of the inner loop
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    // If the ingredient is not found in the plate, set the flag to false
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }

                // If the plate contents match the recipe, the delivery is successful
                if (plateContentsMatchesRecipe)
                {
                    successfulRecipesAmount++; // Increment the successful recipes count
                    waitingRecipeSOList.RemoveAt(i); // Remove the delivered recipe from the waiting list

                    // Invoke events signaling the completion and success of the delivered recipe
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return; // Exit the method after successful delivery
                }
            }
        }

        // If no matches are found, invoke the event signaling the failure of the delivered recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    // Get the list of waiting recipes
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    // Get the number of successfully delivered recipes
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
