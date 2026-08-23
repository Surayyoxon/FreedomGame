using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -30f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Control")]
    [SerializeField] private bool canMove = false;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null &&
            Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (canMove)
        {
            Move();
        }

        ApplyGravity();
    }

    // =========================================
    // MOVEMENT ENABLE / DISABLE
    // =========================================

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
    }

    // =========================================
    // MOVE
    // =========================================

    private void Move()
    {
        float x =
            Input.GetAxisRaw("Horizontal");

        float z =
            Input.GetAxisRaw("Vertical");

        Vector3 input =
            new Vector3(x, 0f, z);

        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        if (cameraTransform == null)
        {
            Debug.LogWarning(
                "PlayerController: Camera Transform topilmadi!"
            );

            return;
        }

        // Camera yo'nalishi
        Vector3 forward =
            cameraTransform.forward;

        Vector3 right =
            cameraTransform.right;

        // Y harakatni butunlay olib tashlaymiz
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Camera-relative movement
        Vector3 movement =
            forward * z +
            right * x;

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        // Playerni yuritamiz
        controller.Move(
            movement *
            moveSpeed *
            Time.deltaTime
        );

        // Harakat yo'nalishiga qarash
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    movement
                );

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed *
                    Time.deltaTime
                );
        }
    }

    // =========================================
    // GRAVITY
    // =========================================

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity +=
                gravity *
                Time.deltaTime;
        }

        controller.Move(
            Vector3.up *
            verticalVelocity *
            Time.deltaTime
        );
    }
}