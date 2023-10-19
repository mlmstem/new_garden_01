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
    private int rows = 3;
    private int column = 4;
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
        if (type == "tomato" || type == "Tomato")
        {
            var thisTomato = Instantiate(tomato, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisTomato.GetComponent<DragAndDrop>().onField = true;
            var field = getField(x, y);
            thisTomato.transform.parent = field.transform;
            thisTomato.GetComponent<DragAndDrop>().Row = x;
            thisTomato.GetComponent<DragAndDrop>().Col = y;
            thisTomato.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (type == "cabbage" || type == "Cabbage")
        {
            var thisCabbage = Instantiate(cabbage, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisCabbage.GetComponent<DragAndDrop>().onField = true;
            var field = getField(x, y);
            thisCabbage.transform.parent = field.transform;
            thisCabbage.GetComponent<DragAndDrop>().Row = x;
            thisCabbage.GetComponent<DragAndDrop>().Col = y;
            thisCabbage.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (type == "Eggplant" || type == "eggplant")
        {
            var thisEggplant = Instantiate(eggplant, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisEggplant.GetComponent<DragAndDrop>().onField = true;
            var field = getField(x, y);
            thisEggplant.transform.parent = field.transform;
            thisEggplant.GetComponent<DragAndDrop>().Row = x;
            thisEggplant.GetComponent<DragAndDrop>().Col = y;
            thisEggplant.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (type == "Chili")
        {
            var thisChili = Instantiate(chili, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisChili.GetComponent<DragAndDrop>().onField = true;
            var field = getField(x, y);
            thisChili.transform.parent = field.transform;
            thisChili.GetComponent<DragAndDrop>().Row = x;
            thisChili.GetComponent<DragAndDrop>().Col = y;
            thisChili.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (type == "Cucumber")
        {
            var thisCucumber = Instantiate(apple, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisCucumber.GetComponent<DragAndDrop>().onField = true;
            var field = getField(x, y);
            thisCucumber.transform.parent = field.transform;
            thisCucumber.GetComponent<DragAndDrop>().Row = x;
            thisCucumber.GetComponent<DragAndDrop>().Col = y;
            thisCucumber.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (type == "Carrot")
        {
            var thisCarrot = Instantiate(apple, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisCarrot.GetComponent<DragAndDrop>().onField = true;
            var field = getField(x, y);
            thisCarrot.transform.parent = field.transform;
            thisCarrot.GetComponent<DragAndDrop>().Row = x;
            thisCarrot.GetComponent<DragAndDrop>().Col = y;
            thisCarrot.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            // is a Potato
            var thisPotato = Instantiate(apple, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisPotato.GetComponent<DragAndDrop>().onField = true;
            var field = getField(x, y);
            thisPotato.transform.parent = field.transform;
            thisPotato.GetComponent<DragAndDrop>().Row = x;
            thisPotato.GetComponent<DragAndDrop>().Col = y;
            thisPotato.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private GameObject getField(int row, int col)
    {
        var fields = GameObject.FindGameObjectsWithTag("field");
        foreach (var field in fields)
        {
            if (field.GetComponent<FieldStatus>().rowIndex == row && field.GetComponent<FieldStatus>().colIndex == col)
            {
                field.GetComponent<FieldStatus>().isFull = true;
                Debug.Log(field.GetComponent<FieldStatus>().isFull);
                return field;
            }
        }
        return null;
    }

    public void createTomato()
    {
        Instantiate(tomato, new Vector3(5, 2, 0), Quaternion.identity);
    }
    public void createCabbage()
    {
        Instantiate(cabbage, new Vector3(5, 2, 0), Quaternion.identity);
    }
    public void createChili()
    {
        Instantiate(chili, new Vector3(5, 2, 0), Quaternion.identity);
    }
    public void createEggplant()
    {
        Instantiate(eggplant, new Vector3(5, 2, 0), Quaternion.identity);
    }
    public void createApple()
    {
        Instantiate(apple, new Vector3(5, 2, 0), Quaternion.identity);
    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}
