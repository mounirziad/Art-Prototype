using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Required for Input System
using UnityEngine.UI; // Required for UI elements

public class DoorB : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    private bool hasTriggered = false;
    private bool playerInTrigger = false;

    [SerializeField] private AudioSource doorAudio = null;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioSource dadtalking = null;
    [SerializeField] private AudioClip dadSound;
    [SerializeField] private GameObject sneakmusic;
    [SerializeField] private GameObject ThunderStrike;
    [SerializeField] private GameObject BaseMusic;
    [SerializeField] private CeilingFanRotation ceilingFan;
    [SerializeField] private CeilingFanRotation ceilingFan2;
    [SerializeField] private CeilingFanRotation ceilingFan3;
    [SerializeField] private GameObject DoorBlockade;

    [SerializeField] private GameObject uiPromptImage; // Reference to UI Image GameObject

    // Add four public Light variables
    [SerializeField] private Light light1;
    [SerializeField] private Light light2;
    [SerializeField] private Light light3;
    [SerializeField] private Light light4;

    // Input System
    private Player playerInput;
    private InputAction interactAction;

    private void Start()
    {
        if (uiPromptImage != null)
            uiPromptImage.SetActive(false); // Ensure UI prompt is hidden initially

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
            playerInTrigger = true;
            if (uiPromptImage != null)
                uiPromptImage.SetActive(true); // Show UI Image Prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (uiPromptImage != null)
                uiPromptImage.SetActive(false); // Hide UI Image Prompt when player leaves
        }
    }

    private void Update()
    {
        // Check if the Interact action was triggered (E on keyboard or X on controller)
        if (playerInTrigger && !hasTriggered && interactAction.triggered)
        {
            hasTriggered = true;
            if (uiPromptImage != null)
                uiPromptImage.SetActive(false); // Hide UI Image Prompt after interaction

            StartCoroutine(OpenAndCloseDoor());
        }
    }

    private IEnumerator OpenAndCloseDoor()
    {
        doorAudio.PlayOneShot(doorSound);
        yield return new WaitForSeconds(2f);
        dadtalking.PlayOneShot(dadSound);
        yield return new WaitForSeconds(2f);
        myDoor.Play("DoorOpen", 0, 0.0f);
        yield return new WaitForSeconds(9f);
        myDoor.Play("DoorClose", 0, 0.0f);
        yield return new WaitForSeconds(1f);
        doorAudio.Stop();
        Destroy(BaseMusic);

        // Instantiate ThunderStrike
        Instantiate(ThunderStrike);

        // Adjust light intensities
        if (light1 != null) light1.intensity = 0f;
        if (light2 != null) light2.intensity = 0f;
        if (light3 != null) light3.intensity = 0f;
        if (light4 != null) light4.intensity = 0f;

        if (ceilingFan != null)
        {
            ceilingFan.StopRotation();
        }

        if (ceilingFan2 != null)
        {
            ceilingFan2.StopRotation();
        }

        if (ceilingFan3 != null)
        {
            ceilingFan3.StopRotation();
        }

        DoorBlockade.SetActive(false);

        yield return new WaitForSeconds(3f);

        // Instantiate sneakmusic
        Instantiate(sneakmusic);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Disable the Input System when the object is destroyed
        playerInput.Disable();
    }
}