using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the movement of a door, allowing it to slide open or close in a specified direction.
/// </summary>
/// <remarks>
/// The door moves in a horizontal direction (left or right) based on its current position
/// and stops when it reaches the target position.
/// </remarks>
public class DoorMovementManager : MonoBehaviour
{
    private Vector3 targetPos; // The target position for the door's movement
    private Vector3 moveDirection; // The direction in which the door moves
    private float moveSpeed = 15f; // Speed at which the door moves
    private float distance = 15f; // Distance the door moves from its starting position
    private bool isMoving = false; // Indicates if the door is currently moving

    void Update()
    {
        if (isMoving)
        {
            // Move the door towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Stop moving when the target position is reached
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// Starts the door's movement in the appropriate horizontal direction.
    /// </summary>
    public void StartMoving()
    {
        // Determine the direction of movement based on the door's current position
        if (transform.position.x > 0)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
        }

        // Calculate the target position and begin movement
        targetPos = transform.position + moveDirection.normalized * distance;
        isMoving = true;
    }
}