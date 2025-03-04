using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    private Player playerInput;
    private Vector2 lookInput;

    public float mouseSensitivityX = 2.0f;
    public float mouseSensitivityY = 2.0f;
    public float controllerSensitivityX = 50.0f; // Higher sensitivity for controller
    public float controllerSensitivityY = 50.0f; // Higher sensitivity for controller

    public float upperLookLimit = 80f;
    public float lowerLookLimit = 80f;

    private float currentX = 0f;
    private float currentY = 0f;

    private void Awake()
    {
        // Initialize the input system
        playerInput = new Player();
        playerInput.Enable(); // Enable the input actions
    }

    private void Update()
    {
        // Read the look input from the mouse or controller
        lookInput = playerInput.PlayerControls.Look.ReadValue<Vector2>();

        // Determine if the input is from a mouse or controller
        bool isMouseInput = true; // Default to mouse input
        if (playerInput.PlayerControls.Look.activeControl != null)
        {
            isMouseInput = playerInput.PlayerControls.Look.activeControl.device is Mouse;
        }

        // Scale the input based on the device
        float sensitivityX = isMouseInput ? mouseSensitivityX : controllerSensitivityX;
        float sensitivityY = isMouseInput ? mouseSensitivityY : controllerSensitivityY;

        // Update the camera rotation based on input
        currentX += lookInput.x * sensitivityX * Time.deltaTime;
        currentY -= lookInput.y * sensitivityY * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, -upperLookLimit, lowerLookLimit);

        // Apply the rotations to the camera
        transform.localRotation = Quaternion.Euler(currentY, currentX, 0);
    }

    private void OnDestroy()
    {
        // Disable the input actions when the object is destroyed
        playerInput.Disable();
    }
}