using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

/// <summary>
/// Manages mouse input for camera and player rotation.
/// Tracks mouse movement to update yaw and pitch values, which are used by other components like CameraManager and PlayerManager.
/// </summary>
public class InputManager : MonoBehaviour
{
    private float xSensitivity = 100f; // Sensitivity for horizontal mouse movement
    private float ySensitivity = 100f; // Sensitivity for vertical mouse movement

    public float yaw, pitch; // Current yaw and pitch values
    public float scrollInput; // Tracks mouse scroll wheel input

    private float mouseX, mouseY; // Mouse movement deltas
    private float savedYaw, savedPitch; // Stored yaw and pitch values for resetting

    // Start is called before the first frame update
    void Start()
    {
        yaw = 0; // Initialize yaw
        pitch = 0; // Initialize pitch
        mouseX = 0; // Reset mouse X delta
        mouseY = 0; // Reset mouse Y delta
    }

    // Update is called once per frame
    void Update()
    {
        // Fetch mouse movement and adjust yaw and pitch
        mouseX = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime; // Horizontal mouse movement
        mouseY = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime; // Vertical mouse movement

        yaw += mouseX; // Update yaw with horizontal movement
        pitch -= mouseY; // Update pitch with vertical movement (inverted)

        pitch = Mathf.Clamp(pitch, -30f, 60f); // Clamp pitch to prevent excessive vertical rotation

        // Handle temporary yaw and pitch storage when Shift key is held
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            savedYaw = yaw; // Save current yaw
            savedPitch = pitch; // Save current pitch
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            yaw = savedYaw; // Restore saved yaw
            pitch = savedPitch; // Restore saved pitch
        }

        // Fetch scroll input
        scrollInput = Input.GetAxis("Mouse ScrollWheel"); // Track mouse scroll wheel movement
    }
}