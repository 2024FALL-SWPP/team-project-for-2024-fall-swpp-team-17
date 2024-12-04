using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class WormholeManager : MonoBehaviour
{
    public GameManager gameManager;
    public Vector3 targetPos;
    private AudioSource wormholl;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        wormholl = GetComponent<AudioSource>();
    }

    void Update()
    { }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.startWormhole(transform, targetPos);
        }
    }
}
