using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class WormholeManager : MonoBehaviour
{
    // public bool triggered = false;
    public GameManager gameManager;
    public Vector3 targetPos;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
    }

    // public void Reset()
    // {
    //     triggered = false;
    // }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //triggered = true;
            gameManager.startWormhole(transform, targetPos);
        }
    }
}
