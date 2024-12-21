using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;

/// <summary>
/// Manages the core game logic, including game state transitions, life management, and wormhole effects.
/// Notifies observers about game state changes.
/// </summary>
public class GameManager : MonoBehaviour, ILifeManager
{
    public CameraManager cameraManager; // Reference to the CameraManager
    public PlayerManager playerManager; // Reference to the PlayerManager
    public UIManager uIManager; // Reference to the UIManager
    public GravityManager gravityManager; // Reference to the GravityManager
    public Vector3 wormholeTargetPos; // Target position after wormhole transition
    public GameState gameState; // Current game state
    Subject<GameStateObserver, GameState> gameStateChange; // Subject for notifying GameStateObservers

    private int life; // Current life of the player
    private int beginningLife; // Initial life at the start of a scene

    void Start()
    {
        gameState = GameState.Playing; // Set initial game state

        // Initialize subject for game state observers
        gameStateChange = new Subject<GameStateObserver, GameState>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        uIManager = FindObjectOfType<UIManager>();
        gravityManager = FindObjectOfType<GravityManager>();

        // Add observers to track game state changes
        gameStateChange.AddObserver(cameraManager);
        gameStateChange.AddObserver(playerManager);
        gameStateChange.AddObserver(uIManager);
        gameStateChange.AddObserver(gravityManager);
        gameStateChange.NotifyObservers(gameState); // Notify initial state

        InitializeLife(); // Initialize player's life based on the scene
    }

    /// <summary>
    /// Called by WormholeManager when the player is close to the wormhole.
    /// Triggers the wormhole animation and sets the state to WormholeEffect.
    /// </summary>
    /// <param name="wormhole">Transform of the wormhole object.</param>
    /// <param name="targetPos">Position to move the player after the animation.</param>
    public void startWormhole(Transform wormhole, Vector3 targetPos)
    {
        wormholeTargetPos = targetPos; // Store the target position
        cameraManager.SetWormhole(wormhole); // Notify CameraManager
        gameState = GameState.WormholeEffect; // Update game state
        gameStateChange.NotifyObservers(gameState); // Notify observers
    }

    /// <summary>
    /// Ends the wormhole effect and teleports the player to the target position.
    /// </summary>
    public void exitWormhole()
    {
        playerManager.Teleport(wormholeTargetPos); // Teleport player
        gameState = GameState.Playing; // Resume playing state
        gameStateChange.NotifyObservers(gameState); // Notify observers
    }

    /// <summary>
    /// Saves the player's life value to PlayerPrefs.
    /// </summary>
    /// <param name="life">Life value to save.</param>
    private void SaveLifeInfo(int life)
    {
        PlayerPrefs.SetInt("Life", life); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Ensure persistence
    }

    /// <summary>
    /// Resets the player's life to its initial value.
    /// </summary>
    public void ResetLife()
    {
        SaveLifeInfo(beginningLife); // Restore initial life
    }

    /// <summary>
    /// Initializes the player's life when a new scene is loaded.
    /// </summary>
    /// <remarks>
    /// Life is fully restored only at the start of specific scenes, such as tutorials or new stages.
    /// </remarks>
    private void InitializeLife()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Fully charge life for specific scenes
        if ((currentSceneIndex == 2) || (currentSceneIndex == 3) || (currentSceneIndex == 6))
        {
            life = 5;
            SaveLifeInfo(5);
        }
        else
        {
            life = PlayerPrefs.GetInt("Life"); // Load saved life
        }

        beginningLife = life; // Store starting life
    }

    /// <summary>
    /// Gets the current life of the player.
    /// </summary>
    public int Life
    {
        get { return life; }
    }

    /// <summary>
    /// Modifies the player's life and updates the game state accordingly.
    /// </summary>
    /// <param name="amount">Amount to modify the life by (positive to increase, negative to decrease).</param>
    public void ModifyLife(int amount)
    {
        if (amount < 0) // Decrease life
        {
            if (gameState == GameState.Playing)
            {
                life += amount;
                SaveLifeInfo(life);

                if (life <= 0)
                {
                    life = 0;
                    gameState = GameState.Gameover; // Game over if life reaches zero
                    gameStateChange.NotifyObservers(gameState);
                }
                else
                {
                    StartCoroutine(HarmCoroutine()); // Handle harm state transition
                }
            }
        }
        else // Increase life
        {
            switch (gameState)
            {
                case GameState.Playing:
                case GameState.Revived:
                    life += amount;
                    SaveLifeInfo(life);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Handles the player's state transitions after taking damage.
    /// </summary>
    /// <returns>A coroutine for timed transitions.</returns>
    private IEnumerator HarmCoroutine()
    {
        gameState = GameState.Stun; // Set state to Stun
        gameStateChange.NotifyObservers(gameState);
        yield return new WaitForSeconds(2f); // Wait 2 seconds

        gameState = GameState.Revived; // Set state to Revived
        gameStateChange.NotifyObservers(gameState);
        yield return new WaitForSeconds(3f); // Wait 3 seconds

        gameState = GameState.Playing; // Resume playing state
        gameStateChange.NotifyObservers(gameState);
    }

    /// <summary>
    /// Retrieves the current game state.
    /// </summary>
    /// <remarks>
    /// Used when adding a GameStateObserver is not efficient for performance reasons.
    /// </remarks>
    public GameState GetGameState()
    {
        return gameState;
    }

    /// <summary>
    /// Pauses the game by setting the game state to Paused.
    /// </summary>
    public void Pause()
    {
        gameState = GameState.Paused;
        gameStateChange.NotifyObservers(gameState);
    }

    /// <summary>
    /// Resumes the game by setting the game state to Playing.
    /// </summary>
    public void Resume()
    {
        gameState = GameState.Playing;
        gameStateChange.NotifyObservers(gameState);
    }
}