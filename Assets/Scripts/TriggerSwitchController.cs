using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Required for Input System
using UnityEngine.SceneManagement;

public class TriggerSwitchController : MonoBehaviour
{
    [SerializeField] private Animator mySwitch = null;

    [SerializeField] private bool OnTrigger = false;
    [SerializeField] private bool OffTrigger = false;

    // Add four public Light variables
    [SerializeField] private Light light1;
    [SerializeField] private Light light2;
    [SerializeField] private Light light3;
    [SerializeField] private Light light4;

    // Add three ceiling fans
    [SerializeField] private CeilingFanRotation ceilingFan;
    [SerializeField] private CeilingFanRotation ceilingFan2;
    [SerializeField] private CeilingFanRotation ceilingFan3;

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
            StartCoroutine(OpenAndCloseSwitch());
        }
    }

    private IEnumerator OpenAndCloseSwitch()
    {
        if (OnTrigger)
        {
            mySwitch.Play("LightSwitchDown", 0, 0.0f);

            // Turn on lights
            if (light1 != null) light1.intensity = 1f;
            if (light2 != null) light2.intensity = 1f;
            if (light3 != null) light3.intensity = 1f;
            if (light4 != null) light4.intensity = 1f;

            // Start ceiling fans
            if (ceilingFan != null) ceilingFan.StartRotation();
            if (ceilingFan2 != null) ceilingFan2.StartRotation();
            if (ceilingFan3 != null) ceilingFan3.StartRotation();

            yield return new WaitForSeconds(2f);

            // Load the win screen
            SceneManager.LoadScene("Win Screen");

            gameObject.SetActive(false);
        }
        else if (OffTrigger)
        {
            mySwitch.Play("LightSwitchUp", 0, 0.0f);
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Disable the Input System when the object is destroyed
        playerInput.Disable();
    }
}