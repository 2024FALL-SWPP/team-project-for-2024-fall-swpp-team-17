using UnityEngine;

public class PlayerFloat : MonoBehaviour
{
    private Vector3 boundsMin = new Vector3(-80f, -50f, 110f); // Minimum bounds
    private Vector3 boundsMax = new Vector3(80f, 55f, 110f);  // Maximum bounds
    private Vector3 initialVelocity = new Vector3(20f, 20f, 0f); // Initial velocity
    private Vector3 velocity;
    private Vector3 rotationVelocity;

    private void Start()
    {
        // Set the initial velocity
        velocity = initialVelocity;
        SetRandomRotation();
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

        transform.Rotate(rotationVelocity * Time.deltaTime);


    }

    private void OnMouseDown()
    {
        SetRandomRotation();
    }


    private void SetRandomRotation()
    {
        float maxRotationSpeed = 150f;
        rotationVelocity = new Vector3(
            Random.Range(-maxRotationSpeed, maxRotationSpeed),
            Random.Range(-maxRotationSpeed, maxRotationSpeed),
            Random.Range(-maxRotationSpeed, maxRotationSpeed)
        );
    }
}