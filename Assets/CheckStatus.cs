using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CheckStatus : MonoBehaviour
{
    public Image healthyPie;
    public Image dangerPie;
    public Image diseasedPie;
    public Image unknownPie;

    public TextMeshProUGUI healthyText;
    public TextMeshProUGUI dangerText;
    public TextMeshProUGUI diseasedText;
    public TextMeshProUGUI unknownText;

     public TextMeshProUGUI overallStatus;

    [System.Serializable]
    public class GardenData
    {
        public float moisture;
        public float Temp;
        public float Pressure;
        public string plantStatus;
    }

    [System.Serializable]
    public class GardenDataArray
    {
        public GardenData[] gardenDataArray;
    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }

    void Start()
    {
        StartCoroutine(GetGraphData());
    }

    private IEnumerator GetGraphData()
    {
        Debug.Log("sending unity request");
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        userData data = new userData();
        data.username = username;

        Debug.Log("username is : " + data.username);


        string url = $"http://127.0.0.1:13756/account/getCurrentGarden?username={username}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        //request.SetRequestHeader("Content-Type", "application/json");
        //string requestBody = JsonUtility.ToJson(data);
        //byte[] usernameRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(usernameRaw);
        //request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //yield return request.SendWebRequest();

        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        }
        else
        {
            // Deserialize the received JSON data into an array of objects
            GardenDataArray gardenDataArrayWrapper = JsonUtility.FromJson<GardenDataArray>(request.downloadHandler.text);
            GardenData[] gardenDataArray = gardenDataArrayWrapper.gardenDataArray;

            // Counters for each status
            int healthyCount = 0;
            int dangerCount = 0;
            int diseasedCount = 0;
            int unknownCount = 0;

            // Calculate counts for each status
            foreach (var dataEntry in gardenDataArray)
            {
                switch (dataEntry.plantStatus.ToLower())
                {
                    case "healthy":
                        healthyCount++;
                        break;
                    case "in danger":
                        dangerCount++;
                        break;
                    case "diseased":
                        diseasedCount++;
                        break;
                    case "unknown":
                        unknownCount++;
                        break;
                    default:
                        break;
                }
            }

            // Calculate percentages
            int totalCount = healthyCount + dangerCount + diseasedCount + unknownCount;
            float healthyPercentage = (float)healthyCount / totalCount;
            float dangerPercentage = (float)dangerCount / totalCount;
            float diseasedPercentage = (float)diseasedCount / totalCount;
            float unknownPercentage = (float)unknownCount / totalCount;

            // Update pie graphs
            UpdatePieChart(healthyPercentage, dangerPercentage, diseasedPercentage, unknownPercentage);
            UpdateTextFields(healthyPercentage, dangerPercentage, diseasedPercentage, unknownPercentage);

        }
    }

    // Update the pie chart fill amount based on the given percentage
     void UpdatePieChart(float healthyPercent, float dangerPercent, float diseasedPercent, float unknownPercent)
    {
        float totalPercentage = healthyPercent + dangerPercent + diseasedPercent + unknownPercent;

        SetPieChartSlice(healthyPie, 0, 0, healthyPercent / totalPercentage * 360f); // Healthy slice
        SetPieChartSlice(dangerPie, 1, healthyPercent / totalPercentage * 360f, (dangerPercent + healthyPercent) / totalPercentage * 360f);  // Danger slice
        SetPieChartSlice(diseasedPie, 2, (healthyPercent + dangerPercent) / totalPercentage * 360f, (diseasedPercent + healthyPercent + dangerPercent)/ totalPercentage * 360f);  // Diseased slice
        SetPieChartSlice(unknownPie, 3, (healthyPercent + dangerPercent + diseasedPercent) / totalPercentage * 360f,  360f);  // Unknown slice
    }

    void SetPieChartSlice(Image pieChart, int sliceIndex, float startDegree, float endDegree)
    {
        float fillAmount = Mathf.Clamp01((endDegree - startDegree) / 360f);
        pieChart.fillAmount = fillAmount;
        pieChart.fillClockwise = true; // Clockwise fill for a pie chart

        RectTransform rectTransform = pieChart.GetComponent<RectTransform>();
        rectTransform.localEulerAngles = new Vector3(0, 0, -startDegree); // Rotate the pie chart to start from the correct degree
    }

    void UpdateTextFields(float healthyPercent, float dangerPercent, float diseasedPercent, float unknownPercent)
    {
        // Update TextMeshProUGUI text
        healthyText.text = "Healthy: " + 100 * healthyPercent + "%";
        dangerText.text = "In Danger: " + 100 * dangerPercent + "%";
        diseasedText.text = "Diseased: " + 100 * diseasedPercent + "%";
        unknownText.text = "Unknown: " + 100 * unknownPercent + "%";

        if(healthyPercent > 0.6){

            overallStatus.text = "Your Plants are in good Conditions!";
        }
    }

}