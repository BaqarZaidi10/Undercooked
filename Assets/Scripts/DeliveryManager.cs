using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    // Events for recipe-related actions
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccessed;
    public event EventHandler OnRecipeFailed;

    // Singleton instance
    public static DeliveryManager Instance { get; private set; }

    // Serialized field for recipe list scriptable object
    [SerializeField] private RecipeListSO recipeListSO;

    // Lists and counters for managing recipes
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer = 0f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    // Awake method for initialization
    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    // Update method for spawning recipes
    private void Update()
    {
        if (GameManager_.Instance.IsGamePlaying())
        {
            spawnRecipeTimer -= Time.deltaTime;
            if (spawnRecipeTimer <= 0f)
            {
                spawnRecipeTimer = spawnRecipeTimerMax;

                if (waitingRecipeSOList.Count < waitingRecipesMax)
                {
                    // Spawn recipe
                    RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                    waitingRecipeSOList.Add(waitingRecipeSO);

                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    // Method for delivering a recipe
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has the same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // Cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient does match
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was not found on the plate
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe)
                {
                    // Player delivered the correct recipe
                    successfulRecipesAmount++;

                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccessed?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }

        // No matches found, the player did not deliver the correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    // Getter for the waiting recipe list
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    // Getter for the number of successful recipes
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }

    
}
