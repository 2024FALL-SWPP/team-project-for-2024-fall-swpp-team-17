using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.Scripting.APIUpdating;
using System.Xml.Serialization;

/// <summary>
/// This class handles movement(+ jump), life, blinking(when revived)
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
    private Quaternion targetGravityRot = Quaternion.identity; // Target rotation for gravity changes. Direction of player facing forward in gravity
    public BoxCollider boxCollider;
    private float height;
    private bool isGround;
    GameState gameState;
    public float revivedTime = -100f;
    public bool revived = false;
    public bool isTransparent = false;

    private AudioSource[] audioSources;
    public AudioSource landing;
    public AudioSource step;
    public AudioSource pipe;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation so that Rigidbody does not control rotation
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
                break;
            case GameState.Stun:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Moving player is here to prevent going through walls
    /// </summary>
    void FixedUpdate()
    {
        switch (gameState)
        {
            case GameState.Playing:
            case GameState.Revived:
                MovePlayer();
                break;
            default:
                break;
        }
    }


    // PLAYER BASIC MOVEMENT SCRIPTS
    // Rotate, Move, Blink, Jump

    // ROTATE
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

            step.Pause();
        }
    }

    // MOVE
    /// <summary>
    /// Check if there are obstacles and move player in the moveDirection
    /// </summary>
    void MovePlayer()
    {
        if (!ObstacleInPath())
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            animator.SetFloat("Speed_f", moveDirection.magnitude * moveSpeed);
            if (!step.isPlaying && moveDirection.magnitude > 0.1f)
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
    /// Checks if there is an obstacle from origin, in direction, within distance
    /// </summary>
    /// <param name="origin">position of origin(player)</param>
    /// <param name="direction">direction to check if there is an obstacle</param>
    /// <param name="distance">distance(bound) to check if there is an obstacle</param>
    /// <returns></returns>
    bool ObstacleInPath()
    {
        float d = moveSpeed * Time.fixedDeltaTime;
        float[] distances = new float[] { d * 10, d * 3, d * 3 };
        Vector3 footPosition = transform.position - transform.up * height;
        Vector3 headPosition = transform.position + transform.up * height;
        Vector3[] positions = new Vector3[] { transform.position, footPosition, headPosition };
        Vector3 leftDirection = Vector3.Cross(moveDirection, transform.up);
        Vector3 rightDirection = -Vector3.Cross(moveDirection, transform.up);
        Vector3[] directions = new Vector3[] { moveDirection, leftDirection, rightDirection };
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 direction = directions[i];
            float distance = distances[i];
            for (int j = 0; j < positions.Length; j++)
            {
                Vector3 origin = positions[j];
                RaycastHit hit;
                if (Physics.Raycast(origin, direction, out hit, distance))
                {
                    if (hit.collider.gameObject.tag == "Wormhole")
                    {
                        Debug.DrawRay(origin, direction * distance, Color.green); // DEBUG
                    }
                    else
                    {
                        Debug.DrawRay(origin, direction * distance, Color.red); // DEBUG
                        return true;
                    }
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.green); // DEBUG
                }
            }
        }
        return false;
    }
    // BLINK
    /// <summary>
    /// Make player blink translucent, opaque to show that player revived
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

    private void SetMaterialsTranslucent()
    {
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            material.SetFloat("_Mode", 3); // Transparent mode
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            Color color = material.color;
            color.a = 0.7f;
            renderer.material.color = color;
        }
    }

    private void SetMaterialsOpaque()
    {
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            material.SetFloat("_Mode", 0); // Opaque ���
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }
    }

    // JUMP
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
    /// check if ground is touched, update isGround
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Wall"))
        {
            if (!isGround)
            {
                if (collision.gameObject.name.StartsWith("Pipe"))
                {
                    pipe.Play();
                }
                else
                {
                    landing.Play();
                }
            }
            isGround = true;
            animator.SetBool("Jump_b", false);

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

    /// <summary>
    /// SetActive to false when player is spiraling towards wormhole
    /// </summary>
    /// <typeparam name="GameStateObserver"></typeparam>
    /// <param name="gs">global game state after modification</param>
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
                // animator.SetBool("Faint_b", false);
                break;
            default:
                gameObject.SetActive(true);
                break;
        }
    }
    private IEnumerator ResetStunAnimation()
    {
        yield return new WaitForSeconds(1.5f); // Even though gameState is stil Stun, want player to stand up little earlier
        animator.SetBool("Faint_b", false); // reset faint
    }


    /// <summary>
    /// Returns player's position.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerPos()
    {
        return transform.position;
    }

}