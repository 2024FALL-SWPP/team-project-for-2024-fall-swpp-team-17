using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
using UnityEngine.Scripting.APIUpdating;

public class PlayerManager : MonoBehaviour, GravityObserver, IPlayerManager, GameStateObserver
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    private Vector3 moveDirection;
    Quaternion targetGravityRot = Quaternion.identity; // Target rotation for gravity changes
    public InputManager inputManager;
    private float height;
    bool isGround;
    private Animator animator;
    public int life;
    Vector3 forwardDirction;
    public float jumpForce = 1200f;
    public int Life
    {
        get { return life; }
    }

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation so that Rigidbody does not control rotation
        height = GetComponent<BoxCollider>().size.y;
        isGround = true;
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        life = 5;
    }

    void Update()
    {
        // TODO: check game state
        RotatePlayer();
        JumpPlayer();
        
    }

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
        }
        else
        {
            // Stop movement if there is no input
            moveDirection = Vector3.zero;
        }

    }

    /// <summary>
    /// Check if there are obstacles and move player in the moveDirection
    /// </summary>
    void MovePlayer()
    {
        Vector3 footPosition = transform.position + Vector3.down * height / 2.5f;
        Vector3 headPosition = transform.position + Vector3.up * height / 2.5f;
        float distance = moveSpeed * Time.fixedDeltaTime * 10;
        if (!ObstacleInPath(transform.position, moveDirection, distance)
        && !ObstacleInPath(footPosition, moveDirection, distance)
        && !ObstacleInPath(headPosition, moveDirection, distance))
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            animator.SetFloat("Speed_f", moveDirection.magnitude * moveSpeed);
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
    bool ObstacleInPath(Vector3 origin, Vector3 direction, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distance)) return true;
        else return false;
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
        life += amount;
        if (life < 0){
            animator.SetBool("Death_b", true);
        }
        else if (amount < 0 && life > 0)
        {
            animator.SetBool("Faint_b", true);
            StartCoroutine(ResetFaintAnimation());
        }
    }

    private IEnumerator ResetFaintAnimation()
    {
    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // wait for animation playtime
    animator.SetBool("Faint_b", false); // reset faint
    }

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
        switch (gs)
        {
            case GameState.Playing:
                gameObject.SetActive(true);
                break;
            case GameState.WormholeEffect:
                gameObject.SetActive(false);
                break;
            case GameState.Fainted:
                gameObject.SetActive(true);
                break;
        }
    }

}