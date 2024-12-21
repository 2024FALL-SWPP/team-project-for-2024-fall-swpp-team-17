using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;

public class GameManager : MonoBehaviour, ILifeManager
{
    public CameraManager cameraManager;
    public PlayerManager playerManager;
    public UIManager uIManager;
    public GravityManager gravityManager;
    public Vector3 wormholeTargetPos;
    public GameState gameState;
    Subject<GameStateObserver, GameState> gameStateChange;

    private int life;
    private int beginningLife;

    void Start()
    {
        gameState = GameState.Playing;

        gameStateChange = new Subject<GameStateObserver, GameState>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        uIManager = FindObjectOfType<UIManager>();
        gravityManager = FindObjectOfType<GravityManager>();
        gameStateChange.AddObserver(cameraManager);
        gameStateChange.AddObserver(playerManager);
        gameStateChange.AddObserver(uIManager);
        gameStateChange.AddObserver(gravityManager);
        gameStateChange.NotifyObservers(gameState);

        InitializeLife();
    }

    /// <summary>
    /// This function is called by WormholeManager when player is close enough to the wormhole.
    /// It triggers animation of warping into the wormhole.
    /// </summary>
    /// <param name="wormhole">transform of the wormhole object</param>
    /// <param name="targetPos">position to move after animation</param>
    public void startWormhole(Transform wormhole, Vector3 targetPos)
    {
        wormholeTargetPos = targetPos;
        cameraManager.SetWormhole(wormhole);
        gameState = GameState.WormholeEffect;
        gameStateChange.NotifyObservers(gameState);
    }

    /// <summary>
    /// This function is called when the animation warping into the wormhole ends.
    /// </summary>
    public void exitWormhole()
    {
        playerManager.Teleport(wormholeTargetPos);
        gameState = GameState.Playing;
        gameStateChange.NotifyObservers(gameState);
    }


    /// <summary>
    /// This function saves life to PlayerPrefs
    /// </summary>
    /// <param name="life">life to save in PlayrePrefs</param>
    private void SaveLifeInfo(int life)
    {
        PlayerPrefs.SetInt("Life", life);
        PlayerPrefs.Save();
    }

    public void ResetLife()
    {
        SaveLifeInfo(beginningLife);
    }

    /// <summary>
    /// This function is called when a new scene is loaded to initialize life
    /// </summary>
    /// <remarks>
    /// checks whether the life need to be fully charged (life is fully charged only on first scenes of each stage)
    /// </remarks>
    private void InitializeLife()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // scene index(2: tutorial, 3: 1-1, : 2-1)
        if ((currentSceneIndex == 2) || (currentSceneIndex == 3) || (currentSceneIndex == 6))
        {
            life = 5;
            SaveLifeInfo(5);
        }
        else
        {
            life = PlayerPrefs.GetInt("Life");
        }
        beginningLife = life;
    }

    // CALLED BY OTHER SCRIPTS

    public int Life
    {
        get { return life; }
    }

    /// <summary>
    /// Called by obstacles to modify life
    /// </summary>
    /// <param name="amount">if positive life is increased, if negative life is decreased</param>    
    public void ModifyLife(int amount)
    {
        if (amount < 0) // harm
        {
            if (gameState == GameState.Playing)
            {
                life += amount;
                SaveLifeInfo(life);
                if (life <= 0)
                {
                    life = 0;
                    gameState = GameState.Gameover;
                    gameStateChange.NotifyObservers(gameState);
                }
                else
                {
                    StartCoroutine(HarmCoroutine());
                }
            }
        }
        else // energy
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
    /// Change player state Stun(2sec) -> Revived(3sec) -> Playing
    /// </summary>
    /// <returns></returns>
    private IEnumerator HarmCoroutine()
    {
        gameState = GameState.Stun;
        gameStateChange.NotifyObservers(gameState);
        yield return new WaitForSeconds(2f);
        gameState = GameState.Revived;
        gameStateChange.NotifyObservers(gameState);
        yield return new WaitForSeconds(3f);
        gameState = GameState.Playing;
        gameStateChange.NotifyObservers(gameState);
    }

    /// <summary>
    /// Used when adding an GameStateObserver is inefficient in terms of performance
    /// </summary>
    /// <remarks>
    /// For example, SpikeManager only needs the gameState when it has to decide whether to play the sound or not.
    /// It is inefficient for all spikes to track the gameState.
    /// <returns></returns>
    public GameState GetGameState()
    {
        return gameState;
    }

    /// <summary>
    /// This function is called by UIManager to set the gamestate to Paused
    /// </summary>
    public void Pause()
    {
        gameState = GameState.Paused;
        gameStateChange.NotifyObservers(gameState);
    }

    /// <summary>
    /// This function is called by UIManager to set the gamestate to Playing
    /// </summary>
    public void Resume()
    {
        gameState = GameState.Playing;
        gameStateChange.NotifyObservers(gameState);
    }
}
