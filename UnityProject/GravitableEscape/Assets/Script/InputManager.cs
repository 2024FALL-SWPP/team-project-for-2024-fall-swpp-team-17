using System.Collections;
using System.Collections.Generic;
using OurGame;
using UnityEngine;

public class InputManager : MonoBehaviour, GravityObserver
{
    private float mouseSensitivity = 200f;
    public float yaw, pitch, savedYaw, savedPitch;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Don't show mouse
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

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
    }

    public void OnNotify(Quaternion rot)
    {
        yaw = 0;
        pitch = 0;
    }
}
