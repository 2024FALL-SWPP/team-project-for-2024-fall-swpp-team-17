using System;
using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

/// <summary>
/// Manages the behavior of heavy objects that can harm the player when crushed underneath.
/// </summary>
/// <remarks>
/// This class inherits from <see cref="HazardManager"/> and interacts with the game manager to reduce the player's life
/// when a collision determines the player has been crushed.
/// </remarks>
public class HeavyObjectManager : HazardManager
{
    private GameManager gameManager;
    private AudioSource audioSource;
    private bool hasPlayedSound = false;

    void Start()
    {
        damage = 1; // Set damage inflicted by this hazard
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Checks if the player is crushed by this object upon collision.
    /// If so, plays a sound and harms the player.
    /// </summary>
    /// <param name="collision">The collision data associated with the event.</param>
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

    /// <summary>
    /// Resets the sound flag when the player exits the collision.
    /// </summary>
    /// <param name="collision">The collision data associated with the event.</param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasPlayedSound = false;
        }
    }

    /// <summary>
    /// Determines if the collision indicates the player is being crushed by the object.
    /// </summary>
    /// <remarks>
    /// A collision is considered crushing if:
    /// 1. The collision point is above the player's head.
    /// 2. The collision normal indicates a primarily vertical force.
    /// 
    /// These checks ensure that horizontal collisions, such as walking into an object, are not mistaken for crushing events.
    /// </remarks>
    /// <param name="collision">The collision data wrapped in a custom interface.</param>
    /// <returns>True if the player is being crushed, otherwise false.</returns>
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

    /// <summary>
    /// Reduces the player's life when harmed by the hazard.
    /// </summary>
    /// <param name="gm">The game manager interface for modifying player life.</param>
    protected override void HarmPlayer(ILifeManager gm)
    {
        gameManager.ModifyLife(-damage);
    }
}