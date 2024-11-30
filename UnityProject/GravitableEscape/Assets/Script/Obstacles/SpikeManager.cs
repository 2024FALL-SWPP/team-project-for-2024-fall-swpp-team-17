using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

public class SpikeManager : HazardManager
{
    Vector3 fixedPosition;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        fixedPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        transform.position = fixedPosition;
    }

    /// <summary>
    /// checks conditions and harms player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsCollisionUpward(collision))
            {
                HarmPlayer(gameManager);
            }
        }
    }

    /// <summary>
    /// whether collision is on the pointy part of spike
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    bool IsCollisionUpward(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        bool isUpward = false;
        int cnt = collision.GetContacts(contacts);
        for (int i = 0; i < cnt; i++)
        {
            ContactPoint contact = contacts[i];
            if (Vector3.Dot(contact.normal, transform.up) < -0.9f)
            {
                isUpward = true;
            }
        }
        return isUpward;
    }

    protected override void HarmPlayer(ILifeManager gm)
    {
        gameManager.ModifyLife(-damage);
    }
}
