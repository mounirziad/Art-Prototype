using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Add this namespace

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
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePerformed;
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

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Unlock cursor to allow UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void Back()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
}