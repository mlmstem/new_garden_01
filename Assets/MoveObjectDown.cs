using System.Collections;
using UnityEngine;

public class RobotCameraMovement : MonoBehaviour
{
    public Transform target; // The robot to follow
    public float followSpeed = 5.0f; // Speed to follow the robot
    public float amplitude = 2.0f; // Amplitude of the sine wave
    public float frequency = 1.0f; // Frequency of the sine wave

    private Vector3 originalPosition;
    private float timeElapsed = 0.0f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // Follow the robot's position smoothly
        Vector3 targetPosition = target.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Perform a sine wave up and down movement
        timeElapsed += Time.deltaTime;
        float newY = originalPosition.y + amplitude * Mathf.Sin(frequency * timeElapsed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}