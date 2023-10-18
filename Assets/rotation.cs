using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float swingAmplitude = 30.0f; // Swing amplitude in degrees
    public float swingSpeed = 500.0f; // Swing speed in degrees per second

    private float timeElapsed = 0.0f;

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Calculate the rotation angle using a sine wave for smooth motion
        float rotationAngle = Mathf.Sin(swingSpeed * timeElapsed * Mathf.Deg2Rad) * swingAmplitude;

        // Apply the rotation
        transform.localRotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
}
