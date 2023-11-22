using System.Collections;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public Transform startPosition; // Initial position
    public Transform endPosition;   // Final position
    public float movementSpeed = 1.0f; // Speed at which the robot moves
    public float delayTime = 1.0f; // Delay time between movements

    private void Start()
    {
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