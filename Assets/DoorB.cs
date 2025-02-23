using System.Collections;
using UnityEngine;

public class DoorB : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    private bool hasTriggered = false; // Prevent multiple activations
    private bool playerInTrigger = false; // Track if player is in the trigger
    [SerializeField] private AudioSource doorAudio = null; // Reference to AudioSource
    [SerializeField] private AudioClip doorSound; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true; // Mark player inside trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false; // Mark player outside trigger
        }
    }

    private void Update()
    {
        if (playerInTrigger && !hasTriggered && Input.GetKeyDown(KeyCode.E))
        {
            hasTriggered = true;
            StartCoroutine(OpenAndCloseDoor());
        }
    }

    private IEnumerator OpenAndCloseDoor()
    {
        doorAudio.PlayOneShot(doorSound);
        yield return new WaitForSeconds(4f); // Wait before opening
        myDoor.Play("DoorOpen", 0, 0.0f);

        yield return new WaitForSeconds(4f); // Wait before closing
        myDoor.Play("DoorClose", 0, 0.0f);

        gameObject.SetActive(false); // Disable trigger after activation (optional)
    }
}
