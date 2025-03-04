using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 0.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    public Camera playerCamera; // Reference to the camera

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Set up the Input System
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        // Ensure the camera is assigned
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep the player grounded
        }

        // Get input for movement using the Input System
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Calculate movement direction relative to the camera
        Vector3 move = playerCamera.transform.right * moveInput.x + playerCamera.transform.forward * moveInput.y;
        move.y = 0; // Ensure the movement is horizontal

        // Normalize the movement vector to maintain consistent speed
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        controller.Move(move * speed * Time.deltaTime);

        // Jumping
        if (jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}