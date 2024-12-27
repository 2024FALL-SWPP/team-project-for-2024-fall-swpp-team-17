using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.Scripting.APIUpdating;
using System.Xml.Serialization;

/// <summary>
/// Handles player movement, jumping, life management, and visual effects like blinking during revival.
/// </summary>
public class PlayerManager : MonoBehaviour, GravityObserver, GameStateObserver
{
    public Rigidbody rb;
    private Animator animator;
    public Renderer[] renderers;
    public InputManager inputManager;
    public float jumpForce = 1500f;
    public float moveSpeed = 20f;
    private Vector3 moveDirection;
    private Quaternion targetGravityRot = Quaternion.identity; // Player's forward direction relative to gravity
    public BoxCollider boxCollider;
    private float height;
    private bool isGround;
    private GameState gameState;
    public float revivedTime = -100f;
    public bool revived = false;
    public bool isTransparent = false;

    private AudioSource[] audioSources;
    public AudioSource landing;
    public AudioSource step;
    public AudioSource pipe;

    /// <summary>
    /// Initializes components and sets default values for the player.
    /// </summary>
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics-based rotation
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.size.y;
        isGround = true;
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        renderers = GetComponentsInChildren<Renderer>();
        audioSources = GetComponents<AudioSource>();
        landing = audioSources[0];
        step = audioSources[1];
        pipe = audioSources[2];
    }

    /// <summary>
    /// Updates player state and executes actions based on the current game state.
    /// </summary>
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
            default:
                break;
        }
    }

    /// <summary>
    /// Handles physics-based movement updates for the player.
    /// </summary>
    void FixedUpdate()
    {
        if (gameState == GameState.Playing || gameState == GameState.Revived)
        {
            MovePlayer();
        }
    }

    /// <summary>
    /// Rotates the player to align with movement direction.
    /// </summary>
    void RotatePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
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
    /// Moves the player in the current direction while avoiding obstacles.
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
    /// Checks if there are obstacles in the player's path.
    /// </summary>
    /// <returns>True if there is an obstacle, otherwise false.</returns>
    bool ObstacleInPath()
    {
        float d = moveSpeed * Time.fixedDeltaTime;
        float[] distances = { d * 10, d * 5, d * 5 };
        Vector3 footPosition = transform.position - transform.up * height * 1.4f;

        Vector3[] positions = { transform.position, footPosition };
        Vector3[] directions = { moveDirection };

        foreach (Vector3 origin in positions)
        {
            foreach (Vector3 direction in directions)
            {
                if (Physics.Raycast(origin, direction, distances[0]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Handles blinking effects to indicate revival.
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
    /// Sets the player's materials to translucent.
    /// </summary>
    private void SetMaterialsTranslucent()
    {
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.renderQueue = 3000;

            Color color = material.color;
            color.a = 0.7f;
            renderer.material.color = color;
        }
    }

    /// <summary>
    /// Resets the player's materials to opaque.
    /// </summary>
    private void SetMaterialsOpaque()
    {
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            material.SetFloat("_Mode", 0);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.renderQueue = -1;
        }
    }

    /// <summary>
    /// Makes the player jump when grounded.
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

    /// <summary>
    /// Handles collision detection and updates ground state.
    /// </summary>
    /// <param name="collision">Collision details.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Wall"))
        {
            if (!isGround)
            {
                landing.Play();
            }
            isGround = true;
            animator.SetBool("Jump_b", false);
        }
    }

    /// <summary>
    /// Teleports the player to a target position.
    /// </summary>
    /// <param name="targetPos">The target position.</param>
    public void Teleport(Vector3 targetPos)
    {
        transform.position = targetPos;
        transform.rotation = targetGravityRot;
    }

    /// <summary>
    /// Updates the player's rotation based on gravity changes.
    /// </summary>
    /// <param name="rot">Gravity rotation.</param>
    public void OnNotify<GravityObserver>(Quaternion rot)
    {
        targetGravityRot *= rot;
        transform.rotation = targetGravityRot;
    }

    /// <summary>
    /// Updates the player's behavior based on game state changes.
    /// </summary>
    /// <param name="gs">Game state.</param>
    public void OnNotify<GameStateObserver>(GameState gs)
    {
        gameState = gs;
        switch (gs)
        {
            case GameState.WormholeEffect:
                gameObject.SetActive(false);
                break;
            case GameState.Gameover:
                animator.SetBool("Death_b", true);
                break;
            case GameState.Stun:
                animator.SetBool("Faint_b", true);
                StartCoroutine(ResetStunAnimation());
                break;
            case GameState.Revived:
                revivedTime = Time.time;
                break;
            default:
                gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Resets the player's "stun" animation after a delay.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator ResetStunAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Faint_b", false);
    }

    /// <summary>
    /// Gets the player's current position.
    /// </summary>
    /// <returns>Player's position as a Vector3.</returns>
    public Vector3 GetPlayerPos()
    {
        return transform.position;
    }

}