using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    // Events for player actions
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // Serialized fields visible in the Unity editor
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    // Private variables
    private bool isWalking;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private PlayerInputActions playerInputActions;

    private void Start()
    {
        // Subscribe to input events when the player is instantiated
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void OnDisable()
    {
        // Unsubscribe from input events when the player is disabled or destroyed
        GameInput.Instance.DestroyPlayerInputActions(playerInputActions);
    }

    private void GameInput_OnInteractAction(object sender, GameInput.OnInteractActionEventArgs e)
    {
        // Handle primary interaction action
        if (!GameManager_.Instance.IsGamePlaying()) return;

        if (selectedCounter != null && GameInput.Instance.isActionMine(e.action, playerInputActions))
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, GameInput.OnInteractAlternateActionEventArgs e)
    {
        // Handle alternate interaction action
        if (!GameManager_.Instance.IsGamePlaying()) return;

        if (selectedCounter != null && GameInput.Instance.isActionMine(e.action, playerInputActions))
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {
        // Update player movement and interactions
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        // Determine the direction of the last interaction
        Vector2 inputVector2Normalized = GameInput.Instance.GetMovementVectorNormalized(playerInputActions);
        Vector3 moveDirection = new Vector3(inputVector2Normalized.x, 0, inputVector2Normalized.y);

        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }

        // Cast a ray to detect nearby counters for interaction
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter
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

    private void HandleMovement()
    {
        // Handle player movement
        Vector2 inputVector2Normalized = GameInput.Instance.GetMovementVectorNormalized(playerInputActions);
        Vector3 moveDirection = new Vector3(inputVector2Normalized.x, 0, inputVector2Normalized.y);

        float moveDistance = movementSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        // Check for obstacles using capsule casting
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            // Cannot move towards the direction, attempt only X movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = (moveDirection.x < -.5f || moveDirection.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                // Cannot move only on the X, attempt only Z movement
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = (moveDirection.z < -.5f || moveDirection.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDirection * movementSpeed * Time.deltaTime;
        }

        // Update walking status and rotation
        isWalking = moveDirection != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        // Set the selected counter and invoke the event
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        // Return the kitchen object follow transform
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        // Set the held kitchen object and invoke the event
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        // Return the held kitchen object
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        // Clear the held kitchen object
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        // Check if the player is holding a kitchen object
        return kitchenObject != null;
    }

    public void SetPlayerInputActions(PlayerInputActions playerInputActions)
    {
        // Set the player input actions
        this.playerInputActions = playerInputActions;
    }
}
