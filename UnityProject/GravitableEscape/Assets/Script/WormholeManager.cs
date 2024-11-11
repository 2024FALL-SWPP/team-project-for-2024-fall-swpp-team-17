using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class WormholeManager : MonoBehaviour
{
    public Transform playerTransform;
    public float triggerDistance = 50.0f;
    public bool triggered = false;
    public GameManager gameManager;
    public Vector3 targetPos;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!triggered && Vector3.Distance(playerTransform.position, transform.position) < triggerDistance)
        {
            triggered = true;
            gameManager.startWormhole(gameObject, targetPos);
        }
    }

    public void Reset()
    {
        triggered = false;
    }

}
