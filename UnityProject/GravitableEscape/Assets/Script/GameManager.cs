using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Diagnostics;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour, ILifeManager
{
    public CameraManager cameraManager;
    public PlayerManager playerManager;
    public UIManager uIManager;
    public Vector3 wormholeTargetPos;
    public GameState gameState; // TODO: make this singleton?
    Subject<GameStateObserver, GameState> gameStateChange;
    public bool isTutorialScene; // Checks if tutorial ui should be active

    void Start()
    {
        gameState = GameState.Playing;

        gameStateChange = new Subject<GameStateObserver, GameState>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        uIManager = FindObjectOfType<UIManager>();
        gameStateChange.AddObserver(cameraManager);
        gameStateChange.AddObserver(playerManager);
        gameStateChange.AddObserver(uIManager);
        gameStateChange.NotifyObservers(gameState);

        isTutorialScene = SceneManager.GetActiveScene().name == "TutorialUITest";
        UnityEngine.Debug.Log("GM.Start() ... isTutorialScene: " + isTutorialScene);
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

    // CALLED BY OTHER SCRIPTS
    /// <summary>
    /// Called by obstacles, energy boosters? to modify life
    /// </summary>
    /// <param name="amount">if positive life is increased, if negative life is decreased</param>
    public int life = 5;
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
    /// do Pause and change button color
    /// </summary>
    public void Puase()
    {
        if (Time.timeScale == 0)
        {
            uIManager.ChangePauseColorBlack();
            Time.timeScale = 1;
        }
        else
        {
            uIManager.ChangePauseColorRed();
            Time.timeScale = 0;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

}
