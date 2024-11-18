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

    private Animator animator;
    void Start()
    {
        gravityTransform = GameObject.Find("GravityManager").transform;
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        height = playerCollider.size.y;
        isground = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveDirection = new Vector3(0, 0, 0);
        }
        else
        {
            moveDirection = forward * verticalInput + right * horizontalInput;

            if (Input.GetKeyDown(KeyCode.Space) && isground)
            {
                isground = false;
                animator.SetBool("Jump_b", true);
                playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }

    }
    void FixedUpdate()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            animator.SetBool("Static_b", false);
            animator.SetFloat("Speed_f", 0.3f);

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
        else
        {
            animator.SetBool("Static_b", true);
            animator.SetFloat("Speed_f", 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isground = true;
            animator.SetBool("Jump_b", false);
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
