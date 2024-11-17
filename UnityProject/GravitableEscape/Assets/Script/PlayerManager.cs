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
    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        height = playerCollider.size.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        moveDirection = forward * verticalInput + right * horizontalInput;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 footPosition = transform.position + Vector3.down * height / 3;
            Vector3 headPosition = transform.position + Vector3.up * height / 3;
            float distance = moveSpeed * Time.fixedDeltaTime * 10;
            if (!ObstacleInPath(transform.position, moveDirection, distance)
            && !ObstacleInPath(footPosition, moveDirection, distance)
            && !ObstacleInPath(headPosition, moveDirection, distance))
            {
                Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
                playerRb.MovePosition(newPosition);
            }
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
