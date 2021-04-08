using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputActionReference moveInputActionReference;

    [SerializeField]
    private InputActionReference runInputActionReference;

    [SerializeField]
    private InputActionReference lookInputActionReference;

    [SerializeField]
    private InputActionReference jumpInputActionReference;

    [SerializeField]
    private InputActionReference tiltInputActionReference;

    [SerializeField]
    private InputActionReference flashlightInputActionReference;

    [SerializeField]
    private InputActionReference actionInputActionReference;

    [Header("Variables")]
    [Min(0f)]
    [SerializeField]
    private float moveSpeed = 4f;

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
    private float maxZangle = 15f;

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

    private InputAction FlashlightInputAction
    {
        get
        {
            var action = flashlightInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }

    private InputAction RunInputAction
    {
        get
        {
            var action = runInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }

    private InputAction ActionInputAction
    {
        get
        {
            var action = actionInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }
    #endregion

    private new GameObject camera;

    private Rigidbody rb;

    private float walkSpeed = 4f;
    private float runSpeed = 8f;

    private bool canJump;
    private bool isGrounded = true;

    private Vector2 moveInputAxis;
    private Vector3 movementAxis;

    private Vector2 lookInputAxis;
    private float tiltInput;
    private Vector3 rotationAxis;

    private bool flashlightEnabled = false;

    private Animator anim;
    private GameObject flashlight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camera = gameObject.transform.Find("PlayerCamera").gameObject;
        anim = gameObject.transform.Find("Stephen").GetComponent<Animator>();
        flashlight = gameObject.transform.Find("Flashlight").gameObject;
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

        FlashlightInputAction.performed += OnFlashlightPerformed;

        RunInputAction.performed += OnRunPerformed;
        RunInputAction.canceled += OnRunCanceled;

        ActionInputAction.performed += OnActionPerformed;

        
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

        FlashlightInputAction.performed -= OnFlashlightPerformed;

        RunInputAction.performed -= OnRunPerformed;
        RunInputAction.canceled -= OnRunCanceled;

        ActionInputAction.performed -= OnActionPerformed;

        
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

    private void OnFlashlightPerformed(InputAction.CallbackContext ctx)
    {
        flashlightEnabled = !flashlightEnabled;
    }

    private void OnRunPerformed(InputAction.CallbackContext ctx)
    {
        moveSpeed = runSpeed;
    }

    private void OnRunCanceled(InputAction.CallbackContext ctx)
    {
        moveSpeed = walkSpeed;
    }

    private void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        //send a ray
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

        UpdateFlashlight();

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

        if (tiltInput != 0)
        {
            playerTransform.rotation = Quaternion.Euler(Mathf.Abs(rotationAxis.z), rotationAxis.x + rotationAxis.z, rotationAxis.z);
        }
        else
        {
            playerTransform.rotation = Quaternion.Euler(0, rotationAxis.x, 0);
        }

        

        // Camera Rotation Part

        var cameraTransform = camera.transform;

        rotationAxis.y = Mathf.Clamp(rotationAxis.y, -maxYangle, maxYangle);
        rotationAxis.z = Mathf.Clamp(rotationAxis.z, -maxZangle, maxZangle);

        cameraTransform.rotation = Quaternion.Euler(rotationAxis.y, rotationAxis.x + rotationAxis.z, rotationAxis.z);

    }

    private void UpdateFlashlight()
    {
        flashlight.transform.rotation = camera.transform.rotation;

        flashlight.SetActive(flashlightEnabled);
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("Speed", movementAxis.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
