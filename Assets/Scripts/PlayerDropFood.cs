using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropFood : MonoBehaviour
{
    PlayerInputActions playerInputActions = new PlayerInputActions();
    private PlayerController player;
    public event EventHandler OnInterectAction;
    private void Awake()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interect_performed;
    }

    private void Update()
    {
        if (player.HasKitchenObject())
        {
           
        }
            
    }

    private void Interect_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInterectAction?.Invoke(this, EventArgs.Empty);
    }

    // public override void Interact(PlayerController player)
    // {
    //     if (player.HasKitchenObject())
    //     {
    //         player.GetKitchenObject().DestroySelf();
    //     }
    // }
}
