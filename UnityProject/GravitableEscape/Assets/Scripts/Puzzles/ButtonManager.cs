using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGame;

/// <summary>
/// Manages the behavior of a button that is used to activate a puzzle.
/// Handles player interactions, button states, and puzzle activation or resetting.
/// </summary>
/// <remarks>
/// The button:
/// - Changes appearance when pressed (red/green states).
/// - Triggers the puzzle to start on the first long press.
/// - Resets the puzzle on subsequent long presses.
/// - Disables interaction once the puzzle is cleared.
/// </remarks>
public class ButtonManager : MonoBehaviour
{
    private float pressTime = 1f; // Required press duration to trigger the puzzle
    private float timer = 0f; // Tracks how long the button has been pressed

    private bool isPressed = false; // Indicates if the button is currently pressed
    private bool isCleared = false; // Indicates if the puzzle is cleared
    private bool firstActivation = true; // Tracks if the puzzle has been activated for the first time

    private Vector3 direction; // Direction vector from button to player
    private float heightDifference; // Vertical alignment between button and player

    private Transform redButton; // Reference to the red button visual
    private Transform greenButton; // Reference to the green button visual

    private PuzzleInterface puzzleInterface; // Reference to the associated puzzle

    public GameObject puzzle; // The puzzle GameObject linked to this button

    void Start()
    {
        // Initialize references to button visuals and puzzle interface
        redButton = transform.Find("redbutton");
        greenButton = transform.Find("greenbutton");
        puzzleInterface = puzzle.GetComponent<PuzzleInterface>();
    }

    void Update()
    {
        if (!isCleared)
        {
            if (isPressed)
            {
                timer += Time.deltaTime;
                ButtonPressed();

                if (timer > pressTime)
                {
                    EnoughPress();
                }
            }
            else
            {
                ButtonReleased();
                timer = 0f;
            }
        }
    }

    /// <summary>
    /// Handles logic when the button has been pressed long enough.
    /// Starts the puzzle on the first activation or resets it on subsequent activations.
    /// </summary>
    void EnoughPress()
    {
        if (firstActivation)
        {
            puzzleInterface.PuzzleStart();
            firstActivation = false;
        }
        else
        {
            puzzleInterface.PuzzleReset();
        }

        timer = 0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isCleared && collision.collider.CompareTag("Player") && IsPlayerUpward(collision))
        {
            isPressed = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isCleared && collision.collider.CompareTag("Player"))
        {
            isPressed = false;
        }
    }

    /// <summary>
    /// Determines if the player is standing on the button (upward collision).
    /// </summary>
    /// <param name="collision">The collision data.</param>
    /// <returns>True if the player is above the button, otherwise false.</returns>
    private bool IsPlayerUpward(Collision collision)
    {
        direction = collision.collider.transform.position - transform.position;
        heightDifference = Vector3.Dot(direction.normalized, Vector3.up);
        return heightDifference > 0.5f;
    }

    /// <summary>
    /// Changes the button to its pressed state (green).
    /// </summary>
    void ButtonPressed()
    {
        redButton.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Changes the button to its released state (red).
    /// </summary>
    void ButtonReleased()
    {
        redButton.gameObject.SetActive(true);
        greenButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fixes the button in its pressed state and disables further interaction.
    /// </summary>
    public void FixButton()
    {
        isCleared = true;
        ButtonPressed();
    }
}