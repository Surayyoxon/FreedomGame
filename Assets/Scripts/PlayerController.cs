using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private float groundOffset = 0.05f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -30f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        Move();
        ApplyGravity();
        StickToGround();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(x, 0f, z);

        if (input.sqrMagnitude > 1f)
            input.Normalize();

        if (cameraTransform == null)
            return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement =
            forward * z +
            right * x;

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        controller.Move(
            movement * moveSpeed * Time.deltaTime
        );

        // Harakat yo'nalishiga qarab rotate
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        controller.Move(
            Vector3.up * verticalVelocity * Time.deltaTime
        );
    }

    private void StickToGround()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(
            rayStart,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundLayer))
        {
            float targetY =
                hit.point.y + groundOffset;

            float difference =
                targetY - controller.transform.position.y;

            if (Mathf.Abs(difference) > 0.01f)
            {
                controller.Move(
                    Vector3.up * difference
                );
            }
        }
    }
}