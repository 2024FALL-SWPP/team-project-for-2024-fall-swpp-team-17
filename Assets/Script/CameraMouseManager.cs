using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class manages the camera's rotation due to the mouse movement.
/// It resets its rotation if the mouse left button is pressed.
/// </summary>
public class CameraMouseManager : MonoBehaviour
{
    private float mouseRotX, mouseRotY;
    private float sensitivity = 300f;
    private float clampAngleY = 70f;
    void Start()
    {
        mouseRotX = 0;
        mouseRotY = 0;
    }
    /// <summary>
    /// This function resets the rotation if mouse left button is clicked.
    /// Also, it retrieves the mouse's location and alters the localRotation of the Main Camera accordingly.
    /// Since the Main Camera object is a child of the CameraManager object, 
    /// the final rotation of the Main Camera object is the addition of its localRotation and the CameraManager's rotation.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseRotX = 0;
            mouseRotY = 0;
        }

        mouseRotX += -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        mouseRotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        float totalRotX = Mathf.Clamp(mouseRotX, -clampAngleY, clampAngleY);
        float totalRotY = mouseRotY;
        transform.localRotation = Quaternion.Euler(totalRotX, totalRotY, 0);
    }
}
