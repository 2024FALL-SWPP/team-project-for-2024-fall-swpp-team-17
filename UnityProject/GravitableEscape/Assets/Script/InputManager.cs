using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

/// <summary>
/// This class handles mouse location input.
/// It fetches mouse's x, y location and 
/// CameraManager, PlayerManager uses this script's yaw, pitch to 
/// </summary>
public class InputManager : MonoBehaviour, GravityObserver
{
    private float mouseSensitivity = 200f;
    public float yaw, pitch, scrollInput;
    private float mouseX, mouseY, savedYaw, savedPitch;

    // Start is called before the first frame update
    void Start()
    {
        yaw = 0;
        pitch = 0;
        mouseX = 0;
        mouseY = 0;
        Cursor.lockState = CursorLockMode.Locked; // Don't show mouse
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Fix for first 1 sec?
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -30f, 60f); // limit vertical rotation

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            savedYaw = yaw;
            savedPitch = pitch;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            yaw = savedYaw;
            pitch = savedPitch;
        }

        scrollInput = Input.GetAxis("Mouse ScrollWheel");
    }

    /// <summary>
    /// Resets pitch, yaw to 0 when gravity changes
    /// </summary>
    /// <typeparam name="GravityObserver"></typeparam>
    /// <param name="rot"></param>
    public void OnNotify<GravityObserver>(Quaternion rot)
    {
        yaw = 0;
        pitch = 0;
    }
}
