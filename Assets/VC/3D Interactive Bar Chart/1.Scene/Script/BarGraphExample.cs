using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BarGraph.VittorCloud;
using UnityEngine.Networking;

public class BarGraphExample : MonoBehaviour
{
    [System.Serializable]
    public class GardenData
    {
        public float moisture; // Adjusted attribute name to "moisture"
        public float Temp;
        public float Pressure;
        public string PlantStatus;
    }

    [System.Serializable]
    public class GardenDataArray // Adjusted class name to "GardenDataArray"
    {
        public GardenData[] gardenDataArray;
    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }

    public List<BarGraphDataSet> exampleDataSet;
    BarGraphGenerator barGraphGenerator;

    private bool hasGeneratedGraph = false;

    void Start()
    {
        barGraphGenerator = GetComponent<BarGraphGenerator>();

        if (exampleDataSet.Count == 0)
        {
            Debug.LogError("ExampleDataSet is Empty!");
            return;
        }

        StartCoroutine(GetGraphData());
    }

    private IEnumerator GetGraphData()
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        userData data = new userData();
        data.username = username;

        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getGraph");
        request.SetRequestHeader("Content-Type", "application/json");
        string requestBody = JsonUtility.ToJson(data);
        byte[] usernameRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(usernameRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Deserialize the received JSON data into an array of objects
            GardenDataArray gardenDataArrayWrapper = JsonUtility.FromJson<GardenDataArray>(request.downloadHandler.text); // Adjusted attribute name to "moisture"
            GardenData[] gardenDataArray = gardenDataArrayWrapper.gardenDataArray;

            // Process the received data and modify the exampleDataSet
            ProcessGardenData(gardenDataArray);

            // Generate the bar graph with the modified exampleDataSet
            barGraphGenerator.GeneratBarGraph(exampleDataSet);

            // Set the flag to indicate that the graph has been generated
            hasGeneratedGraph = true;
        }
    }

    private void ProcessGardenData(GardenData[] gardenDataArray)
    {
        // Reset the values in exampleDataSet
        ResetExampleDataSet();

        // Process each garden data
        for (int i = 0; i < gardenDataArray.Length; i++)
        {
            GardenData gardenData = gardenDataArray[i];

            // Process Moisture
            ProcessVariable(gardenData.moisture, exampleDataSet[0].ListOfBars); // Adjusted attribute name to "moisture"

            // Process Temp
            ProcessVariable(gardenData.Temp, exampleDataSet[1].ListOfBars);

            // Process Pressure
            ProcessVariable(gardenData.Pressure, exampleDataSet[2].ListOfBars);
        }
    }

    private void ProcessVariable(float value, List<XYBarValues> listOfBars)
    {
        // Determine the category based on the value
        int category = (int)Mathf.Clamp(value / 20, 0, 4);

        // Increment the corresponding variable in the list of bars
        listOfBars[category].YValue++;
    }

    private void ResetExampleDataSet()
    {
        foreach (var dataSet in exampleDataSet)
        {
            foreach (var bar in dataSet.ListOfBars)
            {
                // Resetting Y values to zero
                bar.YValue = 0;
            }
        }
    }
}


