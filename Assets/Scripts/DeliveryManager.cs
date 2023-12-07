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

    public int lastScore = -1;

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

    public void NewRecipe()
    {
        if (waitingRecipeSOList.Count > 0)
            waitingRecipeSOList.RemoveAt(0);

        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
        waitingRecipeSOList.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    // Method for delivering a recipe
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        int score = 10;

        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {

                bool emptyRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // Cycling through all ingredients in the recipe
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        Debug.Log("Checking ingredient...");
                        // Cycling through all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            Debug.Log("Ingredient Correct");
                            emptyRecipe = false;
                        }
                        else if (plateKitchenObjectSO.burned)
                        {
                            Debug.Log("Ingredient Burned");
                            score -= 3;
                        }
                        else if (plateKitchenObjectSO.raw)
                        {
                            Debug.Log("Ingredient Raw");
                            score -= 5;
                        }
                        else
                        {
                            Debug.Log("Ingredient Missing");
                            score -= 2;
                        }
                    }
                }

                if (emptyRecipe)
                {
                    score = 0;
                }

                // Player delivered the correct recipe
                successfulRecipesAmount++;

                //waitingRecipeSOList.RemoveAt(i);

                lastScore = score;

                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccessed?.Invoke(this, EventArgs.Empty);
            }
        }

        // No matches found, the player did not deliver the correct recipe
        //OnRecipeFailed?.Invoke(this, EventArgs.Empty);
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
