using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages in-game messages used in the Tutorial Scene displayed to the player based on their position in predefined zones.
/// Also handles gravity change instructions within specific ranges.
/// </summary>
/// <remarks>
/// This script is designed to work in scenes where messages are required, such as the "Tutorial" scene.
/// It interacts with the <see cref="UIManager"/> to display, update, or hide messages dynamically based on the player's z-position.
/// </remarks>
public class MessageManager : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    private UIManager uiManager; // Reference to the UI manager for message handling

    /// <summary>
    /// Represents a zone where a specific message is displayed.
    /// </summary>
    [System.Serializable]
    public class Zone
    {
        [TextArea] public string message; // Message to display in the zone
        public float startZ; // Starting z-coordinate of the zone
        public float endZ; // Ending z-coordinate of the zone
    }

    public Zone[] zones; // Array of defined zones for messages

    private string currentMessage = null; // Tracks the current active message
    private bool isDelayTriggered = false; // Prevents duplicate delayed message triggers
    private bool hasMessageShown = false; // Tracks if a zone's message has already been shown

    public float gravityMessageStartZ; // Starting z-coordinate for gravity instructions
    public float gravityMessageEndZ; // Ending z-coordinate for gravity instructions
    private bool isMessageRequiredScene; // Checks if the scene requires messages

    void Start()
    {
        // Check if the current scene requires messages
        isMessageRequiredScene = SceneManager.GetActiveScene().name == "Tutorial";

        player = GameObject.Find("Player").transform;
        uiManager = FindObjectOfType<UIManager>();

        // Validate zone boundaries
        foreach (var zone in zones)
        {
            if (zone.startZ > zone.endZ)
            {
                Debug.LogError("Invalid zone bounds: startZ is greater than endZ.");
            }
        }
    }

    void Update()
    {
        if (!isMessageRequiredScene) return;

        Vector3 playerPosition = player.position;
        float playerZ = playerPosition.z;

        string newMessage = null;

        // Determine the current zone and message
        foreach (var zone in zones)
        {
            if (playerZ >= zone.startZ && playerZ <= zone.endZ)
            {
                newMessage = zone.message;

                if (playerZ <= -80 && !isDelayTriggered && !hasMessageShown)
                {
                    isDelayTriggered = true;
                    hasMessageShown = true;
                    StartCoroutine(CallUIManagerAfterDelay(newMessage));
                }

                break;
            }
        }

        // Update the current message if it changes
        if (newMessage != currentMessage && !isDelayTriggered)
        {
            currentMessage = newMessage;

            if (string.IsNullOrEmpty(currentMessage))
            {
                uiManager.HideMessage();
                hasMessageShown = false;
            }
            else
            {
                uiManager.ShowMessage(currentMessage);
            }
        }

        // Handle gravity change instructions
        if (playerZ >= gravityMessageStartZ && playerZ <= gravityMessageEndZ)
        {
            uiManager.ShowGravityDirections();
        }
        else
        {
            uiManager.HideGravityDirections();
        }
    }

    /// <summary>
    /// Triggers the UI manager to show a message after a delay.
    /// </summary>
    /// <param name="currentMessage">The message to display.</param>
    IEnumerator CallUIManagerAfterDelay(string currentMessage)
    {
        yield return new WaitForSeconds(1.5f);
        uiManager.ShowMessage(currentMessage);
        isDelayTriggered = false;
    }
}