using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputActionReference moveInputActionReference;

    [SerializeField]
    private InputActionReference lookInputActionReference;

    [SerializeField]
    private InputActionReference jumpInputActionReference;

    [SerializeField]
    private InputActionReference tiltInputActionReference;

    [Header("Variables")]
    [Min(0f)]
    [SerializeField]
    private float moveSpeed = 8f;

    [Min(0f)]
    [SerializeField]
    private float jumpForce = 3f;

    [Min(0f)]
    [SerializeField]
    private float lookSensitivity = 0.3f;

    [Min(0f)]
    [SerializeField]
    private float maxYangle = 80f;

    [Min(0f)]
    [SerializeField]
    private float maxZangle = 30f;

    #region InputActions
    private InputAction MoveInputAction
    {
        get
        {
            var action = moveInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }

    private InputAction LookInputAction
    {
        get
        {
            var action = lookInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }

    private InputAction JumpInputAction
    {
        get
        {
            var action = jumpInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }

    private InputAction TiltInputAction
    {
        get
        {
            var action = tiltInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }
    #endregion

    private GameObject camera;

    private Rigidbody rb;

    private bool canJump;
    private bool isGrounded = true;

    private Vector2 moveInputAxis;
    private Vector3 movementAxis;

    private Vector2 lookInputAxis;
    [SerializeField]
    private float tiltInput;
    [SerializeField]
    private Vector3 rotationAxis;

    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camera = gameObject.transform.Find("PlayerCamera").gameObject;
        anim = gameObject.transform.Find("Stephen").GetComponent<Animator>();
    }

    private void OnEnable()
    {
        MoveInputAction.performed += OnMovePerformed;
        MoveInputAction.canceled += OnMoveCanceled;
        
        LookInputAction.performed += OnLookPerformed;
        LookInputAction.canceled += OnLookCanceled;

        JumpInputAction.performed += OnJumpPerformed;
        JumpInputAction.canceled += OnJumpCanceled;

        TiltInputAction.performed += OnTiltPerformed;
        TiltInputAction.canceled += OnTiltCanceled;
    }

    private void OnDisable()
    {
        MoveInputAction.performed -= OnMovePerformed;
        MoveInputAction.canceled -= OnMoveCanceled;

        LookInputAction.performed -= OnLookPerformed;
        LookInputAction.canceled -= OnLookCanceled;

        JumpInputAction.performed -= OnJumpPerformed;
        JumpInputAction.canceled -= OnJumpCanceled;

        TiltInputAction.performed -= OnTiltPerformed;
        TiltInputAction.canceled -= OnTiltCanceled;
    }

    #region InputEvents
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInputAxis = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInputAxis = Vector2.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        lookInputAxis = ctx.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        lookInputAxis = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        canJump = ctx.ReadValueAsButton();
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        canJump = false;
    }

    private void OnTiltPerformed(InputAction.CallbackContext ctx)
    {
        tiltInput = ctx.ReadValue<float>();
    }

    private void OnTiltCanceled(InputAction.CallbackContext ctx)
    {
        tiltInput = 0;
    }
    #endregion

    private void Update()
    {
        UpdateMovementAxis();
        UpdateRotationAxis();
    }

    private void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
        UpdateAnimations();
    }

    private void UpdateMovementAxis()
    {
        movementAxis = new Vector3(moveInputAxis.x, 0, moveInputAxis.y);
    }

    private void UpdateRotationAxis()
    {
        rotationAxis.x += lookInputAxis.x * lookSensitivity;
        rotationAxis.y -= lookInputAxis.y * lookSensitivity;

        if (tiltInput != 0)
        {
            rotationAxis.z -= tiltInput * lookSensitivity;
        }
        else
        {
            rotationAxis.z = 0;
        }
    }

    private void UpdatePosition()
    {
        var posMovement = movementAxis * moveSpeed * Time.deltaTime;
        transform.position += transform.TransformDirection(posMovement);

        if(canJump && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }

    private void UpdateRotation()
    {
        // Player Rotation Part

        var playerTransform = gameObject.transform;

        rotationAxis.x = Mathf.Repeat(rotationAxis.x, 360);

        playerTransform.rotation = Quaternion.Euler(0, rotationAxis.x, 0);

        // Camera Rotation Part

        var cameraTransform = camera.transform;

        rotationAxis.y = Mathf.Clamp(rotationAxis.y, -maxYangle, maxYangle);
        rotationAxis.z = Mathf.Clamp(rotationAxis.z, -maxZangle, maxZangle);

        cameraTransform.rotation = Quaternion.Euler(rotationAxis.y, rotationAxis.x, rotationAxis.z);
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("DirectionX", movementAxis.x);
        anim.SetFloat("DirectionZ", movementAxis.z);
        anim.SetFloat("Speed", movementAxis.magnitude);
    }
}
