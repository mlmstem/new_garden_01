using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartSyncStatusOnClick : MonoBehaviour
{
    public SyncStatus syncStatusScript; // Reference to the SyncStatus script

    // UnityEvent to allow multiple functions to be added through the Unity Editor
    [SerializeField] private UnityEvent onClickEvents;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            // Add the StartSyncStatus method to the UnityEvent
            onClickEvents.AddListener(StartSyncStatus);

            // Add any additional methods here using onClickEvents.AddListener(YourMethod);
            // For example:
            // onClickEvents.AddListener(YourOtherMethod);
            
            // Attach the UnityEvent to the button's onClick event
            button.onClick.AddListener(InvokeOnClickEvents);
        }
    }

    void StartSyncStatus()
    {
        StartCoroutine(syncStatusScript.DelayedCheckPlantStatus());
    }

    // Method to invoke all added UnityEvents
    void InvokeOnClickEvents()
    {
        onClickEvents.Invoke();
    }
}
