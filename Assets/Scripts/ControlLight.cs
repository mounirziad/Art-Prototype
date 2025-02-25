using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLight : MonoBehaviour
{

    private Light directionalLight;
    [SerializeField] private GameObject BaseMusic;
    // Start is called before the first frame update
    void Start()
    {
        directionalLight = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
        if(BaseMusic == null)
        {
            directionalLight.intensity -= 0.5f;
        }
    }
}
