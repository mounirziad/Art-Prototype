using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class DoorB : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    private bool hasTriggered = false;
    private bool playerInTrigger = false;

    [SerializeField] private AudioSource doorAudio = null;
    [SerializeField] private AudioClip doorSound;

    [SerializeField] private GameObject uiPromptImage; // Reference to UI Image GameObject

    private void Start()
    {
        if (uiPromptImage != null)
            uiPromptImage.SetActive(false); // Ensure UI prompt is hidden initially
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
        if (playerInTrigger && !hasTriggered && Input.GetKeyDown(KeyCode.E))
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
        yield return new WaitForSeconds(4f);
        myDoor.Play("DoorOpen", 0, 0.0f);

        yield return new WaitForSeconds(4f);
        myDoor.Play("DoorClose", 0, 0.0f);

        gameObject.SetActive(false);
    }
}
