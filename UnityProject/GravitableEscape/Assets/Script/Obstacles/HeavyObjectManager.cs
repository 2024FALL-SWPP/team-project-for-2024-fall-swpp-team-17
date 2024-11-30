using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

public class HeavyObjectManager : HazardManager
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Checks if player collided with the pointy part of the thorn, calls playerManager.ThornDamage() if so.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsCollisionHead(collision))
            {
                HarmPlayer(gameManager);
            }
        }
    }

    private bool IsCollisionHead(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        Vector3 playerUp = collision.gameObject.transform.up;
        Vector3 playerPos = collision.gameObject.transform.position;
        bool isHead = false;
        int cnt = collision.GetContacts(contacts);
        for (int i = 0; i < cnt; i++)
        {
            ContactPoint contact = contacts[i];
            Vector3 direction = contact.point - playerPos;
            if (Vector3.Dot(direction, playerUp) > 0f)
            {
                isHead = true;
            }
        }
        return isHead;
    }
    protected override void HarmPlayer(ILifeManager gm)
    {
        gameManager.ModifyLife(-damage);
    }
}