using System;
using UnityEngine;
using System.Collections;

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
    public float movementSpeed = 7f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    // Private variables
    private bool isWalking;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;

    [SerializeField]
    private GameObject _playerVisual;
    [SerializeField]
    private GameObject _playerSlip;

    private bool _canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _canMove = true;
    }

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

        if (_canMove)
        {
            HandleMovement();
            _playerSlip.gameObject.SetActive(false);
        }
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

        rb.velocity = moveDirection * movementSpeed * Time.deltaTime;

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Slip"))
        {
            _playerVisual.gameObject.SetActive(false);
            _playerSlip.gameObject.SetActive(true);
            Quaternion currentRotation;
            currentRotation = _playerVisual.transform.rotation;
            rb.velocity = Vector3.zero;
            _canMove = false;
            StartCoroutine(SlipPlayer(5f, currentRotation));
        }
    }

    IEnumerator SlipPlayer(float time, Quaternion rotationNormal)
    {
        yield return new WaitForSeconds(time);
        _canMove = true;
        _playerVisual.gameObject.SetActive(true);
    }
    

}
