using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.Scripting.APIUpdating;
using System.Xml.Serialization;

/// <summary>
/// This class handles movement(+ jump), life, blinking(when revived)
/// </summary>
public class PlayerManager : MonoBehaviour, GravityObserver, IPlayerManager, GameStateObserver
{
    public Rigidbody rb;
    private Animator animator;
    public Renderer[] renderers;
    public InputManager inputManager;
    public float jumpForce = 1200f;
    public float moveSpeed = 20f;
    private Vector3 moveDirection;
    private Quaternion targetGravityRot = Quaternion.identity; // Target rotation for gravity changes. Direction of player facing forward in gravity
    public BoxCollider boxCollider;
    private float height;
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
        rb.freezeRotation = true; // Freeze rotation so that Rigidbody does not control rotation
        boxCollider = GetComponent<BoxCollider>();
        height = boxCollider.size.y;
        isGround = true;
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        life = 5;
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
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
            Blink();
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
        float[] distances = new float[] { d * 10, d * 5, d * 5 };
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
                    Debug.DrawRay(origin, direction * distance, Color.red); // DEBUG
                    return true;
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
        float time = Time.time - lastDamageTime;
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
            material.SetFloat("_Mode", 0); // Opaque 모드
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
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            animator.SetBool("Jump_b", false);
        }
    }

    // CALLED BY OTHER SCRIPTS
    /// <summary>
    /// Called by obstacles, energy boosters? to modify life
    /// </summary>
    /// <param name="amount">if positive life is increased, if negative life is decreased</param>
    public void ModifyLife(int amount)
    {
        if (!revived)
        {
            life += amount;
            if (amount < 0)
            {
                animator.SetBool("Faint_b", true);
                StartCoroutine(ResetFaintAnimation());
            }
        }

        if (life < 0)
        {
            life = 0;
            animator.SetBool("Death_b", true);
        }
    }

    private IEnumerator ResetFaintAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // wait for animation playtime
        animator.SetBool("Faint_b", false); // reset faint
        lastDamageTime = Time.time;
        revived = true;
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
            default:
                gameObject.SetActive(true);
                break;
        }
    }

}