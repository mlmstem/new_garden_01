using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SyncStatus : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI alertText; // Reference to the TMP text field
    private int dangerPlantCount = 0; // Counter for the number of plants in danger

    public GameObject dangerAlertObject;

    void Start()
    {


    Debug.Log("starting the checking process");
    StartCoroutine(DelayedCheckPlantStatus());
    }

    public IEnumerator DelayedCheckPlantStatus()
    {
    yield return new WaitForSeconds(3.0f); // Delay for 3 seconds

    // Now start checking plant status
    StartCoroutine(CheckPlantStatus());
    }

    IEnumerator CheckPlantStatus()
    {
        string[] plantTags = { "tomato", "apple", "eggplant", "chili", "cabbage" };

        foreach (string plantTag in plantTags)
        {
            GameObject[] plants = GameObject.FindGameObjectsWithTag(plantTag);

            foreach (GameObject plant in plants)
            {
                PlantStatus status = plant.GetComponent<PlantStatus>();

                if (status != null && status.status != null && status.status == "In Danger")
                {
                    // Set the child object active
                    plant.transform.GetChild(0).gameObject.SetActive(true);
                    
                    // Increment the count of plants in danger
                    dangerPlantCount++;
                }
            }
        }

        if (dangerPlantCount > 0)
        {
            dangerAlertObject.SetActive(true);
        }
        else{
            dangerAlertObject.SetActive(false);
        }

        // Update the alert text
        UpdateAlertText();

        yield return null;
    }

    void UpdateAlertText()
    {
        // Set the TMP text field to display the alert message
        alertText.text = "Alert: There are " + dangerPlantCount + " plants in your field having danger status!";
    }
}