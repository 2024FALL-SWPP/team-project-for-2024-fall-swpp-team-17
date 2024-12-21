using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;

/// <summary>
/// Manages the behavior of a lock within a puzzle.
/// Handles unlocking logic when the correct key collides with the lock
/// and sends a signal to the puzzle interface upon unlocking.
/// </summary>
/// <remarks>
/// Each lock has a unique ID assigned via <see cref="SetLockID"/> and interacts with the
/// <see cref="PuzzleInterface"/> to notify the puzzle of unlock events.
/// </remarks>
public class LockManager : MonoBehaviour
{
    private PuzzleInterface puzzleInterface; // Reference to the associated puzzle interface
    public GameObject puzzle; // The puzzle GameObject this lock is part of
    private bool isUnlocked = false; // Tracks whether the lock is unlocked
    private int lockID = -1; // Unique identifier for the lock

    void Start()
    {
        // Initialize the puzzle interface reference
        puzzleInterface = puzzle.GetComponent<PuzzleInterface>();
    }

    /// <summary>
    /// Assigns a unique ID to the lock.
    /// </summary>
    /// <param name="idNumber">The ID number to assign.</param>
    public void SetLockID(int idNumber)
    {
        lockID = idNumber;
    }

    /// <summary>
    /// Checks for collisions with the key object and unlocks the lock if conditions are met.
    /// </summary>
    /// <param name="collision">The collision event data.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("key") && !isUnlocked)
        {
            Unlock();
        }
    }

    /// <summary>
    /// Unlocks the lock and sends an unlock signal to the puzzle interface.
    /// </summary>
    private void Unlock()
    {
        isUnlocked = true;
        puzzleInterface.GetUnlockSignal(lockID);
    }
}