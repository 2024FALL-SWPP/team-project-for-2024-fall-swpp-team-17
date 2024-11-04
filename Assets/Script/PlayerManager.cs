using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;
    public float jumpForce = 600f;
    private float horizontalInput, verticalInput;
    private Vector3 moveDirection;
    Transform rot;
    Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        rot = GameObject.Find("Rot").transform;
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = rot.transform.rotation;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        moveDirection = rot.transform.rotation * new Vector3(horizontalInput, 0, verticalInput).normalized;
        // moveDirection = rot.TransformDirection(localDirection);

        if (moveDirection.magnitude >= 0.1f)
        {
            // 플레이어가 이동할 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 이동
            //transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        // transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(transform.rotation * new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
        }
    }

}
