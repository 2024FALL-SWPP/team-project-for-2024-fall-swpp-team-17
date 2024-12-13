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
    public GameState gameState; // TODO: make this singleton?
    Subject<GameStateObserver, GameState> gameStateChange;
    public bool isMessageRequiredScene; // Checks if message ui should be active

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

        UpdateLife();

        isMessageRequiredScene = SceneManager.GetActiveScene().name == "Tutorial";
    }

    // Update is called once per frame
    void Update()
    {

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

    // check whether the life need to be fully charged
    // scene index(1: tutorial, 2: 1-1, 5: 2-1)
    private bool isNewStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if ((currentSceneIndex == 2) || (currentSceneIndex == 3) || (currentSceneIndex == 6))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // save life information
    private void SaveLifeInfo(int lifeInformation)
    {
        PlayerPrefs.SetInt("Life", lifeInformation);
        PlayerPrefs.Save();
    }

    public void ResetLife()
    {
        SaveLifeInfo(beginningLife);
    }

    // called when new Scene loaded
    private void UpdateLife()
    {
        if (isNewStage())
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
    /// <summary>
    /// Called by obstacles, energy boosters? to modify life
    /// </summary>
    /// <param name="amount">if positive life is increased, if negative life is decreased</param>    
    public int Life
    {
        get { return life; }
    }
    // private float lastHarmTime = -100.0f;
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

    public GameState GetGameState()
    {
        return gameState;
    }

    public void Pause()
    {
        gameState = GameState.Paused;
        gameStateChange.NotifyObservers(gameState);
    }

    public void Resume()
    {
        gameState = GameState.Playing;
        gameStateChange.NotifyObservers(gameState);
    }


}
