using UnityEngine;
using System.Collections;

public class ControlLight : MonoBehaviour
{
    private Light directionalLight;
    [SerializeField] private GameObject BaseMusic;
    private bool hasReducedLight = false; // Ensure it runs only once

    void Start()
    {
        directionalLight = GetComponent<Light>();
    }

    void Update()
    {
        if (BaseMusic == null && !hasReducedLight)
        {
            StartCoroutine(ChangeLight());
            hasReducedLight = true; // Prevent multiple calls
        }
    }

    private IEnumerator ChangeLight()
    {
        yield return new WaitForSeconds(2f);

        if (directionalLight != null)
        {
            directionalLight.intensity = Mathf.Max(0, directionalLight.intensity - 2.5f); // Prevents negative intensity
        }
    }
}
