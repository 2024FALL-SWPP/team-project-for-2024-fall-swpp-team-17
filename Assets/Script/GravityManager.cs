using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages the gravity of the overall game.
/// When number keys 1, 2, 3 are pushed, the GravityManager object rotates appropriately.
/// Objects that need to change direction to align with gravity (player, camera, etc.) 
/// changes its direction by reffering to this object's rotation.
/// </summary>
public class GravityManager : MonoBehaviour
{
    public Vector3 initGravity = new Vector3(0, -35f, 0);
    private Rigidbody[] rigidbodies;
    CameraManager cameraManager;
    PlayerManager playerManager;
    void Start()
    {
        rigidbodies = FindObjectsOfType<Rigidbody>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        Physics.gravity = initGravity;
    }
    /// <summary>
    /// This function checks if every rigidbody object is at rest, 
    /// and if so, alters gravity according to the input key.
    /// </summary>
    void Update()
    {
        if (AllRest())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) RotateAngle(-90);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) RotateAngle(-180);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) RotateAngle(-270);
        }
    }
    /// <summary>
    /// This function rotates this object, Physics.gravity, camera, player.
    /// </summary>
    /// <param name="angle">angle to rotate</param>
    void RotateAngle(int angle)
    {
        transform.Rotate(0, 0, angle, Space.World);
        Physics.gravity = Quaternion.Euler(0, 0, angle) * Physics.gravity;
        cameraManager.CameraRot();
        playerManager.PlayerRot();
    }
    /// <summary>
    /// This function checks whether all rigidbodies are at rest.
    /// </summary>
    /// <returns>true if all rigidbodies are at rest, false if not</returns>
    private bool AllRest()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb.velocity.magnitude > 0.1f) return false;
        }
        return true;
    }

}
