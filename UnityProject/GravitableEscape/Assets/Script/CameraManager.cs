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
    //public Vector3 offset = new Vector3(0, -3, 6);
    public float followSpeed = 15f;
    public float moveSpeed = 10.0f;
    public int cameraMode = 0; // 0 is default mode, 1 is wormhole mode
    private GameState gameState;
    CameraMouseManager cameraMouseManager;
    GameManager gameManager;

    public float distance = 10.0f;
    public float verticalOffset = -3.0f;
    public float rotationSpeed = 5f;
    private Vector3 sphericalCoordinates;
    private float mouseRotX, mouseRotY;
    private float sensitivity = 5f;
    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerTransform = GameObject.Find("Player").transform;
        targetRot = gravityTransform.rotation;
        cameraMouseManager = GameObject.Find("Main Camera").GetComponent<CameraMouseManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraMode = 0;

        sphericalCoordinates = new Vector3(0, 0, distance);
        UpdateCameraPosition();

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
                UpdateCameraPosition();
                break;
            case GameState.WormholeEffect:
                SpiralTowardsWormhole();
                break;
        }
    }

    private void UpdateCameraPosition()
    {

        if (Input.GetMouseButtonDown(0)) ResetMouseControl();

        mouseRotX += -Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        mouseRotY += Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        sphericalCoordinates.x += mouseRotX * rotationSpeed * Time.deltaTime;
        sphericalCoordinates.y = Mathf.Clamp(sphericalCoordinates.y + mouseRotY * rotationSpeed * Time.deltaTime, 0.1f, Mathf.PI - 1.5f);

        Vector3 centerOffset = new Vector3(0, 4, -4.0f);
        Vector3 center = playerTransform.localPosition + playerTransform.localRotation * centerOffset;

        float x = sphericalCoordinates.z * Mathf.Cos(sphericalCoordinates.x) * Mathf.Sin(sphericalCoordinates.y);
        float y = sphericalCoordinates.z * Mathf.Cos(sphericalCoordinates.y) + verticalOffset;
        float z = sphericalCoordinates.z * Mathf.Sin(sphericalCoordinates.x) * Mathf.Sin(sphericalCoordinates.y);

        Vector3 newPosition = center + gravityTransform.rotation * new Vector3(x, y, z);
        transform.localPosition = newPosition;
        //transform.localPosition = new Vector3(x, y, z);
        transform.LookAt(center);
        //transform.LookAt(playerTransform);

        UpdatePlayerRotation();

    }

    private void UpdatePlayerRotation()
    {
        Vector3 directionToCamera = transform.localPosition - playerTransform.localPosition;
        directionToCamera.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);

        playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, targetRotation, Time.deltaTime * 100);


    }

    private void ResetMouseControl()
    {
        mouseRotX = 0;
        mouseRotY = 0;
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