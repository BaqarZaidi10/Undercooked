using System;
using UnityEngine;

public class OvenCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Baking,
        Baked,
        Burnt,
    }

    [SerializeField] private OvenRecipe[] ovenRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float bakingTimer;
    private OvenRecipe ovenRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        Debug.Log(HasKitchenObject());
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    // Do nothing in the idle state
                    break;
                case State.Baking:
                    bakingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = bakingTimer / ovenRecipeSO.bakingTimerMax
                    });

                    if (bakingTimer > ovenRecipeSO.bakingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(ovenRecipeSO.output, this);

                        state = State.Baked;
                        bakingTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Baked:
                    bakingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = bakingTimer / burningRecipeSO.burningTimerMax
                    });

                    if (bakingTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burnt;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

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

    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    ovenRecipeSO = GetOvenRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Baking;
                    bakingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = bakingTimer / ovenRecipeSO.bakingTimerMax
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
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        OvenRecipe ovenRecipe = GetOvenRecipeSOWithInput(inputKitchenObjectSO);
        return ovenRecipe != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        OvenRecipe ovenRecipe = GetOvenRecipeSOWithInput(inputKitchenObjectSO);
        return ovenRecipe != null ? ovenRecipe.output : null;
    }

    private OvenRecipe GetOvenRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (OvenRecipe ovenRecipe in ovenRecipeSOArray)
        {
            if (ovenRecipe.input == inputKitchenObjectSO)
            {
                return ovenRecipe;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipe in burningRecipeSOArray)
        {
            if (burningRecipe.input == inputKitchenObjectSO)
            {
                return burningRecipe;
            }
        }
        return null;
    }

    public bool IsBaked()
    {
        return (state == State.Baked);
    }
}
