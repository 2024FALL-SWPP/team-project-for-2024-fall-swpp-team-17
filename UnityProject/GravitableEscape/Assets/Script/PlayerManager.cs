using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerManager : MonoBehaviour
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
        moveDirection = gravityTransform.rotation * new Vector3(horizontalInput, 0, verticalInput).normalized;
        if (moveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -Physics.gravity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            //if you want to look like ice...
            //playerRb.velocity += moveDirection * 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(transform.rotation * new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
        }
    }
    public bool a, b, c;
    void FixedUpdate()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 footPosition = transform.position + Vector3.down * height / 3;
            Vector3 headPosition = transform.position + Vector3.up * height / 3;
            float distance = moveSpeed * Time.fixedDeltaTime * 10;
            a = ObstacleInPath(transform.position, moveDirection, distance);
            b = ObstacleInPath(footPosition, moveDirection, distance);
            c = ObstacleInPath(headPosition, moveDirection, distance);
            if (!ObstacleInPath(transform.position, moveDirection, distance)
            && !ObstacleInPath(footPosition, moveDirection, distance)
            && !ObstacleInPath(headPosition, moveDirection, distance))
            {
                Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
                playerRb.MovePosition(newPosition);
            }
            // Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            // playerRb.MovePosition(newPosition);
        }
    }

    bool ObstacleInPath(Vector3 origin, Vector3 direction, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distance)) return true;
        else return false;
    }


    /// <summary>
    /// This function rotates the player to align with the gravity.
    /// </summary>
    public void PlayerRot()
    {
        transform.rotation = gravityTransform.rotation;
    }

    public void TakeDamage()
    {
        life--;
    }

}
