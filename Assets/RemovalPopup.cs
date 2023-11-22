using UnityEngine;

public class RemovalPopup : MonoBehaviour
{
    public void ConfirmRemoval()
    {
        // Implement the logic to remove the plant here
        // For example, call the RemovePlant method from your DragAndDrop script.
        GetComponentInParent<DragAndDrop>().RemovePlant();
        // Close the popup after removal
        gameObject.SetActive(false);
    }
}
