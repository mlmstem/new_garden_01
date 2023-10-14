using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class PlantStatus : MonoBehaviour
{
    public int rowIndex;
    public int colIndex;

    [SerializeField] private GameObject specificDataPopUp;

    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI idField;
    [SerializeField] private TextMeshProUGUI typeField;
    [SerializeField] private TextMeshProUGUI startDateField;
    [SerializeField] private TextMeshProUGUI daysField;
    [SerializeField] private TextMeshProUGUI positionField;
    [SerializeField] private TextMeshProUGUI statusField;
    [SerializeField] private TextMeshProUGUI moistureField;
    [SerializeField] private TextMeshProUGUI temperatureField;
    [SerializeField] private TextMeshProUGUI pressureField;

    public string name;
    public int id;
    public string type;
    public string startDate;
    public int days;
    public string position;
    public string status;
    public double moisture;
    public double temperature;
    public double pressure;

    private void Start()
    {
        StartCoroutine(GetPlantInfo());
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.GetComponent<PopUp>().openPopUp();
        }
    }

    private IEnumerator GetPlantInfo()
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        userData data = new userData();
        data.username = username;

        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getPlantData");
        request.SetRequestHeader("Content-Type", "application/json");
        string requestBody = JsonUtility.ToJson(data);
        byte[] usernameRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(usernameRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) // Error
        {
            Debug.Log(request.error);
        }
        else // Success
        {
            JsonUtility.FromJsonOverwrite(request.downloadHandler.text, this);
            // Debug.Log(usernameGot);
            // Debug.Log(request.downloadHandler.text);

            // Changing the placeholder text of the fields to data received
            nameField.text = name;
            idField.text = "Id: " + id;
            typeField.text = "Plant Type: " + type;
            startDateField.text = "Start Date: " + startDate.Substring(0, 10);
            daysField.text = "Days: " + days;
            positionField.text = "Position: " + position;
            statusField.text = "Status: " + status;
            moistureField.text = "Moisture (%): " + moisture;
            temperatureField.text = "Temperature (°C): " + temperature;
            pressureField.text = "Pressure (Pa): " + pressure;

            // <Question> how to update profile info?
        }
    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}
