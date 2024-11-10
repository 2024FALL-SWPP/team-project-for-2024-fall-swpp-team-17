using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeManager : MonoBehaviour
{
    public Transform playerTransform;
    public float triggerDistance = 50.0f;
    public bool triggered = false;
    public CameraManager cameraManager;
    void Start()
    {
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }

    void Update()
    {
        if (!triggered && Vector3.Distance(playerTransform.position, transform.position) < triggerDistance)
        {
            triggered = true;
            cameraManager.enterWormholeMode(transform);
        }
    }
}
