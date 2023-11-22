using UnityEngine;
using UnityEngine.UI;

public class YourScript : MonoBehaviour
{
    public Button yourButton; // Reference to your Button component

    void Start()
    {
        // Assuming you have assigned the Button component in the Inspector
        yourButton.interactable = true; // Set to true to make it interactable
    }

    // You can call this function to change interactability during runtime
    public void SetButtonInteractable(bool isInteractable)
    {
        yourButton.interactable = isInteractable;
    }
}
