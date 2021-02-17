using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Min(0f)]
    [SerializeField]
    private float moveSpeed = 8f;

    [Min(0f)]
    [SerializeField]
    private float jumpForce = 3f;

    [Min(0f)]
    [SerializeField]
    private float lookSensitivity = 5f;

    [Min(0f)]
    [SerializeField]
    private float maxYangle = 80f;

    [Min(0f)]
    [SerializeField]
    private float maxZangle = 30f;

    private GameObject camera;

    private Rigidbody rb;

    private bool canJump;
    private bool isGrounded = true;
    private Vector3 movementAxis;
    private Vector3 rotationAxis;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camera = gameObject.transform.Find("PlayerCamera").gameObject;
    }

    private void Update()
    {
        UpdateJump();
        UpdateMovementAxis();
        UpdateRotationAxis();
    }

    private void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdateJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            canJump = true;
        }
    }

    private void UpdateMovementAxis()
    {
        movementAxis.x = Input.GetAxis("Horizontal");
        movementAxis.z = Input.GetAxis("Vertical");
    }

    private void UpdateRotationAxis()
    {
        rotationAxis.x += Input.GetAxis("Mouse X") * lookSensitivity;
        rotationAxis.y -= Input.GetAxis("Mouse Y") * lookSensitivity;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            rotationAxis.z -= Input.GetAxis("Tilt") * lookSensitivity;
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

        if(canJump)
        {
            canJump = false;
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
}
