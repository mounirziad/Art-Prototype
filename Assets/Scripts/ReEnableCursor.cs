using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReEnableCursor : MonoBehaviour
{
    void OnEnable()
    {
        // Enable and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnDisable()
    {
        // Optionally disable and lock the cursor when the screen is not active
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
