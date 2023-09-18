using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DragAndDrop : MonoBehaviour
{   
    [SerializeField] private string plantServer = "http://127.0.0.1:13756/account/add-plant";
    Vector3 mousePosition;
    private bool onField = false;

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        onField = false;
        //Debug.Log(this.gameObject.name);
    }

    private void OnMouseDrag()
    {
        var moveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        // make sure object cannot pass through ground
        moveTo.y = 2;
        if (moveTo.y > 0)
        {
            transform.position = moveTo;
        }
        else
        {
            moveTo.y = 0;
            transform.position = moveTo;
        }
    }

    // <Implement> destroy object when moved out of field
    void OnTriggerEnter(Collider col)
    {
        if (!onField)
        {
            //Debug.Log("hits something");
            Debug.Log(col.GetComponent<Collider>().name);
            // check if object hit is a field
            if (col.GetComponent<Collider>().name == "Field(Clone)")
            {
                // Debug.Log("hits field");
                var fieldStatus = col.GetComponent<FieldStatus>();
                // check if field is full
                if (!fieldStatus.isFull)
                {
                    // Debug.Log("field is empty");
                    // allocate object to the field
                    // Using post request here to send plant data to mongodb atlas

                    onField = true;
                    fieldStatus.setFull();
                    this.gameObject.transform.parent = col.gameObject.transform;
                    this.gameObject.transform.localPosition = new Vector3(0, 0, 0);

                    StartCoroutine(SendPlantDataToServer());
                }
                else
                {
                    // Debug.Log("field is full");
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("hits something else");
                Destroy(gameObject);
            }
        }
    }

    IEnumerator SendPlantDataToServer()
{
    // Create a JSON object with the default plant data using JsonUtility
    PlantData plantData = new PlantData
    {
        plantType = "tomato",
        startDate = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
        age = 0,
        position = "unknown",
        status = "Unknown",
        moisturePercentage = 100,
        temperatureCelsius = 100,
        atmosphericPressurePa = 100
    };

    // Replace "YOUR_USERNAME" with the actual username of the user
    string username = PlayerPrefs.GetString("Username", "DefaultUsername");

    // Create a request object
    UnityWebRequest request = UnityWebRequest.PostWwwForm(plantServer, "POST");
    request.SetRequestHeader("Content-Type", "application/json");

    // Create a JSON object to send in the request
    PlantRequestData requestData = new PlantRequestData
    {
        username = username,
        plantInfo = plantData
    };

    string requestBody = JsonUtility.ToJson(requestData);

    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

    // Send the POST request
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        // Plant added successfully
        Debug.Log("Plant added successfully");
    }
    else
    {
        // Error handling if the request fails
        Debug.LogError("Error sending plant data: " + request.error);
    }
    }

    [System.Serializable]
    public class PlantRequestData
    {
        public string username;
        public PlantData plantInfo;
    }

    [System.Serializable]
    public class PlantData
    {
        public string plantType;
        public string startDate;
        public int age;
        public string position;
        public string status;
        public int moisturePercentage;
        public int temperatureCelsius;
        public int atmosphericPressurePa;
    }

}
