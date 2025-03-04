using UnityEngine;
using UnityEngine.InputSystem; // Required for Input System

public class TriggerFridgeDoor : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    [SerializeField] private bool OpenTrigger = false;
    [SerializeField] private bool CloseTrigger = false;

    private bool playerInsideTrigger = false;

    // Input System
    private Player playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        // Initialize the Input System
        playerInput = new Player();
        playerInput.Enable();

        // Get the Interact action
        interactAction = playerInput.PlayerControls.Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
        }
    }

    private void Update()
    {
        // Check if the Interact action was triggered (E on keyboard or X on controller)
        if (playerInsideTrigger && interactAction.triggered)
        {
            if (OpenTrigger)
            {
                myDoor.Play("FridgeOpen", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if (CloseTrigger)
            {
                myDoor.Play("FridgeClose", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // Disable the Input System when the object is destroyed
        playerInput.Disable();
    }
}