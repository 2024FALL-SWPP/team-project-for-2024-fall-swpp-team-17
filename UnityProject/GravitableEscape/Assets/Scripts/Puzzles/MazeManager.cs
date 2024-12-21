using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;
/// <summary>
/// Manages the logic for a maze puzzle, including key box activation, resetting, and puzzle completion.
/// </summary>
/// <remarks>
/// Implements <see cref="PuzzleInterface"/> to handle puzzle lifecycle events such as start, reset, and clear.
/// The puzzle involves activating a key box and handling its state until the puzzle is cleared.
/// </remarks>
public class MazeManager : MonoBehaviour, PuzzleInterface
{
    public GameObject keyBox; // The key box used in the maze puzzle
    private Vector3 startPos; // The initial position of the key box
    private Vector3 targetRot = new Vector3(0f, 90f, 0f); // Target rotation for visual effect on puzzle clear
    private float rotationSpeed = 30f; // Speed of rotation during puzzle clear effect
    private bool isCleared = false; // Tracks if the puzzle is cleared

    public ButtonManager buttonManager; // Reference to the button manager

    void Start()
    {
        // Initialize references and deactivate the key box
        buttonManager = GameObject.Find("Button").GetComponent<ButtonManager>();
        Transform keyBoxTransform = transform.Find("keybox");
        keyBox = keyBoxTransform.gameObject;
        startPos = keyBoxTransform.position;
        keyBox.SetActive(false);
    }

    void Update()
    {
        if (isCleared)
        {
            // Rotate the maze when the puzzle is cleared
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRot), rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Starts the puzzle by activating the key box.
    /// </summary>
    public void PuzzleStart()
    {
        keyBox.SetActive(true);
    }

    /// <summary>
    /// Resets the puzzle by deactivating and repositioning the key box.
    /// </summary>
    public void PuzzleReset()
    {
        StartCoroutine(ResetBox());
    }

    /// <summary>
    /// Handles unlocking signals from the puzzle and triggers puzzle clearance.
    /// </summary>
    /// <param name="lockID">The ID of the lock being unlocked (not used in this implementation).</param>
    public void GetUnlockSignal(int lockID)
    {
        PuzzleClear();
    }

    /// <summary>
    /// Clears the puzzle, fixes the button state, and marks the puzzle as completed.
    /// </summary>
    public void PuzzleClear()
    {
        buttonManager.FixButton();
        isCleared = true;
    }

    /// <summary>
    /// Coroutine to reset the key box to its initial state.
    /// </summary>
    private IEnumerator ResetBox()
    {
        keyBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        keyBox.transform.position = startPos;
        keyBox.SetActive(true);
    }
}