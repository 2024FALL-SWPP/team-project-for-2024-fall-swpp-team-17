using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.Scripting.APIUpdating;
using System.Xml.Serialization;

public class PlayerManager : MonoBehaviour, GravityObserver, IPlayerManager, GameStateObserver
{
    public Rigidbody rb;
    private Animator animator;
    // public Renderer renderer;
    public Color originalColor, transparentColor;
    public InputManager inputManager;
    public float jumpForce = 1200f;
    public float moveSpeed = 20f;
    private Vector3 moveDirection;
    private Quaternion targetGravityRot = Quaternion.identity; // Target rotation for gravity changes. Direction of player facing forward in gravity
    public BoxCollider boxCollider;
    public float height;
    private bool isGround;
    GameState gameState;
    public int life;
    public int Life
    {
        get { return life; }
    }
    public float lastDamageTime = -100f;
    public bool revived = false;
    public bool isTransparent = false;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody>();
        // renderer = GetComponent<Renderer>();
        // originalColor = renderer.material.color;
        // transparentColor = originalColor;
        // transparentColor.a = 0.5f;
        rb.freezeRotation = true; // Freeze rotation so that Rigidbody does not control rotation
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.size.y;
        isGround = true;
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        life = 5;
    }

    void Update()
    {
        // TODO: check game state
        switch (gameState)
        {
            case GameState.Playing:
                RotatePlayer();
                JumpPlayer();
                break;
            default:
                break;
        }

        if (revived)
        {
            // Blink();
        }

    }

    // void Blink()
    // {
    //     float time = Time.time - lastDamageTime;
    //     if ((time % 1) < 0.5f && !isTransparent)
    //     {
    //         renderer.material.color = transparentColor;
    //         isTransparent = true;
    //     }
    //     else if ((time % 1) > 0.5f && isTransparent)
    //     {
    //         renderer.material.color = originalColor;
    //         isTransparent = false;
    //     }
    //     if (time > 3.0f)
    //     {
    //         renderer.material.color = originalColor;
    //         revived = false;
    //         isTransparent = false;
    //     }

    // }

    /// <summary>
    /// Moving player is here to prevent going through walls
    /// </summary>
    void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Gets Spacekey input and makes player jump.
    /// Double jump is prevented using isGround tag.
    /// </summary>
    void JumpPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isGround = false;
            animator.SetBool("Jump_b", true);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Rotate player to the direction of movement when it moves(has WASD input).
    /// The direction is determined by the mouse input and keybord input(WASD keys)
    /// </summary>
    void RotatePlayer()
    {
        // Handle input
        float horizontal = Input.GetAxis("Horizontal"); // Get horizontal input (A, D)
        float vertical = Input.GetAxis("Vertical");     // Get vertical input (W, S)
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            // Define directions considering gravity and mouse input
            Quaternion rotation = targetGravityRot * Quaternion.Euler(0, inputManager.yaw, 0);
            Vector3 forward = rotation * Vector3.forward;
            Vector3 right = rotation * Vector3.right;
            Vector3 up = rotation * Vector3.up;

            // Merge directions above and keybord input
            moveDirection = forward * inputDirection.z + right * inputDirection.x;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, up);

            // Update roatation and position to player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Apply animation
            animator.SetBool("Static_b", false);
            animator.SetFloat("Speed_f", 0.3f);
        }
        else
        {
            // Stop movement if there is no input
            moveDirection = Vector3.zero;

            // Apply animation
            animator.SetBool("Static_b", true);
            animator.SetFloat("Speed_f", 0);
        }

    }

    public bool a, b, c, d, e, f;
    /// <summary>
    /// Check if there are obstacles and move player in the moveDirection
    /// </summary>
    void MovePlayer()
    {
        if (!ObstacleInPath())
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Checks if there is an obstacle from origin, in direction, within distance
    /// </summary>
    /// <param name="origin">position of origin(player)</param>
    /// <param name="direction">direction to check if there is an obstacle</param>
    /// <param name="distance">distance(bound) to check if there is an obstacle</param>
    /// <returns></returns>
    bool ObstacleInPath()
    {
        float distance = moveSpeed * Time.fixedDeltaTime * 10;
        Vector3 footPosition = transform.position - transform.up * height;
        Vector3 headPosition = transform.position + transform.up * height;
        Vector3[] positions = new Vector3[] { transform.position, footPosition, headPosition };
        Vector3 leftDirection = Vector3.Cross(moveDirection, transform.up);
        Vector3 rightDirection = -Vector3.Cross(moveDirection, transform.up);
        Vector3[] directions = new Vector3[] { moveDirection, leftDirection, rightDirection };
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 direction = directions[i];
            for (int j = 0; j < positions.Length; j++)
            {
                Vector3 origin = positions[j];
                RaycastHit hit;
                if (Physics.Raycast(origin, direction, out hit, distance))
                {
                    Debug.DrawRay(origin, direction * distance, Color.red);
                    return true;
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.green);
                }
            }
        }
        return false;
    }

    /// <summary>
    /// check if ground is touched, update isGround
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            animator.SetBool("Jump_b", false);
        }
    }
    public void ModifyLife(int amount)
    {
        if ((amount < 0) && !revived)
        {
            life += amount;
        }

    }

    /// <summary>
    /// Move player to targetPos. Used in wormhole
    /// </summary>
    /// <param name="targetPos">position to teleport</param>
    public void Teleport(Vector3 targetPos)
    {
        transform.position = targetPos;
        transform.rotation = targetGravityRot;
    }

    /// <summary>
    /// Update targetGravityRot when gravity is changed.
    /// Player's rotation is altered so that it is standing up facing front.
    /// </summary>
    /// <param name="rot">how gravity is changed. should be multiplied to original rotation</param>
    public void OnNotify<GravityObserver>(Quaternion rot)
    {
        targetGravityRot = targetGravityRot * rot;
        transform.rotation = targetGravityRot;
    }

    public void OnNotify<GameStateObserver>(GameState gs)
    {
        gameState = gs;
        switch (gs)
        {
            case GameState.WormholeEffect:
                gameObject.SetActive(false);
                break;
            default:
                gameObject.SetActive(true);
                break;
        }
    }

}