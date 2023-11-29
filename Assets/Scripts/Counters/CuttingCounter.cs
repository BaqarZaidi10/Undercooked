using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    // Event triggered when any cutting action occurs
    public static event EventHandler OnAnyCut;

    // Method to reset static data for this class
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    // Event triggered when the progress changes
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    // Event triggered when a cut action is performed
    public event EventHandler OnCut;

    // Array of cutting recipes
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    // Current cutting progress
    private int cuttingProgress;

    // Called when the player interacts with the cutting counter
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchen object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    // Get the cutting recipe for the input
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    // Invoke the progress changed event with the initial progress
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // There is a kitchen object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    // Called when the player performs an alternate interaction with the cutting counter
    public override void InteractAlternate(PlayerController player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a KitchenObject here && can be cut
            cuttingProgress++;

            // Trigger the cut event
            OnCut?.Invoke(this, EventArgs.Empty);

            // Trigger the global cut event
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            // Get the cutting recipe for the input
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            // Invoke the progress changed event with the updated progress
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            // Check if the cutting progress is complete
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // Get the output kitchen object for the input
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // Destroy the input kitchen object
                GetKitchenObject().DestroySelf();

                // Spawn the output kitchen object
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    // Check if there is a cutting recipe for the given input kitchen object
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    // Get the output kitchen object for the given input kitchen object
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null ? cuttingRecipeSO.output : null;
    }

    // Get the cutting recipe for the given input kitchen object
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
