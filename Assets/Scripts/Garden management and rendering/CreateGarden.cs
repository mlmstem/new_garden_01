using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CreateGarden : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject tomatoPrefab;
    [SerializeField] private GameObject cabbagePrefab;
    [SerializeField] private GameObject chiliPrefab;
    [SerializeField] private GameObject eggplantPrefab;
    [SerializeField] private GameObject applePrefab;

    //[SerializeField] private string authenticationEndpoint = "http://127.0.0.1:13756/account/getCurrentGarden";
    [SerializeField] private string authenticationEndpoint = "/account/getCurrentGarden";

    private int rows = 3;
    private int column = 4;

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

    // creates a garden based on the input field size
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

        StartCoroutine(ShowCurrentGarden());
    }

    private IEnumerator ShowCurrentGarden()
    {

        Debug.Log("sending unity request");
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        userData data = new userData();
        data.username = username;

        Debug.Log("username is : " + data.username);

        
        UnityWebRequest request = UnityWebRequest.Get($"{authenticationEndpoint}?username={username}");
        //UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getCurrentGarden");
        // request.SetRequestHeader("Content-Type", "application/json");
        // string requestBody = JsonUtility.ToJson(data);
        // byte[] usernameRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        // request.uploadHandler = (UploadHandler)new UploadHandlerRaw(usernameRaw);
        //request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
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

        //yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) // Error
        {
            Debug.Log(request.error);
            // Debug.LogError(request.error);
            // Debug.LogError(request.downloadHandler.text);
        }
        else
        {
            // Deserialize the received JSON data into an array of objects
            GardenDataArray gardenDataArrayWrapper = JsonUtility.FromJson<GardenDataArray>(request.downloadHandler.text);
            GardenData[] gardenDataArray = gardenDataArrayWrapper.gardenDataArray;

            Debug.Log("Accquiring the garden array data");

            // Iterate through the array and instantiate objects in Unity
            for (int i = 0; i < rows * column && i < gardenDataArray.Length; i++)
            {
                GardenData gardenData = gardenDataArray[i];
                showCurrentPlants(gardenData.X, gardenData.Y, gardenData.plantType, gardenData.plantStatus);
            }
        }
    }



    public void showCurrentPlants(int x, int y, string type, string status)
    {
        GameObject plantPrefab = null;

        // Determine the prefab based on the plant type
        switch (type.ToLower())
        {
            case "tomato":
                plantPrefab = tomatoPrefab;
                break;
            case "cabbage":
                plantPrefab = cabbagePrefab;
                break;
            case "eggplant":
                plantPrefab = eggplantPrefab;
                break;
            case "chili":
                plantPrefab = chiliPrefab;
                break;
            case "apple":
                plantPrefab = applePrefab;
                break;
            default:
                Debug.LogWarning("Unknown plant type: " + type);
                return;
        }

        if (plantPrefab != null)
        {
            var thisPlant = Instantiate(plantPrefab, new Vector3(-4 * x, 0, -4 * y), Quaternion.identity);
            thisPlant.GetComponent<DragAndDrop>().onField = true;
            thisPlant.GetComponent<DragAndDrop>().enteredField = true;
            var field = getField(x, y);
            thisPlant.transform.parent = field.transform;
            thisPlant.GetComponent<DragAndDrop>().Row = x;
            thisPlant.GetComponent<DragAndDrop>().Col = y;
            thisPlant.transform.localPosition = new Vector3(0, 0, 0);
            CollisionImmune collisionImmune = thisPlant.GetComponent<CollisionImmune>();
            if (collisionImmune != null)
                {
                collisionImmune.enabled = true;
                }

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

    [System.Serializable]
    public class userData
    {
        public string username;
    }


     public void createTomato()
    {
         GameObject newTomato = Instantiate(tomatoPrefab, new Vector3(5, 2, 0), Quaternion.identity);
        
        // Disable CollisionImmune script for the newly created tomato
        CollisionImmune collisionImmune = newTomato.GetComponent<CollisionImmune>();
        if (collisionImmune != null)
        {
            collisionImmune.enabled = false;
        }
    }

    public void createCabbage()
    {
        GameObject newCabbage = Instantiate(cabbagePrefab, new Vector3(5, 2, 0), Quaternion.identity);

        CollisionImmune collisionImmune = newCabbage.GetComponent<CollisionImmune>();
        if (collisionImmune != null)
        {
            collisionImmune.enabled = false;
        }
    }

    public void createChili()
    {
         GameObject newchili = Instantiate(chiliPrefab, new Vector3(5, 2, 0), Quaternion.identity);
        
        // Disable CollisionImmune script for the newly created tomato
        CollisionImmune collisionImmune = newchili.GetComponent<CollisionImmune>();
        if (collisionImmune != null)
        {
            collisionImmune.enabled = false;
        }
    }

    public void createEggplant()
    {
         GameObject newEggplant = Instantiate(eggplantPrefab, new Vector3(5, 2, 0), Quaternion.identity);
        
        // Disable CollisionImmune script for the newly created tomato
        CollisionImmune collisionImmune = newEggplant.GetComponent<CollisionImmune>();
        if (collisionImmune != null)
        {
            collisionImmune.enabled = false;
        }
    }

    public void createApple()
    {
         GameObject newApple = Instantiate(applePrefab, new Vector3(5, 2, 0), Quaternion.identity);
        
        // Disable CollisionImmune script for the newly created tomato
        CollisionImmune collisionImmune = newApple.GetComponent<CollisionImmune>();
        if (collisionImmune != null)
        {
            collisionImmune.enabled = false;
        }
    }

}
