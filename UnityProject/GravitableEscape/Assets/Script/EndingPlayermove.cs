using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayermove : MonoBehaviour
{
    public RuntimeAnimatorController endingController;
    public RuntimeAnimatorController simpleCharacterController;
    private Animator animator;
    public float moveSpeed = 2f;
    private bool isMovingLeft = true;
    private bool isMoving = false;

    private Vector3 leftTargetPosition;
    private Vector3 rightTargetPosition;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = endingController;
        leftTargetPosition = transform.position + Vector3.left * 100f;
        rightTargetPosition = transform.position;
        transform.rotation = Quaternion.Euler(0, 150, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            StartCoroutine(MoveAndPlayEnding());
        }
    }

    IEnumerator MoveAndPlayEnding()
    {
        isMoving = true;

        Vector3 targetPosition = isMovingLeft ? leftTargetPosition : rightTargetPosition;
        FlipPlayer(isMovingLeft);

        animator.runtimeAnimatorController = simpleCharacterController;
        animator.SetFloat("Speed_f", 0.5f);

        while (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        SetArrivalRotation();
        animator.SetFloat("Speed_f", 0f);

        animator.runtimeAnimatorController = endingController;

        isMovingLeft = !isMovingLeft;

        isMoving = false;
    }

    void FlipPlayer(bool moveLeft)
    {
        if (moveLeft)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    void SetArrivalRotation()
    {
        if (isMovingLeft)
        {
            transform.rotation = Quaternion.Euler(0, 210, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 150, 0);
        }
    }
}
