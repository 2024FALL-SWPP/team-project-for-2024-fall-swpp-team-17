using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IPlayerManager
{
    public CameraManager cameraManager;
    public PlayerManager playerManager;
    public UIManager uIManager;
    public Vector3 wormholeTargetPos;
    public GameState gameState; // TODO: make this singleton?
    Subject<GameStateObserver, GameState> gameStateChange;
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

    public bool foo = false;
    /// <summary>
    /// This function is called when the animation warping into the wormhole ends.
    /// </summary>
    public void exitWormhole()
    {
        foo = true;
        playerManager.Teleport(wormholeTargetPos);
        gameState = GameState.Playing;
        gameStateChange.NotifyObservers(gameState);
    }

    // CALLED BY OTHER SCRIPTS
    /// <summary>
    /// Called by obstacles, energy boosters? to modify life
    /// </summary>
    /// <param name="amount">if positive life is increased, if negative life is decreased</param>
    private int life = 2;
    public int Life
    {
        get { return life; }
    }
    private float lastCollisionTime = -100.0f;
    public void ModifyLife(int amount)
    {
        if (Time.time - lastCollisionTime >= 10.0f)
        {
            lastCollisionTime = Time.time;
            life += amount;

            if (life <= 0)
            {
                life = 0;
                gameState = GameState.Gameover;
                gameStateChange.NotifyObservers(gameState);
            }

            playerManager.ModLife(amount);
        }
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
