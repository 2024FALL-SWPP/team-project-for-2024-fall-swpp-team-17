using System;
using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

public class HeavyObjectManager : HazardManager
{
    private GameManager gameManager;
    private AudioSource audioSource;
    private bool hasPlayedSound = false;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// checks if player is crushed below this object and harms player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IMyCollision mycol = new CollisionWrapper(collision);
            if (IsCrushed(mycol))
            {
                if (!hasPlayedSound)
                {
                    audioSource.Play();
                    hasPlayedSound = true;
                }
                HarmPlayer(gameManager);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasPlayedSound = false;
        }
    }

    /// <summary>
    /// whether collision is crushing player
    /// </summary>
    /// <remarks>
    /// Checks 1. if collision location is player's head and 2. if collision's direction was vertical
    /// Need to check second condition because if player walks to an object that is taller than itself and collides, the collision location can be head, but the player isn't crushed.
    /// <param name="collision"></param>
    /// <returns></returns>
    private bool IsCrushed(IMyCollision collision)
    {
        MyContactPoint[] contacts = new MyContactPoint[10];
        Vector3 playerUp = collision.gameObject.transform.up;
        Vector3 playerPos = collision.gameObject.transform.position;
        bool isCrushed = false;
        int cnt = collision.GetContacts(contacts);
        for (int i = 0; i < cnt; i++)
        {
            MyContactPoint contact = contacts[i];
            bool isLocationHead = Vector3.Dot(contact.point - playerPos, playerUp) > 0f;
            bool isCollisionVertical = Math.Abs(Vector3.Dot(contact.normal, playerUp)) > 0.5f;
            if (isLocationHead && isCollisionVertical)
            {
                isCrushed = true;
            }
        }
        return isCrushed;
    }

    protected override void HarmPlayer(ILifeManager gm)
    {
        gameManager.ModifyLife(-damage);
    }
}