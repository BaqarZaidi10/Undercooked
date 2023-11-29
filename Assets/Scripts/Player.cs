using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    // Events for player actions
    public static event EventHandler OnPickedSomething;
    public static event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // Struct for event arguments when the selected counter changes
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // Serialized fields for player parameters and references
    [SerializeField] private float moveSpeed = 7f;
    //[SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask = 1 << 6;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private CharacterController controller;
    private Vector2 inputVector;
    private bool interactPressed;
    private bool altInteractPressed;
    private bool pausePressed;

    // Private variables for internal state
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    // Events for different input actions
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;    

    // Input actions for player controls
    private PlayerInputActions playerInputActions;

    // Called when the script instance is being loaded
    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions(); // Initialize the player input actions
        
        // Enable player input actions
        playerInputActions.Player.Enable(); 

        // Subscribe to input action events
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    // Called when the script is being destroyed
    private void OnDisable()
    {
        // Unsubscribe from input action events
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        // Dispose of player input actions
        playerInputActions.Dispose();
    }

    private void Awake()
    {
        kitchenObjectHoldPoint = GetComponentInChildren<KitchenObjectHoldPoint>().transform;
        //gameInput = GameObject.Find("GameInput").GetComponent<GameInput>();
        controller = GetComponent<CharacterController>();
    }

    // Event handler for pause input action
    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    // Event handler for alternate interact input action
    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    // Event handler for primary interact input action
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // Start method for initialization
    private void Start()
    {
        // Subscribe to input events
        OnInteractAction += GameInput_OnInteractAction;
        OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    // Input event handlers
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        // Handle alternate interaction action
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        // Handle regular interaction action
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Update method for player logic
    private void Update()
    {
        // Handle movement and interactions
        HandleMovement();
        HandleInteractions();
    }

    // Method to check if the player is walking
    public bool IsWalking()
    {
        return isWalking;
    }

    // Method to handle interactions with counters
    private void HandleInteractions()
    {
        // Get normalized movement vector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Update the last interact direction
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        // Raycast to find interactable counters
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has BaseCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
    }
    
    public bool OnInteract(InputAction.CallbackContext context)
    {
        interactPressed = context.action.triggered;
        return interactPressed;
    }

    // Method to handle player movement
    private void HandleMovement()
    {
        // Get normalized movement vector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Check for obstacles and adjust movement direction
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Attempt movement only on X-axis
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt movement only on Z-axis
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        // Move the player
        if (canMove)
        {
            controller.Move(moveDir * moveDistance);
        }

        // Update walking state
        isWalking = moveDir != Vector3.zero;

        // Smoothly rotate the player to face the movement direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Method to set the selected counter and invoke the event
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    // Implementation of IKitchenObjectParent interface

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
