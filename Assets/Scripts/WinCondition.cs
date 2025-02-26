using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private bool inrangeofbox = false;
    public Light directionalLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("fusebox"))
        {
            inrangeofbox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("fusebox"))
        {
            inrangeofbox = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inrangeofbox && Input.GetKeyDown(KeyCode.E))
        {
            directionalLight.intensity += 0.5f;
        }
    }
}
