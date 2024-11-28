using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovementManager : MonoBehaviour
{
    private Vector3 targetPos;
    private Vector3 moveDirection;
    private float moveSpeed = 15f;
    private float distance = 15f;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    public void StartMoving()
    {
        if (transform.position.x > 0)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
        }
        targetPos = transform.position + moveDirection.normalized * distance;
        isMoving = true;
    }
}
