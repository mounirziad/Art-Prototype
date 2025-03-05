using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public Button quitButton;
    public Button resumeButton;
    public Button settingsButton;
    public Button backButton;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    private bool isPaused = false;

    public InputActionReference pauseAction; // Reference to the Pause action
    public InputActionReference navigateAction; // Reference to the Navigate action
    public InputActionReference submitAction; // Reference to the Submit action
    public InputActionReference cancelAction; // Reference to the Cancel action

    private Button[] buttons; // Array to store buttons for navigation
    private int selectedButtonIndex = 0; // Index of the currently selected button

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(Settings);
        backButton.onClick.AddListener(Back);
        quitButton.onClick.AddListener(QuitToMainMenu);
        ResumeGame(); // Ensure the game starts unpaused

        // Enable the Pause action and subscribe to its performed event
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPausePerformed;
        }
        else
        {
            Debug.LogError("Pause action is not assigned!");
        }

        // Enable UI navigation actions
        if (navigateAction != null)
        {
            navigateAction.action.Enable();
            navigateAction.action.performed += OnNavigatePerformed;
        }
        if (submitAction != null)
        {
            submitAction.action.Enable();
            submitAction.action.performed += OnSubmitPerformed;
        }
        if (cancelAction != null)
        {
            cancelAction.action.Enable();
            cancelAction.action.performed += OnCancelPerformed;
        }

        // Initialize button navigation
        buttons = new Button[] { resumeButton, settingsButton, quitButton }; // Add buttons in order
        SelectButton(selectedButtonIndex); // Select the first button by default
    }

    void OnDestroy()
    {
        // Unsubscribe from events to avoid memory leaks
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePerformed;
        }
        if (navigateAction != null)
        {
            navigateAction.action.performed -= OnNavigatePerformed;
        }
        if (submitAction != null)
        {
            submitAction.action.performed -= OnSubmitPerformed;
        }
        if (cancelAction != null)
        {
            cancelAction.action.performed -= OnCancelPerformed;
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // Toggle pause when the action is performed
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void OnNavigatePerformed(InputAction.CallbackContext context)
    {
        // Get the input value (e.g., Vector2 for stick/d-pad)
        Vector2 input = context.ReadValue<Vector2>();

        // Navigate between buttons based on input
        if (input.y > 0.5f) // Up
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttons.Length) % buttons.Length;
        }
        else if (input.y < -0.5f) // Down
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttons.Length;
        }

        SelectButton(selectedButtonIndex); // Update the selected button
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        // Simulate a click on the currently selected button
        buttons[selectedButtonIndex].onClick.Invoke();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        // Go back to the previous menu or resume the game
        if (settingsMenuUI.activeSelf)
        {
            Back();
        }
        else if (pauseMenuUI.activeSelf)
        {
            ResumeGame();
        }
    }

    private void SelectButton(int index)
    {
        // Deselect all buttons
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = Color.white; // Reset button color
        }

        // Select the current button
        buttons[index].Select();
        buttons[index].GetComponent<Image>().color = Color.yellow; // Highlight the selected button
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Unlock cursor to allow UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Reset button selection when pausing
        selectedButtonIndex = 0;
        SelectButton(selectedButtonIndex);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Lock cursor back to the center when resuming the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time resumes before loading
        SceneManager.LoadScene("Main Menu");
    }

    public void Settings()
    {
        Debug.Log("Opening Settings Menu");
        settingsMenuUI.SetActive(true);
        Debug.Log("Settings Menu Active: " + settingsMenuUI.activeSelf);

        Debug.Log("Closing Pause Menu");
        pauseMenuUI.SetActive(false);
        Debug.Log("Pause Menu Active: " + pauseMenuUI.activeSelf);

        // Update button navigation for the settings menu
        buttons = new Button[] { backButton }; // Only the back button in settings
        Debug.Log("Buttons in Settings Menu: " + buttons.Length);

        selectedButtonIndex = 0;
        SelectButton(selectedButtonIndex);
    }

    public void Back()
    {
        // Activate the pause menu first
        pauseMenuUI.SetActive(true);

        // Then deactivate the settings menu
        settingsMenuUI.SetActive(false);

        // Update button navigation for the pause menu
        buttons = new Button[] { resumeButton, settingsButton, quitButton };
        selectedButtonIndex = 0;
        SelectButton(selectedButtonIndex);
    }
}