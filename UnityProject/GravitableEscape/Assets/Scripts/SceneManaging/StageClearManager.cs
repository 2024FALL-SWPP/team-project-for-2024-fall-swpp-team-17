using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the logic for clearing a stage, including opening doors when the player triggers the stage-clear condition.
/// </summary>
/// <remarks>
/// This script interacts with <see cref="DoorMovementManager"/> to handle door movements upon stage completion.
/// </remarks>
public class StageClearManager : MonoBehaviour
{
    public DoorMovementManager leftDoorManager; // Manages the movement of the left door
    public DoorMovementManager rightDoorManager; // Manages the movement of the right door

    private bool stageCleared = false; // Tracks whether the stage has been cleared

    void Start()
    {
        // Initialize references to the door movement managers
        leftDoorManager = GameObject.Find("LeftDoor").gameObject.GetComponent<DoorMovementManager>();
        rightDoorManager = GameObject.Find("RightDoor").gameObject.GetComponent<DoorMovementManager>();
    }

    /// <summary>
    /// Opens both doors by triggering their movement logic.
    /// </summary>
    void DoorOpen()
    {
        leftDoorManager.StartMoving();
        rightDoorManager.StartMoving();
    }

    /// <summary>
    /// Detects collision with the player to trigger stage clearing and door opening.
    /// </summary>
    /// <param name="collision">The collision event data.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!stageCleared && collision.collider.CompareTag("Player"))
        {
            stageCleared = true;
            DoorOpen();
        }
    }
}