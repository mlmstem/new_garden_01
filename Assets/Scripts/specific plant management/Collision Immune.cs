using UnityEngine;

public class CollisionImmune : MonoBehaviour
{
    // List of tags to ignore collisions with and destroy on collision
    public string[] immuneTags = { "apple", "tomato", "cabbage", "eggplant", "chili" };

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a tag to be ignored
        if (ArrayContains(immuneTags, collision.gameObject.tag))
        {
            // // Disable the collider of the object with this script
            // GetComponent<Collider>().enabled = false;

            // // Destroy the object with this script after a short delay (you can adjust the delay value)
            // Destroy(gameObject, 0.1f);

            // Ignore all collisions involving this collider
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }

    // Helper method to check if an array contains a specific value
    private bool ArrayContains(string[] array, string value)
    {
        foreach (string item in array)
        {
            if (item == value)
            {
                return true;
            }
        }
        return false;
    }
}
