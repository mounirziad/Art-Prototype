using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    [SerializeField] private bool OpenTrigger = false;
    [SerializeField] private bool CloseTrigger = false;

    private bool playerInsideTrigger = false;

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
        if (playerInsideTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (OpenTrigger)
            {
                myDoor.Play("DoorAOpen", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if (CloseTrigger)
            {
                myDoor.Play("DoorBClose", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}