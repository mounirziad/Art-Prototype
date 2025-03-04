using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverRestart : MonoBehaviour
{

    public Button mainMenuButton;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(mainMenu);
        restartButton.onClick.AddListener(restartLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void mainMenu()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene("Main Menu");
    }

    void restartLevel()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene("Art Prototype");
    }
}
