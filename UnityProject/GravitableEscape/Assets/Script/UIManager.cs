using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OurGame;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, GameStateObserver
{
    public Slider healthBar;
    private GameManager gameManager;
    private GameState gameState;
    public TextMeshProUGUI gameOverText;
    public Button pauseButton;
    public TextMeshProUGUI hintMessageText;
    private Coroutine typingCoroutine;
    public GameObject menu;
    public GameObject hintMessagebox;
    public Button restartButton;
    public Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        healthBar.value = gameManager.Life;
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        menu.SetActive(false);

        if (hintMessageText != null)
        {
            hintMessageText.gameObject.SetActive(false);
        }
        hintMessagebox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.value != gameManager.Life)
        {
            StartCoroutine(SmoothHealthBarUpdate(gameManager.Life));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameState != GameState.Paused)
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// smooth health bar update, not concrete
    /// </summary>
    /// <param name="targetValue"></param>
    /// <returns></returns>
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
    /// gameovertext SetActive to true when Gameover
    /// </summary>
    /// <typeparam name="GameStateObserver"></typeparam>
    /// <param name="gs">global game state after modification</param>
    public void OnNotify<GameStateObserver>(GameState gs)
    {
        gameState = gs;
        switch (gs)
        {
            case GameState.Gameover:
                gameOverText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                mainMenuButton.gameObject.SetActive(true);
                pauseButton.gameObject.SetActive(false);
                break;
            default:

                break;
        }
    }


    /// <summary>
    /// Displays hint message with a typing effect, based on player position.
    /// </summary>
    /// <param name="message">message for a specific zone</param>
    public void ShowMessage(string message)
    {
        if (hintMessageText == null)
        {
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeMessage(message));
    }

    /// <summary>
    /// Hides hint message based on player position.
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
    /// Triggers typing effect.
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
    /// do Pause
    /// </summary>
    public void Pause()
    {
        gameManager.Pause(); // change gameState in gameManager, gameState of UIManager is changed when OnNotify is called
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            menu.SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// do resume when pause
    /// </summary>
    public void Resume()
    {
        gameManager.Resume();
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    /// <summary>
    /// do restart when pause
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }


}

