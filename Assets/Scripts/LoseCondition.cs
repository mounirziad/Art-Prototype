using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCondition : MonoBehaviour
{
    private bool hascollided = false;

    public void RestartScene()
    {
        Debug.Log("Restarting Scene: " + SceneManager.GetActiveScene().name); // Debugging
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other) // Change to Collider
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name); // Debugging
        if (other.gameObject.name == "Agent") // Use 'other' instead of 'collision'
        {
            hascollided = true;
        }
    }

    private void Update()
    {
        if (hascollided)
        {
            RestartScene();
        }
    }
}
