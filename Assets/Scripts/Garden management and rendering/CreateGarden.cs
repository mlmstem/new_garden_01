using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateGarden : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject tomato;
    [SerializeField] private GameObject cabbage;
    [SerializeField] private GameObject chili;
    [SerializeField] private GameObject eggplant;
    [SerializeField] private GameObject apple;
    private int rows = 2;
    private int column = 3;
    private int numVeg;
    // private int[] allX;
    // private int[] allY;
    // private string[] allPlantType;
    // private string[] allPlantStatus;
    [System.Serializable]
    public class GardenData
    {
        public int X;
        public int Y;
        public string plantType;
        public string plantStatus;
    }

    public class GardenDataArray
    {
        public GardenData[] gardenDataArray;
    }

    // creates garden based on input of field size
    void Start()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < column; j++)
            {
                var thisField = Instantiate(field, new Vector3(-4 * j, 0, -4 * i), Quaternion.identity);
                var fieldStatus = thisField.GetComponent<FieldStatus>();
                fieldStatus.setIndex(i, j);
            }
        }

        // allX = new int[rows * column];
        // allY = new int[rows * column];
        // allPlantType = new string[rows * column];
        // allPlantStatus = new string[rows * column];

        StartCoroutine(ShowCurrentGarden());
    }

    private IEnumerator ShowCurrentGarden()
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        userData data = new userData();
        data.username = username;

        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getCurrentGarden");
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
        else
        {
            // Deserialize the received JSON data into an array of objects
            GardenDataArray gardenDataArrayWrapper = JsonUtility.FromJson<GardenDataArray>(request.downloadHandler.text);
            GardenData[] gardenDataArray = gardenDataArrayWrapper.gardenDataArray;

            // Iterate through the array and instantiate objects in Unity
            for (int i = 0; i < rows * column && i < gardenDataArray.Length; i++)
            {
                GardenData gardenData = gardenDataArray[i];
                Debug.Log("the position is: " + gardenData.X + " " + gardenData.Y);
                showCurrentPlants(gardenData.X, gardenData.Y, gardenData.plantType, gardenData.plantStatus);
            }

        }


    }

    public void showCurrentPlants(int x, int y, string type, string status)
    {
        if (status == "Unknown")
        {
            if (type == "tomato" || type == "Tomato")
            {
                var thisTomato = Instantiate(tomato, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisTomato.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisTomato.transform.parent = thisField.transform;
                thisTomato.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
            else if (type == "cabbage" || type == "Cabbage")
            {
                var thisCabbage = Instantiate(cabbage, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisCabbage.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisCabbage.transform.parent = thisField.transform;
                thisCabbage.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
            else if (type == "Eggplant")
            {
                var thisCabbage = Instantiate(eggplant, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisCabbage.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisCabbage.transform.parent = thisField.transform;
                thisCabbage.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
            else if (type == "Chili")
            {
                var thisCabbage = Instantiate(chili, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisCabbage.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisCabbage.transform.parent = thisField.transform;
                thisCabbage.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
            else if (type == "Cucumber")
            {
                var thisCabbage = Instantiate(apple, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisCabbage.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisCabbage.transform.parent = thisField.transform;
                thisCabbage.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
            else if (type == "Carrot")
            {
                var thisCabbage = Instantiate(apple, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisCabbage.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisCabbage.transform.parent = thisField.transform;
                thisCabbage.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
            else
            {
                // is a Potato
                var thisCabbage = Instantiate(apple, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisCabbage.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisCabbage.transform.parent = thisField.transform;
                thisCabbage.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
        }
    }

    public void createTomato()
    {
        Instantiate(tomato, new Vector3(6, 2, 0), Quaternion.identity);
    }
    public void createCabbage()
    {
        Instantiate(cabbage, new Vector3(6, 2, 0), Quaternion.identity);
    }
    public void createChili()
    {
        Instantiate(chili, new Vector3(6, 2, 0), Quaternion.identity);
    }
    public void createEggplant()
    {
        Instantiate(eggplant, new Vector3(6, 2, 0), Quaternion.identity);
    }
    public void createApple()
    {
        Instantiate(apple, new Vector3(6, 2, 0), Quaternion.identity);
    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}
