using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 600f;
    private float horizontalInput, verticalInput;
    GameObject rot;
    Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        rot = GameObject.Find("Rot");
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rot.transform.rotation;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Space)){
            playerRb.AddForce(transform.rotation * new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
        }
    }
}
