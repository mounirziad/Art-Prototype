using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayButton : MonoBehaviour
{
    public Button playButton;
    public Button creditsButton;
    public Button mainMenuButton;
    public Button quitButton;

    public GameObject MainMenu;
    public GameObject Credits;

    private int selectedIndex = 0;
    private Button[] buttons;

    // Reference to the Input Action for Move
    public InputAction moveAction;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(play);
        creditsButton.onClick.AddListener(credits);
        mainMenuButton.onClick.AddListener(mainMenu);
        quitButton.onClick.AddListener(exit);

        buttons = new Button[] { playButton, creditsButton, mainMenuButton, quitButton };
        buttons[selectedIndex].Select();

        // Enable the Move action
        moveAction.Enable();
        moveAction.performed += OnMovePerformed;
    }

    private void OnDestroy()
    {
        // Disable the Move action when the object is destroyed
        moveAction.performed -= OnMovePerformed;
        moveAction.Disable();
    }

    // Handle the Move action
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        if (moveInput.y > 0.5f) // Up
        {
            Navigate(-1);
        }
        else if (moveInput.y < -0.5f) // Down
        {
            Navigate(1);
        }
    }

    private void Navigate(int direction)
    {
        selectedIndex = Mathf.Clamp(selectedIndex + direction, 0, buttons.Length - 1);
        buttons[selectedIndex].Select();
    }

    // Function that loads the first level
    void play()
    {
        SceneManager.LoadScene("Art Prototype");
    }

    // Function that loads the credit scene
    void credits()
    {
        MainMenu.gameObject.SetActive(false);
        Credits.gameObject.SetActive(true);
    }

    // Function that loads the menu scene
    void mainMenu()
    {
        MainMenu.gameObject.SetActive(true);
        Credits.gameObject.SetActive(false);
    }

    void exit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}