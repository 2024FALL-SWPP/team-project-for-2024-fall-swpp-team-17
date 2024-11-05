using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float rotationSpeed = 10f;
    public float jumpForce = 1200f;
    private float horizontalInput, verticalInput;
    private Vector3 moveDirection;
    Transform gravityTransform;
    Rigidbody playerRb;
    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerRb = GetComponent<Rigidbody>();
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
    void FixedUpdate()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            RaycastHit hit;
            float distance = moveSpeed * Time.fixedDeltaTime * 10;
            if (!Physics.Raycast(transform.position, moveDirection, out hit, distance))
            {
                Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
                playerRb.MovePosition(newPosition);
            }
        }
    }
    /// <summary>
    /// This function rotates the player to align with the gravity.
    /// </summary>
    public void PlayerRot()
    {
        transform.rotation = gravityTransform.rotation;
    }
}
