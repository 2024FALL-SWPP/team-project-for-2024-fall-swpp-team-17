using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;

/// <summary>
/// Manages the logic for a box puzzle, including the activation of boxes, locks, and plates.
/// Handles puzzle progression, resetting, and clearing.
/// </summary>
/// <remarks>
/// This class implements <see cref="PuzzleInterface"/> and provides functionality for:
/// - Tracking the positions of keyboxes and resetting them.
/// - Activating locks and plates based on player interaction.
/// - Clearing the puzzle and triggering corresponding visual effects.
/// </remarks>
public class BoxPuzzleManager : MonoBehaviour, PuzzleInterface
{
    public GameObject[] keyboxes; // Array of keyboxes involved in the puzzle
    public GameObject[] locks; // Array of locks to be unlocked
    public GameObject[] plates; // Array of plates to activate upon unlocking

    private Vector3[] startPoses = new Vector3[7]; // Initial positions of the keyboxes
    private Vector3 targetRot = new Vector3(0f, 90f, 0f); // Rotation for puzzle clear effect
    private float rotationSpeed = 30f; // Speed of rotation when the puzzle is cleared
    private bool isCleared = false; // Indicates if the puzzle is cleared
    private int unlockCount = 0; // Tracks the number of locks unlocked

    public ButtonManager buttonManager; // Reference to the button manager

    void Start()
    {
        buttonManager = GameObject.Find("Button").GetComponent<ButtonManager>();
        GetBoxPosition();
        SetBoxesInActive();
        SetLockIDs();
        SetPlatesInActive();
    }

    void Update()
    {
        if (isCleared)
        {
            // Rotate the puzzle object upon clearing
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRot), rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Receives a signal to unlock a specific lock and activates its corresponding plate.
    /// </summary>
    /// <param name="lockID">The ID of the lock to unlock.</param>
    public void GetUnlockSignal(int lockID)
    {
        plates[lockID].SetActive(true);
        IncreaseUnlockCount();
    }

    /// <summary>
    /// Assigns unique IDs to each lock in the puzzle.
    /// </summary>
    void SetLockIDs()
    {
        int lockID = 0;
        foreach (GameObject puzzleLock in locks)
        {
            LockManager lockManager = puzzleLock.GetComponent<LockManager>();
            lockManager.SetLockID(lockID);
            lockID++;
        }
    }

    /// <summary>
    /// Activates all keyboxes in the puzzle.
    /// </summary>
    void SetBoxesActive()
    {
        foreach (GameObject box in keyboxes)
        {
            box.SetActive(true);
        }
    }

    /// <summary>
    /// Records the initial positions of all keyboxes.
    /// </summary>
    void GetBoxPosition()
    {
        for (int index = 0; index < 7; index++)
        {
            startPoses[index] = keyboxes[index].transform.position;
        }
    }

    /// <summary>
    /// Resets all keyboxes to their initial positions.
    /// </summary>
    void SetBoxAtStartPos()
    {
        for (int index = 0; index < 7; index++)
        {
            keyboxes[index].transform.position = startPoses[index];
        }
    }

    /// <summary>
    /// Deactivates all keyboxes.
    /// </summary>
    void SetBoxesInActive()
    {
        foreach (GameObject box in keyboxes)
        {
            box.SetActive(false);
        }
    }

    /// <summary>
    /// Deactivates all plates.
    /// </summary>
    void SetPlatesInActive()
    {
        foreach (GameObject plate in plates)
        {
            plate.SetActive(false);
        }
    }

    /// <summary>
    /// Increments the unlock count and clears the puzzle if all locks are unlocked.
    /// </summary>
    void IncreaseUnlockCount()
    {
        unlockCount++;
        if (unlockCount == 7)
        {
            PuzzleClear();
        }
    }

    /// <summary>
    /// Starts the puzzle by activating all keyboxes.
    /// </summary>
    public void PuzzleStart()
    {
        SetBoxesActive();
    }

    /// <summary>
    /// Resets the puzzle to its initial state.
    /// </summary>
    public void PuzzleReset()
    {
        StartCoroutine(ResetBox());
    }

    /// <summary>
    /// Clears the puzzle and fixes the button state.
    /// </summary>
    public void PuzzleClear()
    {
        buttonManager.FixButton();
        isCleared = true;
    }

    /// <summary>
    /// Coroutine to reset the keyboxes and plates to their initial states.
    /// </summary>
    private IEnumerator ResetBox()
    {
        SetBoxesInActive();
        SetPlatesInActive();
        yield return new WaitForSeconds(0.5f);
        SetBoxAtStartPos();
        SetBoxesActive();
    }
}