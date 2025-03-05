using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickeringLight;
    public float minIntensity = 1f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 1f;

    private float time;

    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime * (1 / flickerSpeed);
        flickeringLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(time, 1));
    }
}