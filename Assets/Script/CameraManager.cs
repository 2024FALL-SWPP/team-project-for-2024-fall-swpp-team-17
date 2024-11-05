using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Do not let the Camera see outside of the hallway.

/// <summary>
/// This class manages the overall rotation and location of the camera.
/// Camera rotation due to mouse movement is managed by CameraMouseController.
/// </summary>
public class CameraManager : MonoBehaviour
{
    Transform gravityTransform, playerTransform;
    Quaternion targetRot;
    public Vector3 offset = new Vector3(0, -3, 6);
    public float followSpeed = 15f;

    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerTransform = GameObject.Find("Player").transform;
        targetRot = gravityTransform.rotation;
    }

    void Update()
    {
        if (targetRot != transform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        transform.position = playerTransform.position + transform.rotation * offset;
        // Vector3 targetPostion = playerTransform.position + transform.rotation * offset;
        // transform.position = Vector3.Lerp(transform.position, targetPostion, Time.deltaTime * followSpeed);
    }

    /// <summary>
    /// This function is called by GravityManager when the gravity changes.
    /// It updates targetRot, so that the camera's rotation can smoothly change to the gravity's direction via the Slerp in the Update().
    /// </summary>
    public void CameraRot()
    {
        targetRot = gravityTransform.rotation;
    }
}
