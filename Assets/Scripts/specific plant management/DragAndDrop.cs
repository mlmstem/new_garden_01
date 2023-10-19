using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private string plantServer = "http://127.0.0.1:13756/account/add-plant";
    [SerializeField] private GameObject inputPopUp;
    Vector3 mousePosition;
    public bool onField = false;
    public bool pickedUp = false;
    public bool inDatabase = false;
    public int Col;
    public int Row;
    [SerializeField] private TMP_InputField statusInputField;
    [SerializeField] private TMP_InputField moistureInputField;
    [SerializeField] private TMP_InputField temperatureInputField;
    [SerializeField] private TMP_InputField pressureInputField;
    private GameObject closepopup;

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        onField = false;
        pickedUp = true;
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

    void OnTriggerEnter(Collider col)
    {

        if (!onField && pickedUp)
        {
            //Debug.Log("hits something");
            Debug.Log(col.GetComponent<Collider>().name);
            // check if object hit is a field
            if (col.GetComponent<Collider>().name == "Field(Clone)")
            {
                // Debug.Log("hits field");
                var fieldStatus = col.GetComponent<FieldStatus>();
                Col = fieldStatus.colIndex;
                Row = fieldStatus.rowIndex;

                Debug.Log("current location is " + Col + "  " + Row);

                // check if field is full
                if (!fieldStatus.isFull)
                {
                    inputPopUp.SetActive(true);
                    closepopup = GameObject.FindWithTag("closepopup");
                    if (closepopup)
                    {
                        if (closepopup.activeSelf)
                        {
                            closepopup.SetActive(false);
                        }
                    }
                    // Debug.Log("field is empty");
                    // allocate object to the field
                    // Using post request here to send plant data to mongodb atlas

                    onField = true;
                    fieldStatus.setFull();
                    this.gameObject.transform.parent = col.gameObject.transform;
                    this.gameObject.transform.localPosition = new Vector3(0, 0, 0);

                }
                else
                {
                    Debug.Log("field is full");

                    // if (inDatabase)
                    // {
                    //     // send plant id
                    //     Debug.Log("deleting object");
                    //     StartCoroutine(RemovePlantData(Row, Col));
                    // }
                    // Destroy(gameObject);
                }
            }
            else
            {

                // var fieldStatus = col.GetComponent<FieldStatus>();
                // Col = fieldStatus.colIndex;
                // Row = fieldStatus.rowIndex;

                StartCoroutine(RemovePlantData(Row, Col));
                Debug.Log("deleting object");
                // if (inDatabase)
                // {
                //     // send plant id
                //     StartCoroutine(RemovePlantData(Row,Col));
                //     Debug.Log("deleting object");
                // }
                Debug.Log("hits something else");
                Destroy(gameObject);
            }
        }
    }

    public void setSend(GameObject window)
    {
        if (statusInputField.text != "" && moistureInputField.text != "" && temperatureInputField.text != "" && pressureInputField.text != "")
        {
            Debug.Log(statusInputField.text);
            Debug.Log(moistureInputField.text);
            Debug.Log(temperatureInputField.text);
            Debug.Log(pressureInputField.text);
            // finding info about the plant to send to database
            var plantType = this.gameObject.tag;
            // Debug.Log(plantType);
            // Debug.Log(fieldStatus.rowIndex);
            // Debug.Log(fieldStatus.colIndex);
            window.SetActive(false);
            closepopup = GameObject.FindWithTag("closepopup");
            if (closepopup)
            {
                if (!closepopup.activeSelf)
                {
                    closepopup.SetActive(true);
                }
            }
            StartCoroutine(SendPlantDataToServer(plantType, Row, Col, statusInputField.text, int.Parse(moistureInputField.text), int.Parse(temperatureInputField.text), int.Parse(pressureInputField.text)));
        }
    }

    IEnumerator SendPlantDataToServer(string type, int rowIndex, int colIndex, string status, int moist, int temp, int pressure)
    {
        // Create a JSON object with the default plant data using JsonUtility
        PlantData plantData = new PlantData
        {
            plantType = type,
            startDate = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            age = 0,
            position = "(" + rowIndex + ", " + colIndex + ")",
            status = status,
            moisturePercentage = moist,
            temperatureCelsius = temp,
            atmosphericPressurePa = pressure
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
            inDatabase = true;
        }
        else
        {
            // Error handling if the request fails
            Debug.LogError("Error sending plant data: " + request.error);
        }
    }

    [System.Serializable]
    public class PlantRemovalData
    {
        public string username;
        public int rowIndex;
        public int colIndex;
    }



    IEnumerator RemovePlantData(int rowIndex, int colIndex)
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");

        if (rowIndex >= 0 && colIndex >= 0)
        {
            // rowIndex and colIndex are valid numbers, continue with the request logic
        }
        else
        {
            // rowIndex or colIndex is not a valid number
            Debug.LogError("Invalid rowIndex or colIndex: " + rowIndex + ", " + colIndex);
        }

        PlantRemovalData data = new PlantRemovalData
        {
            username = username,
            rowIndex = rowIndex,
            colIndex = colIndex
        };

        Debug.Log("removing plant on position " + rowIndex + colIndex);

        // Use UnityWebRequest.Post to send data as JSON
        UnityWebRequest request = UnityWebRequest.PostWwwForm("http://127.0.0.1:13756/account/removePlant", "");
        request.SetRequestHeader("Content-Type", "application/json");

        string requestBody = JsonUtility.ToJson(data);
        byte[] requestBodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = new UploadHandlerRaw(requestBodyRaw);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("delete success");
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

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}