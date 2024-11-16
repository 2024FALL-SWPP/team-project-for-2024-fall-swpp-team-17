using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using OurGame;

// TODO: Do not let the Camera see outside of the hallway.

/// <summary>
/// This class manages the overall rotation and location of the camera.
/// Camera rotation due to mouse movement is managed by CameraMouseController.
/// </summary>
public class CameraManager : MonoBehaviour, GravityObserver
{
    Transform gravityTransform, playerTransform;
    public Transform wormhole;
    Quaternion targetRot;
    public Vector3 offset = new Vector3(0, -3, 6);
    public float followSpeed = 15f;
    public float moveSpeed = 10.0f;
    public int cameraMode = 0; // 0 is default mode, 1 is wormhole mode
    private GameState gameState;
    CameraMouseManager cameraMouseManager;
    GameManager gameManager;

    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerTransform = GameObject.Find("Player").transform;
        targetRot = gravityTransform.rotation;
        cameraMouseManager = GameObject.Find("Main Camera").GetComponent<CameraMouseManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraMode = 0;
    }

    /// <summary>
    /// This function updates camera rotation and position according to the mode.
    /// 0: default mode, 1: wormhole animation mode
    /// </summary>
    void Update()
    {
        switch (gameState)
        {
            case GameState.Playing:
                FollowPlayer();
                break;
            case GameState.WormholeEffect:
                SpiralTowardsWormhole();
                break;
        }
    }

    private void FollowPlayer()
    {
        if (targetRot != transform.rotation) transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        transform.position = playerTransform.position + transform.rotation * offset;
    }


    float spiralAngle = 0.0f, spiralRadius, distanceToWormhole;
    public float spiralSpeed = 15.0f;
    public float spiralRadiusDenom = 25.0f;
    public float moveSpeedNum = 2.5f;
    public float minRad = 0.5f;

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
            exitWormholeMode();
            gameManager.exitWormhole();
        }
    }

    /// <summary>
    /// Accelerates towards wormhole. can be used in mode 1.
    /// </summary>
    private void MoveTowardsWormhole()
    {
        if (Vector3.Distance(transform.position, wormhole.position) > 2f)
        {
            transform.position = Vector3.Lerp(transform.position, wormhole.position, moveSpeed * Time.deltaTime);
            moveSpeed += 2.0f * Time.deltaTime;
            transform.LookAt(wormhole);
        }
        else
        {
            exitWormholeMode();
            gameManager.exitWormhole();
        }
    }

    public void enterWormholeMode(Transform wormhole)
    {
        gameState = GameState.WormholeEffect;
        cameraMouseManager.SetMouseControl(false);
        this.wormhole = wormhole;
    }

    public void exitWormholeMode()
    {
        gameState = GameState.Playing;
        cameraMouseManager.SetMouseControl(true);
    }

    public void OnNotify(Quaternion gravityRot)
    {
        targetRot = gravityRot;
    }
}