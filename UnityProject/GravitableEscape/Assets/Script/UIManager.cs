using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OurGame;

public class UIManager : MonoBehaviour, GameStateObserver
{
    public TextMeshProUGUI lifeText;
    public Slider healthBar;
    private GameManager gameManager;
    private GameState gameState;
    public TextMeshProUGUI gameOverText;
    public Button pauseButton;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        healthBar.value = gameManager.Life;
        gameOverText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // lifeText.text = $"Life: {playerManager.GetLife()}";
        healthBar.value = gameManager.Life;
    }

    /// <summary>
    /// change button color
    /// </summary>
    /// <param name="button"></param>
    /// <param name="color"></param>
    private void ChangeButtonTextColor(Button button, Color color)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.color = color;
    }

    public void ChangePauseColorRed()
    {
        ChangeButtonTextColor(pauseButton, Color.red);
    }

    public void ChangePauseColorBlack()
    {
        ChangeButtonTextColor(pauseButton, Color.black);
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
                break;
            default:

                break;
        }
    }

}

