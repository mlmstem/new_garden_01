using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateGarden : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject tomato;
    [SerializeField] private GameObject cabbage;
    private int rows = 2;
    private int column = 3;
    private int numVeg;
    // private int[] allX;
    // private int[] allY;
    // private string[] allPlantType;
    // private string[] allPlantStatus;
    public int X;
    public int Y;
    public string plantType;
    public string plantStatus;

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
        else // Success
        {
            JsonUtility.FromJsonOverwrite(request.downloadHandler.text, this);
            showCurrentPlants(X, Y, plantType, plantStatus);
        }
    }

    public void showCurrentPlants(int x, int y, string type, string status)
    {
        if (status == "Unknown")
        {
            if (type == "tomato")
            {
                var thisTomato = Instantiate(tomato, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
                thisTomato.GetComponent<DragAndDrop>().onField = true;
                // change field to full
                //thisTomato.transform.parent = thisField.transform;
                thisTomato.transform.localPosition = new Vector3(-4 * x, 0, -4 * y);
            }
            else
            {
                var thisCabbage = Instantiate(cabbage, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
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

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}
