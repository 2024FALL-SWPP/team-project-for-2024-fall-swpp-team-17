using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OurGame;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the game's user interface elements, including health bar updates, pause menu, game over screen, 
/// hint messages, and gravity information display. It observes game state changes to update the UI accordingly.
/// </summary>
public class UIManager : MonoBehaviour, GameStateObserver
{
    public Slider healthBar; // Health bar slider to display player health
    private GameManager gameManager; // Reference to the GameManager
    private GameState gameState; // Tracks the current game state
    public TextMeshProUGUI gameOverText; // Text element for the game over message
    public Button pauseButton; // Pause button reference
    public TextMeshProUGUI hintMessageText; // Text element for hint messages
    private Coroutine typingCoroutine; // Coroutine for typing effect in hint messages
    public GameObject menu; // Pause menu UI object
    public GameObject hintMessagebox; // Hint message background UI object
    public Button restartButton; // Restart button in the game over menu
    public Button mainMenuButton; // Main menu button in the game over menu
    public LoadScene loadScene; // LoadScene component for scene transitions
    public GameObject gravityDirectionMessage; // UI element for displaying gravity directions (1, 2, 3)

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find GameManager in the scene
        healthBar.value = gameManager.Life; // Initialize health bar value
        gameOverText.gameObject.SetActive(false); // Hide game over text initially
        restartButton.gameObject.SetActive(false); // Hide restart button initially
        mainMenuButton.gameObject.SetActive(false); // Hide main menu button initially
        pauseButton.gameObject.SetActive(true); // Show pause button
        menu.SetActive(false); // Hide the pause menu initially

        if (hintMessageText != null)
        {
            hintMessageText.gameObject.SetActive(false); // Hide hint message text initially
        }
        hintMessagebox.gameObject.SetActive(false); // Hide hint message box initially

        if (gravityDirectionMessage != null)
        {
            gravityDirectionMessage.SetActive(false); // Hide gravity direction message initially
        }
    }

    void Update()
    {
        // Smoothly update the health bar if the life value changes
        if (healthBar.value != gameManager.Life)
        {
            StartCoroutine(SmoothHealthBarUpdate(gameManager.Life));
        }

        // Pause the game when the P key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameState != GameState.Paused)
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// Smoothly updates the health bar to the target value.
    /// </summary>
    /// <param name="targetValue">Target value to update the health bar to.</param>
    private IEnumerator SmoothHealthBarUpdate(float targetValue)
    {
        float currentValue = healthBar.value;
        while (Mathf.Abs(currentValue - targetValue) > 0.01f)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 2f);
            healthBar.value = currentValue;
            yield return null;
        }
        healthBar.value = targetValue;
    }

    /// <summary>
    /// Handles UI updates based on the game state.
    /// </summary>
    /// <param name="gs">The updated game state.</param>
    public void OnNotify<GameStateObserver>(GameState gs)
    {
        gameState = gs;
        switch (gs)
        {
            case GameState.Gameover:
                gameOverText.gameObject.SetActive(true); // Show game over text
                restartButton.gameObject.SetActive(true); // Show restart button
                mainMenuButton.gameObject.SetActive(true); // Show main menu button
                pauseButton.gameObject.SetActive(false); // Hide pause button
                HideMessage(); // Hide hint messages
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Displays a hint message with a typing effect.
    /// </summary>
    /// <param name="message">The message to display.</param>
    public void ShowMessage(string message)
    {
        if (hintMessageText == null) return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeMessage(message));
    }

    /// <summary>
    /// Hides the hint message.
    /// </summary>
    public void HideMessage()
    {
        if (hintMessageText != null)
        {
            hintMessageText.gameObject.SetActive(false);
            hintMessagebox.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Displays a message character by character with a typing effect.
    /// </summary>
    private IEnumerator TypeMessage(string message)
    {
        hintMessagebox.gameObject.SetActive(true);
        hintMessageText.text = "";
        hintMessageText.gameObject.SetActive(true);

        foreach (char letter in message.ToCharArray())
        {
            hintMessageText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    /// <summary>
    /// Shows gravity directions (1, 2, 3) on the screen.
    /// </summary>
    public void ShowGravityDirections()
    {
        if (gravityDirectionMessage != null)
        {
            gravityDirectionMessage.SetActive(true);
        }
    }

    /// <summary>
    /// Hides gravity directions from the screen.
    /// </summary>
    public void HideGravityDirections()
    {
        if (gravityDirectionMessage != null)
        {
            gravityDirectionMessage.SetActive(false);
        }
    }

    /// <summary>
    /// Pauses the game and shows the pause menu.
    /// </summary>
    public void Pause()
    {
        gameManager.Pause(); // Notify GameManager to change the game state
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0; // Pause the game
            menu.SetActive(true); // Show the pause menu
        }
        EventSystem.current.SetSelectedGameObject(null); // Clear button selection
    }

    /// <summary>
    /// Resumes the game and hides the pause menu.
    /// </summary>
    public void Resume()
    {
        gameManager.Resume();
        Time.timeScale = 1; // Resume the game
        menu.SetActive(false); // Hide the pause menu
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1; // Resume time if paused
        gameManager.ResetLife(); // Reset player life
        loadScene.ReloadScene(); // Reload the current scene
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadMenu()
    {
        Time.timeScale = 1; // Resume time if paused
        loadScene.LoadTitle(); // Load the title menu scene
    }
}