using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roboticArmMovement : MonoBehaviour
{   
    
    public Transform startPosition; // Initial position
    public Transform endPosition;   // Final position
    public float movementSpeed = 1.0f; // Speed at which the robot moves
    public float delayTime = 1.0f; // Delay time between movements
    public GameObject cameraSensor; // The CameraSensor GameObject
    public float amplitude = 1.0f; // Amplitude of the sine wave for CameraSensor
    public float frequency = 1.0f; // Frequency of the sine wave for CameraSensor

    private Vector3 originalPosition;
    private float timeElapsed = 0.0f;

    private Vector3 originalCameraSensorPosition;
    private float cameraSensorTimeElapsed = 0.0f;

    private void Start()
    {
        originalPosition = transform.position;
        originalCameraSensorPosition = cameraSensor.transform.position;
        StartCoroutine(MoveRobot());
    }

    private IEnumerator MoveRobot()
    {
        while (true)
        {
            float journeyLength = Vector3.Distance(startPosition.position, endPosition.position);
            float startTime = Time.time;

            while (Time.time - startTime < 15.0f) // Move for 15 seconds
            {
                float distanceCovered = (Time.time - startTime) * movementSpeed;
                float fractionOfJourney = distanceCovered / journeyLength;

                transform.position = Vector3.Lerp(startPosition.position, endPosition.position, fractionOfJourney);

                // Perform a sine wave movement for CameraSensor
                cameraSensorTimeElapsed += Time.deltaTime;
                float newY = originalCameraSensorPosition.y + amplitude * Mathf.Sin(frequency * cameraSensorTimeElapsed);
                cameraSensor.transform.position = new Vector3(cameraSensor.transform.position.x, newY, cameraSensor.transform.position.z);

                yield return null;
            }

            // Wait for the specified delay time
            yield return new WaitForSeconds(delayTime);

            // Reverse the movement by swapping the start and end positions
            var temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;

            // Reset the timer
            startTime = Time.time;
        }
    }
}
