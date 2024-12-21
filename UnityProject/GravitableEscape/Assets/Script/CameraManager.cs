using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using OurGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class manages camera movement and rotation.
/// In most cases it follows the player and roates using mouse input.
/// It has some special effects like spiraling towards wormhole.
/// </summary>
public class CameraManager : MonoBehaviour, GravityObserver, GameStateObserver
{
    Transform player; // target to follow. Player in our case
    private float distance = 15f; // distance to target
    private float targetDistance = 15f;
    float height = 5f; // height from target
    Quaternion gravityRot, targetGravityRot;
    InputManager inputManager;
    GameManager gameManager;
    Vector3 targetPosition;
    GameState gameState;
    Transform wormhole = null;
    Vector3 boundsMin, boundsMax;
    public float corridors_z_min, corridors_z_max;

    private AudioSource bgmAudioSource; // bgm audio source
    private bool isbgmPlaying = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        gravityRot = Quaternion.identity;
        targetGravityRot = Quaternion.identity;
        bgmAudioSource = GetComponent<AudioSource>();
        boundsMin = new Vector3(-15f, -15f, corridors_z_min);
        boundsMax = new Vector3(15f, 15f, corridors_z_max);
    }


    /// <summary>
    /// Checks gameState and does appropriate camera movement
    /// </summary>
    void LateUpdate()
    {
        switch (gameState)
        {
            case GameState.Playing:
            case GameState.Stun:
            case GameState.Revived:
                ScrollDistance();
                RotateCamera();
                FollowPlayer();
                ShiftToFront();
                transform.position = targetPosition;
                break;
            case GameState.WormholeEffect:
                SpiralTowardsWormhole();
                break;
            case GameState.Gameover:
                GameOverCameraMove();
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// This function is called when the game enters the gameover state
    /// </summary>
    void GameOverCameraMove()
    {
        float radius = 15.0f;
        float speed = 1.5f;
        float angle = Time.time * speed;
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
        gravityRot = targetGravityRot;
        transform.position = player.position + gravityRot * offset + gravityRot * new Vector3(0, 10, 0);
        transform.LookAt(player, -Physics.gravity);
    }

    /// <summary>
    /// This function alters distance to reflect mouse scroll input
    /// </summary>
    void ScrollDistance()
    {
        if (inputManager.scrollInput != 0)
        {
            targetDistance = Mathf.Clamp(distance - inputManager.scrollInput * 10f, 5f, 25f);
        }
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * 10f);
    }

    void RotateCamera()
    {
        gravityRot = Quaternion.Slerp(gravityRot, targetGravityRot, Time.deltaTime * 10);
        transform.rotation = gravityRot * Quaternion.Euler(inputManager.pitch, inputManager.yaw, 0);
    }

    void FollowPlayer()
    {
        targetPosition = player.position - transform.forward * distance + transform.up * height;
    }

    /// <summary>
    /// When the camera is outside the corridor, this function shifts the camera to be in front of the wall, so that the camera can see the player.
    /// </summary>
    void ShiftToFront()
    {
        targetPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, boundsMin.x, boundsMax.x),
            Mathf.Clamp(targetPosition.y, boundsMin.y, boundsMax.y),
            Mathf.Clamp(targetPosition.z, boundsMin.z, boundsMax.z)
        );
    }

    float spiralAngle = 0.0f, spiralRadius, distanceToWormhole;
    float spiralSpeed = 15.0f;
    float spiralRadiusDenom = 25.0f;
    float moveSpeedNum = 2.5f;
    float minRad = 0.5f;
    float moveSpeed = 10.0f;

    /// <summary>
    /// Updates camera's position to spiral towards the wormhole.
    /// </summary>
    private void SpiralTowardsWormhole()
    {
        if (Vector3.Distance(transform.position, wormhole.position) > 1f)
        {
            spiralAngle += spiralSpeed * Time.deltaTime;
            distanceToWormhole = Vector3.Distance(transform.position, wormhole.position);
            spiralRadius = Mathf.Min(minRad, distanceToWormhole / spiralRadiusDenom);
            Vector3 spiralOffest = new Vector3(
                Mathf.Cos(spiralAngle) * spiralRadius,
                Mathf.Sin(spiralAngle) * spiralRadius,
                0);

            transform.position = Vector3.Lerp(transform.position, wormhole.position, moveSpeed * Time.deltaTime) + spiralOffest;
            moveSpeed = moveSpeedNum;

            transform.LookAt(wormhole);
        }
        else
        {
            gameManager.exitWormhole();
        }
    }

    /// <summary>
    /// Sets wormhole position so that the camera can spiral towards that position
    /// </summary>
    /// <param name="wh">Transform of the wormhole</param>
    public void SetWormhole(Transform wh)
    {
        wormhole = wh;
    }

    public void OnNotify<GravityObserver>(Quaternion rot)
    {
        targetGravityRot = targetGravityRot * rot;
    }

    public void OnNotify<GameStateObserver>(GameState gs)
    {
        gameState = gs;
    }

    /// <summary>
    /// Turns on/off bgm
    /// </summary>
    public void ToggleBGM()
    {
        if (isbgmPlaying)
        {
            bgmAudioSource.Pause();
        }
        else
        {
            bgmAudioSource.Play();
        }
        isbgmPlaying = !isbgmPlaying;
    }
}