using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Button playButton;
    public Button creditsButton;
    public Button mainMenuButton;

    public GameObject MainMenu;
    public GameObject Credits;


    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(play);
        creditsButton.onClick.AddListener(credits);
        mainMenuButton.onClick.AddListener(mainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function that loads the first level
    void play()
    {
        SceneManager.LoadScene("Art Prototype");
    }

    //Function that loads the credit scene
    void credits()
    {
        MainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(true);
    }
    
    //Function that loads the menu scene
    void mainMenu()
    {
        MainMenu.gameObject.SetActive(true);
        Credits.gameObject.SetActive(false);
    }

}
