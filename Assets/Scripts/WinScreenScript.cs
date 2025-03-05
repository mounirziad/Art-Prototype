using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{

    public Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(mainMenu);
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

}