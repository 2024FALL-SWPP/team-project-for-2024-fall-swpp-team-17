using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

public class ThornManager : HazardManager
{
    Rigidbody rb;
    Vector3 fixedPosition;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        fixedPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.position = fixedPosition;
    }

    private ContactPoint[] contacts = new ContactPoint[10];
    private float lastCollisionTime = -100.0f;
    public bool isUpward;

    /// <summary>
    /// Checks if player collided with the pointy part of the thorn, calls playerManager.ThornDamage() if so.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsCollisionUpward(collision) && (Time.time - lastCollisionTime >= 3.0f))
            {
                Player player = collision.gameObject.GetComponent<PlayerManager>().player;
                HarmPlayer(player);
                lastCollisionTime = Time.time;
            }
        }
    }

    bool IsCollisionUpward(Collision collision)
    {
        bool isUpward = false;
        int cnt = collision.GetContacts(contacts);
        for (int i = 0; i < cnt; i++)
        {
            ContactPoint contact = contacts[i];
            if (Vector3.Dot(contact.normal, Vector3.up) < -0.9f)
            {
                isUpward = true;
            }
        }
        return isUpward;
    }

    protected override void HarmPlayer(Player player)
    {
        player.ModifyLife(-damage);
    }
}
