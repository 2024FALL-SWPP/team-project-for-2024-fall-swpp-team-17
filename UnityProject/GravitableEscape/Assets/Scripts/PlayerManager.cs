using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

/// <summary>
/// Handles the player's movement, jumping, animations, life status, and gravity adjustments.
/// </summary>
public class PlayerManager : MonoBehaviour, GravityObserver, GameStateObserver
{
    public Rigidbody rb; // Reference to the Rigidbody for physics-based movement
    private Animator animator; // Handles player animations
    public Renderer[] renderers; // Renderer array for controlling material transparency
    public InputManager inputManager; // Reference to the InputManager for movement inputs
    public float jumpForce = 1500f; // Force applied when the player jumps
    public float moveSpeed = 20f; // Movement speed of the player
    private Vector3 moveDirection; // Direction in which the player moves
    private Quaternion targetGravityRot = Quaternion.identity; // Rotation aligning with the current gravity
    public BoxCollider boxCollider; // Collider for ground and obstacle detection
    private float height; // Player's height based on the BoxCollider
    private bool isGround; // Tracks if the player is on the ground
    GameState gameState; // Current game state
    public float revivedTime = -100f; // Time when the player was revived
    public bool revived = false; // Tracks if the player is in a revived state
    public bool isTransparent = false; // Tracks if the player is currently blinking

    private AudioSource[] audioSources; // Array of AudioSources for sound effects
    public AudioSource landing; // Landing sound
    public AudioSource step; // Walking sound
    public AudioSource pipe; // Pipe interaction sound

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>(); // Find InputManager in the scene
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from altering rotation
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.size.y; // Store player's height for movement logic
        isGround = true;
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false; // Manually control player movement
        renderers = GetComponentsInChildren<Renderer>();
        audioSources = GetComponents<AudioSource>();
        landing = audioSources[0];
        step = audioSources[1];
        pipe = audioSources[2];
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.Playing:
                RotatePlayer();
                JumpPlayer();
                break;
            case GameState.Revived:
                RotatePlayer();
                JumpPlayer();
                Blink();
                break;
            case GameState.Gameover:
            case GameState.Stun:
                break;
        }
    }

    void FixedUpdate()
    {
        switch (gameState)
        {
            case GameState.Playing:
            case GameState.Revived:
                MovePlayer();
                break;
        }
    }

    // PLAYER BASIC MOVEMENT SCRIPTS

    /// <summary>
    /// Rotates the player to face the movement direction based on input.
    /// </summary>
    void RotatePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Horizontal input (A/D)
        float vertical = Input.GetAxis("Vertical"); // Vertical input (W/S)
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            Quaternion rotation = targetGravityRot * Quaternion.Euler(0, inputManager.yaw, 0);
            Vector3 forward = rotation * Vector3.forward;
            Vector3 right = rotation * Vector3.right;
            Vector3 up = rotation * Vector3.up;

            moveDirection = forward * inputDirection.z + right * inputDirection.x;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, up);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            animator.SetBool("Static_b", false);
            animator.SetFloat("Speed_f", 0.3f);
        }
        else
        {
            moveDirection = Vector3.zero;
            animator.SetBool("Static_b", true);
            animator.SetFloat("Speed_f", 0);
            step.Pause();
        }
    }

    /// <summary>
    /// Moves the player in the specified direction, checking for obstacles.
    /// </summary>
    void MovePlayer()
    {
        if (!ObstacleInPath())
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            animator.SetFloat("Speed_f", moveDirection.magnitude * moveSpeed);

            if (!step.isPlaying && moveDirection.magnitude > 0.1f && isGround)
            {
                step.Play();
            }
        }
        else
        {
            animator.SetFloat("Speed_f", 0f);
        }
    }

    /// <summary>
    /// Checks for obstacles in the player's path using raycasts.
    /// </summary>
    /// <returns>True if an obstacle is detected, false otherwise.</returns>
    bool ObstacleInPath()
    {
        float d = moveSpeed * Time.fixedDeltaTime;
        float[] distances = { d * 6, d * 3, d * 3 };
        Vector3[] positions = {
            transform.position - transform.up * height * 1.4f,
            transform.position - transform.up * height * 1.0f,
            transform.position - transform.up * height * 0.5f,
            transform.position + transform.up * height * 0.5f,
            transform.position + transform.up * height * 1.0f,
            transform.position + transform.up * height * 1.4f
        };

        Vector3[] directions = {
            moveDirection,
            Vector3.Cross(moveDirection, transform.up) + moveDirection,
            -Vector3.Cross(moveDirection, transform.up) + moveDirection
        };

        foreach (Vector3 direction in directions)
        {
            foreach (Vector3 origin in positions)
            {
                if (Physics.Raycast(origin, direction, out RaycastHit hit, d))
                {
                    if (hit.collider.CompareTag("Wormhole"))
                        Debug.DrawRay(origin, direction * d, Color.green);
                    else
                        return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Handles blinking effects when the player is revived.
    /// </summary>
    void Blink()
    {
        float time = Time.time - revivedTime;

        if ((time % 1) < 0.5f && !isTransparent)
        {
            SetMaterialsTranslucent();
            isTransparent = true;
        }
        else if ((time % 1) > 0.5f && isTransparent)
        {
            SetMaterialsOpaque();
            isTransparent = false;
        }

        if (time > 3.0f)
        {
            SetMaterialsOpaque();
            revived = false;
            isTransparent = false;
        }
    }

    /// <summary>
    /// Makes the player's materials translucent.
    /// </summary>
    private void SetMaterialsTranslucent() { /* Code unchanged */ }

    /// <summary>
    /// Restores the player's materials to opaque.
    /// </summary>
    private void SetMaterialsOpaque() { /* Code unchanged */ }

    /// <summary>
    /// Handles player jumping, ensuring double jumps are prevented.
    /// </summary>
    void JumpPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isGround = false;
            step.Pause();
            animator.SetBool("Jump_b", true);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    // STATE MANAGEMENT

    /// <summary>
    /// Updates the player's rotation when gravity changes.
    /// </summary>
    public void OnNotify<GravityObserver>(Quaternion rot) { /* Code unchanged */ }

    /// <summary>
    /// Updates the player's behavior based on the game state.
    /// </summary>
    public void OnNotify<GameStateObserver>(GameState gs) { /* Code unchanged */ }
}