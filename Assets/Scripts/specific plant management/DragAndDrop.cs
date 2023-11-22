using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private string plantServer = "http://127.0.0.1:13756/account/add-plant";
    [SerializeField] private GameObject inputPopUp;
    [SerializeField] private GameObject removalPopUp;


    Vector3 mousePosition;
    public bool onField = false;
    public bool pickedUp = false;
    public bool inDatabase = false;
    public int Col;
    public int Row;
    [SerializeField] private TMP_Dropdown statusDropdown;
    [SerializeField] private TMP_InputField moistureInputField;
    [SerializeField] private TMP_InputField temperatureInputField;
    [SerializeField] private TMP_InputField pressureInputField;

    [SerializeField] private TextMeshProUGUI Status1;
    [SerializeField] private TextMeshProUGUI Type1;
    [SerializeField] private TextMeshProUGUI Position1;
    [SerializeField] private TextMeshProUGUI moistureText1;
    [SerializeField] private TextMeshProUGUI temperatureText1;
    [SerializeField] private TextMeshProUGUI pressureText1;
    private GameObject closepopup;
    [SerializeField] public bool enteredField = false;


    private int originalRow;
    private int originalCol;

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


        originalRow = Row;
        originalCol = Col;
    

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

     private void OnMouseUp()
    {
        if (onField)
        {
            // Check if the field is full
            var fieldStatus = getFieldStatus(Row, Col);
            if (fieldStatus != null && fieldStatus.isFull)
            {
                // Set the plant inactive if the field is full
                gameObject.SetActive(false);
            }
            else
            {
                // Plant is over an empty field or other space, set it active
                gameObject.SetActive(true);
            }
        }
    }



    void OnTriggerEnter(Collider col)
    {
        if (!onField && pickedUp)
        {
            // Check if object hit is a field
            if (col.GetComponent<Collider>().name == "Field(Clone)")
            {
                StartCoroutine(EnterFieldCoroutine(col));
            }
            else if (col.CompareTag("apple") || col.CompareTag("tomato") || col.CompareTag("cabbage") || col.CompareTag("chili") || col.CompareTag("eggplant"))
            {
    // Check if the collided object is a plant
                DragAndDrop otherPlant = col.GetComponent<DragAndDrop>();
                if (otherPlant != null && otherPlant.onField)
                {
                    // Return the plant to its original position when colliding with another plant
                    Debug.Log("Hitting another plant!!");
                    DestroyPlant(); // Destroy the initiating plant
                }
            }
             else if (col.CompareTag("Environment"))
            {
                if (enteredField)
                {
                    // If the plant has entered the field and hits an environment object, trigger RemovePlant

                    Debug.Log(col.GetComponent<Collider>().name);
                    RemovePlant();
                }
                else
                {
                    // If the plant has not entered the field and hits an environment object, set it to inactive
                    gameObject.SetActive(false);
                }
            }
            else if (!col.GetComponent<Collider>().name.StartsWith("NonField"))
            {
                // Disable collision with non-field objects
                GetComponent<Collider>().enabled = false;
            }
        }
        else if (col.CompareTag("robot"))
        {
            // Destroy the plant if it collides with an object with the "robot" tag

            Debug.Log("hitting with the robot !!");
            DestroyPlant();
        }
    }

    IEnumerator EnterFieldCoroutine(Collider col)
    {
        var fieldStatus = col.GetComponent<FieldStatus>();
        Col = fieldStatus.colIndex;
        Row = fieldStatus.rowIndex;

        // Check if field is full
        if (!fieldStatus.isFull)
        {
            if (!enteredField)
            {
            inputPopUp.SetActive(true);
            }
            closepopup = GameObject.FindWithTag("closepopup");
            if (closepopup && closepopup.activeSelf)
            {
                closepopup.SetActive(false);
            }
            onField = true;
            fieldStatus.setFull();
            this.gameObject.transform.parent = col.gameObject.transform;
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0);

            // Set enteredField to true when the plant enters the field
            enteredField = true;

            CollisionImmune collisionImmune = this.GetComponent<CollisionImmune>();
            if (collisionImmune != null)
            {
                collisionImmune.enabled = true;
            }

            // Rigidbody rb = GetComponent<Rigidbody>();
            // rb.WakeUp();  // Wake up the Rigidbody

            // // Disable gravity with a frame delay
            // rb.useGravity = false;
            yield return null;  // Wait for the next frame
            //yield return new WaitForSeconds(2);

            // Disable collider
            // GetComponent<Collider>().enabled = false;
        }
        else
        {
            // Field is full, check if the plant has entered the field
            if (enteredField)
            {
                // Plant has entered the field, return it to its initial position
                Debug.Log("returning to the initial position");
                ReturnToOriginalPosition();


            }
            else
            {
                // Plant has not entered the field, set it to inactive
                gameObject.SetActive(false);
            }
        }
    }

   void ReturnToOriginalPosition()
    {
        // Return the plant to its original position only when the plant has entered the field and is being dragged
        if (enteredField)
        {
            Row = originalRow;
            Col = originalCol;
            var initialFieldStatus = getFieldStatus(originalRow, originalCol);

            Debug.Log("now returning to " +  originalRow + " " + originalCol );

            if (initialFieldStatus != null)
            {
                transform.parent = initialFieldStatus.transform;
                transform.localPosition = new Vector3(0, 0, 0);

                
            }
            else
            {
                // Handle the case where the initialFieldStatus is not found (optional)
                Debug.LogWarning("Initial field status not found for Row: " + originalRow + ", Col: " + originalCol);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        // Check if the plant is leaving the field
        if (col.GetComponent<Collider>().name == "Field(Clone)")
        {
            var fieldStatus = col.GetComponent<FieldStatus>();

            // Reset field status to false when the plant leaves the field
            // Rigidbody rb = GetComponent<Rigidbody>();

            // // Enable gravity
            // if (rb)
            // {
            //     // Enable gravity
            //     rb.useGravity = true;
            // }
            // else
            // {
            //     Debug.LogError("Rigidbody component not found!");
            // }

            // // Enable collision detection
            // GetComponent<Collider>().enabled = true;

            // Reset field status to false when the plant leaves the field
            fieldStatus.setEmpty();
        }
        else
        {
            // Check if the collider is another field clone
            if (col.GetComponent<Collider>().name.StartsWith("Field(Clone)"))
            {
                // Return the plant to its original position
                transform.parent = null;
                transform.localPosition = new Vector3(0, 0, 0);

                // Optionally, you can add any other logic related to the plant hitting another field clone
            }
            else if (col.CompareTag("robot"))
            {
                // Destroy the plant if it collides with an object with the "robot" tag
                DestroyPlant();
            }
        }
    }

    void DestroyPlant()
    {
        // Destroy the plant and handle any other logic related to the destruction
        Destroy(gameObject);
    }


    public void RemovePlant()
    {
    StartCoroutine(RemovePlantData(Row, Col));
    // Optionally, you can add any other logic related to plant removal
    Debug.Log("starting removing the plant on Row : " + Row + " COL: " +  Col);
    }

   public void setSend(GameObject window)
    {
    if (moistureInputField.text != "" && temperatureInputField.text != "" && pressureInputField.text != "")
    {
        Debug.Log(moistureInputField.text);
        Debug.Log(temperatureInputField.text);
        Debug.Log(pressureInputField.text);

        // Finding info about the plant to send to the database
        var plantType = this.gameObject.tag;

        // Access the selected value from the dropdown
        string selectedStatus = statusDropdown.options[statusDropdown.value].text;

        Debug.Log(selectedStatus);

        window.SetActive(false);
        closepopup = GameObject.FindWithTag("closepopup");

        if (closepopup)
        {
            if (!closepopup.activeSelf)
            {
                closepopup.SetActive(true);
            }
        }

        StartCoroutine(SendPlantDataToServer(plantType, Row, Col, selectedStatus, int.Parse(moistureInputField.text), int.Parse(temperatureInputField.text), int.Parse(pressureInputField.text)));
    }
    }

    IEnumerator SendPlantDataToServer(string type, int rowIndex, int colIndex, string status, int moist, int temp, int pressure)
    {
        // Create a JSON object with the default plant data using JsonUtility
        var plantStatus = GetComponent<PlantStatus>();
        plantStatus.name = type;
        plantStatus.type = type;
        plantStatus.status = status;
        plantStatus.rowIndex = rowIndex;
        plantStatus.moisture = moist;
        plantStatus.temperature = temp;
        plantStatus.pressure = pressure;

        Status1.text = "Status: " + status;
        Type1.text = "Plant Type: " + type;
        moistureText1.text = "Moisture (%): " + moist;
        temperatureText1.text = "Temperature (°C): " + temp;
        pressureText1.text = "Pressure (Pa): " + pressure;
        Position1.text = "Position: " + "(" + rowIndex + ", " + colIndex + ")";
            

// Repeat the pattern for other properties...

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
            Destroy(gameObject);
        }
    }

    private FieldStatus getFieldStatus(int row, int col)
    {
        var fields = GameObject.FindGameObjectsWithTag("field");
        foreach (var field in fields)
        {
            var fieldStatus = field.GetComponent<FieldStatus>();
            if (fieldStatus != null && fieldStatus.rowIndex == row && fieldStatus.colIndex == col)
            {
                return fieldStatus;
            }
        }
        return null;
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
