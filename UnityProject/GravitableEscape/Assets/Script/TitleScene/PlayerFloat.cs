using UnityEngine;

public class PlayerFloat : MonoBehaviour
{
    public Vector3 boundsMin = new Vector3(-5f, -5f, 0f); // Minimum bounds
    public Vector3 boundsMax = new Vector3(5f, 5f, 0f);  // Maximum bounds
    public Vector3 initialVelocity = new Vector3(2f, 2f, 0f); // Initial velocity
    private Vector3 velocity;
    private Transform cameraTransform;

    private void Start()
    {
        // Set the initial velocity
        velocity = initialVelocity;
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Move the player
        transform.position += velocity * Time.deltaTime;

        // Check for collisions with the bounds
        if (transform.position.x <= boundsMin.x || transform.position.x >= boundsMax.x)
        {
            velocity.x = -velocity.x; // Reverse and dampen velocity on X-axis
            velocity.x = Mathf.Sign(velocity.x) * Mathf.Max(Mathf.Abs(velocity.x), Mathf.Abs(initialVelocity.x)); // Prevent stopping
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, boundsMin.x, boundsMax.x),
                transform.position.y,
                transform.position.z
            );
        }

        if (transform.position.y <= boundsMin.y || transform.position.y >= boundsMax.y)
        {
            velocity.y = -velocity.y; // Reverse and dampen velocity on Y-axis
            velocity.y = Mathf.Sign(velocity.y) * Mathf.Max(Mathf.Abs(velocity.y), Mathf.Abs(initialVelocity.y)); // Prevent stopping
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y, boundsMin.y, boundsMax.y),
                transform.position.z
            );
        }

        if (transform.position.z <= boundsMin.z || transform.position.z >= boundsMax.z)
        {
            velocity.z = -velocity.z; // Reverse and dampen velocity on Z-axis
            velocity.z = Mathf.Sign(velocity.z) * Mathf.Max(Mathf.Abs(velocity.z), Mathf.Abs(initialVelocity.z)); // Prevent stopping
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                Mathf.Clamp(transform.position.z, boundsMin.z, boundsMax.z)
            );
        }
        // transform.LookAt(cameraTransform);
    }
}