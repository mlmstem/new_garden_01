using UnityEngine;

public class DisableForcesAndCollisions : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        
        // Disable gravity
        rb.useGravity = false;

        // Disable any existing forces
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Disable collisions
        DisableCollisions();
    }

    void DisableCollisions()
    {
        // Get all colliders attached to the object
        Collider[] colliders = GetComponentsInChildren<Collider>();

        // Disable collisions for each collider
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
