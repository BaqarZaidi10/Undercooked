using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    // Event triggered when progress changes
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    // Event triggered when the state of the stove counter changes
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    // Custom event arguments for the state change event
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    // Enumeration representing the possible states of the stove counter
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt,
    }

    // Array of frying recipes
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    // Array of burning recipes
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    // Current state of the stove counter
    private State state;

    // Timer for frying
    private float fryingTimer;

    // Current frying recipe
    private FryingRecipeSO fryingRecipeSO;

    // Timer for burning
    private float burningTimer;

    // Current burning recipe
    private BurningRecipeSO burningRecipeSO;

    // Start is called before the first frame update
    private void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(   "pan"+ HasKitchenObject());
        // Check if the stove counter has a kitchen object
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    // Do nothing in the idle state
                    break;
                case State.Frying:
                    // Increment frying timer
                    fryingTimer += Time.deltaTime;

                    // Invoke progress changed event
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    // Check if frying is complete
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        // Invoke state changed event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    // Increment burning timer
                    burningTimer += Time.deltaTime;

                    // Invoke progress changed event
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    // Check if burning is complete
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        // Burnt
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burnt;

                        // Invoke state changed event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        // Invoke progress changed event with progress reset to 0
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burnt:
                    // Do nothing in the burnt state
                    break;
            }
        }
    }

    // Interact method when the player interacts with the stove counter
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
                    // Player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    // Invoke state changed event
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    // Invoke progress changed event with initial progress
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
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
                        state = State.Idle;

                        // Invoke state changed event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        // Invoke progress changed event with progress reset to 0
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                // Invoke state changed event
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                // Invoke progress changed event with progress reset to 0
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    // Check if there is a recipe with the given input
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    // Get the output kitchen object for the given input
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null ? fryingRecipeSO.output : null;
    }

    // Get the frying recipe with the given input
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    // Get the burning recipe with the given input
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    // Check if the stove counter is in the Fried state
    public bool IsFried()
    {
        return (state == State.Fried);
    }
}
