using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerManager : MonoBehaviour, GravityObserver
{
    public int life = 5;
    public float moveSpeed = 20f;
    public float rotationSpeed = 10f;
    public float jumpForce = 1200f;
    private float horizontalInput, verticalInput;
    private Vector3 moveDirection;
    Transform gravityTransform;
    Rigidbody playerRb;
    BoxCollider playerCollider;
    float height;
    bool isground;
    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        height = playerCollider.size.y;
        isground = true;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        moveDirection = forward * verticalInput + right * horizontalInput;

        if (Input.GetKeyDown(KeyCode.Space) && isground)
        {
            isground = false;
            playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 footPosition = transform.position + Vector3.down * height / 2.5f;
            Vector3 headPosition = transform.position + Vector3.up * height / 2.5f;
            float distance = moveSpeed * Time.fixedDeltaTime * 10;
            if (!ObstacleInPath(transform.position, moveDirection, distance)
            && !ObstacleInPath(footPosition, moveDirection, distance)
            && !ObstacleInPath(headPosition, moveDirection, distance))
            {
                transform.position = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isground = true;

        }
    }

    bool ObstacleInPath(Vector3 origin, Vector3 direction, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distance)) return true;
        else return false;
    }

    public void OnNotify(Quaternion gravityRot)
    {
        transform.rotation = gravityRot;
    }

    public void ThornDamage()
    {
        // TODO: Animation
        life--;
    }
}
