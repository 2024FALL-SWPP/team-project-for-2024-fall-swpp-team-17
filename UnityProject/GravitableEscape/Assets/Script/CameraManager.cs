using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using OurGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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


    void LateUpdate()
    {
        switch (gameState)
        {
            case GameState.Playing:
                ScrollDistance();
                RotateCamera();
                FollowPlayer();
                ShiftToFront();
                transform.position = targetPosition;
                break;
            case GameState.WormholeEffect:
                SpiralTowardsWormhole();
                break;
            default:
                break;
        }

    }

    void ScrollDistance()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            targetDistance = Mathf.Clamp(distance - scrollInput * 10f, 5f, 25f);
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

    void ShiftToFront()
    {
        Vector3 directionToTarget = player.position - targetPosition;
        float distanceToTarget = Vector3.Distance(targetPosition, player.position);
        if (Physics.Raycast(targetPosition, directionToTarget, out RaycastHit hit, distanceToTarget))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                float distanceObstToTarget = Vector3.Distance(hit.point, player.position);
                float shiftedDistance = distance * (distanceObstToTarget / distanceToTarget) * 0.5f;
                targetPosition = player.position - transform.forward * shiftedDistance + transform.up * height;
            }
        }
    }

    float spiralAngle = 0.0f, spiralRadius, distanceToWormhole;
    public float spiralSpeed = 15.0f;
    public float spiralRadiusDenom = 25.0f;
    public float moveSpeedNum = 2.5f;
    public float minRad = 0.5f;
    public float moveSpeed = 10.0f;

    /// <summary>
    /// Updates camera's position to spiral towards the wormhole.
    /// can be used in mode 1.
    /// </summary>
    // TODO: alter constants on main scene
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