using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using OurGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour, GravityObserver
{
    public Transform player; // target to follow. Player in our case
    public float mouseSensitivity = 100f;
    public float distance = 10f; // distance to target
    private float targetDistance = 10f;
    public float height = 5f; // height from target
    Quaternion gravityRot, targetGravityRot;
    public InputManager inputManager;
    Vector3 targetPosition;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        gravityRot = Quaternion.identity;
        targetGravityRot = Quaternion.identity;
    }


    void LateUpdate()
    {
        ScrollDistance();
        RotateCamera();
        FollowPlayer();
        ShiftToFront();
        transform.position = targetPosition;
    }

    void ScrollDistance()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            targetDistance = Mathf.Clamp(distance - scrollInput * 10f, 5f, 20f);
        }
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * 100f);
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
            if (hit.collider.gameObject.name != "astronaut")
            {
                float distanceObstToTarget = Vector3.Distance(hit.point, player.position);
                float shiftedDistance = distance * (distanceObstToTarget / distanceToTarget) * 0.9f;
                targetPosition = player.position - transform.forward * shiftedDistance + transform.up * height;
            }
        }
    }

    public void OnNotify(Quaternion rot)
    {
        targetGravityRot = targetGravityRot * rot;
    }
}