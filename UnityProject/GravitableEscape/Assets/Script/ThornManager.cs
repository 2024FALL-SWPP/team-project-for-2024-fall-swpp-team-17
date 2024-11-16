using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornManager : MonoBehaviour
{
    PlayerManager playerManager;
    Rigidbody rb;
    Vector3 fixedPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        fixedPosition = transform.position;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rb.MovePosition(fixedPosition);
    }

    public int foo = 0;
    public int bar = 0;
    public Vector3 n;
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
            isUpward = false;
            int cnt = collision.GetContacts(contacts);
            for (int i = 0; i < cnt; i++)
            {
                ContactPoint contact = contacts[i];
                n = contact.normal;
                if (Vector3.Dot(contact.normal, Vector3.up) < -0.9f)
                {
                    isUpward = true;
                }
            }
            if (isUpward && (Time.time - lastCollisionTime >= 3.0f))
            {
                playerManager.ThornDamage();
                lastCollisionTime = Time.time;
            }
        }
    }
}
