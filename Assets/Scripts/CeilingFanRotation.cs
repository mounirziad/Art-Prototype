using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingFanRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation
    private bool isRotating = true; // Controls whether the fan is rotating

    private void Update()
    {
        if (isRotating)
        {
            // Rotate the fan around its Y-axis
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    // Method to start rotation
    public void StartRotation()
    {
        isRotating = true;
    }

    // Method to stop rotation
    public void StopRotation()
    {
        isRotating = false;
    }
}