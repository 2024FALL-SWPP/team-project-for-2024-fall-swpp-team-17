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
    public Transform player; // target to follow. Player in our case
    public float mouseSensitivity = 100f;
    private float distance = 15f; // distance to target
    private float targetDistance = 15f;
    public float height = 5f; // height from target
    Quaternion gravityRot, targetGravityRot;
    public InputManager inputManager;
    public GameManager gameManager;
    Vector3 targetPosition;
    private GameState gameState;
    public Transform wormhole = null;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        gravityRot = Quaternion.identity;
        targetGravityRot = Quaternion.identity;
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
    /// When there is an obstacle between the camera and player, this function shifts the camera to be in front of the obstacle, so that the camera can see the player.
    /// This also prevents the camera from seeing outside of the corridors.
    /// </summary>
    void ShiftToFront()
    {
        Vector3 directionToTarget = player.position - targetPosition;
        float distanceToTarget = Vector3.Distance(targetPosition, player.position);
        Ray ray = new Ray(targetPosition, directionToTarget);
        RaycastHit[] hits = Physics.RaycastAll(ray, distanceToTarget);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                float distanceObstToTarget = Vector3.Distance(hit.point, player.position);
                float shiftedDistance = distance * (distanceObstToTarget / distanceToTarget) * 0.5f;
                targetPosition = player.position - transform.forward * shiftedDistance + transform.up * height;
            }
        }
        // if (Physics.Raycast(targetPosition, directionToTarget, out RaycastHit hit, distanceToTarget))
        // {
        //     go = hit.collider.gameObject;
        //     if (hit.collider.gameObject.tag == "ground")
        //     {
        //         float distanceObstToTarget = Vector3.Distance(hit.point, player.position);
        //         float shiftedDistance = distance * (distanceObstToTarget / distanceToTarget) * 0.5f;
        //         targetPosition = player.position - transform.forward * shiftedDistance + transform.up * height;
        //     }
        // }
    }

    float spiralAngle = 0.0f, spiralRadius, distanceToWormhole;
    public float spiralSpeed = 15.0f;
    public float spiralRadiusDenom = 25.0f;
    public float moveSpeedNum = 2.5f;
    public float minRad = 0.5f;
    public float moveSpeed = 10.0f;

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
}