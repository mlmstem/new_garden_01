using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InDangerToRed : MonoBehaviour
{
    public string danger;
    private int rowIndex;
    private int colIndex;

    private void syncDanger()
    {
        rowIndex = gameObject.GetComponent<DragAndDrop>().Row;
        colIndex = gameObject.GetComponent<DragAndDrop>().Col;
        StartCoroutine(GetPlantDanger(rowIndex, colIndex));
    }

    private IEnumerator GetPlantDanger(int rowIndex, int colIndex)
    {
        if (rowIndex < 0 && colIndex < 0)
        {
            // rowIndex or colIndex is not a valid number
            Debug.LogError("Invalid rowIndex or colIndex: " + rowIndex + ", " + colIndex);
        }

        PlantData data = new PlantData();
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        data.username = username;
        data.rowIndex = rowIndex;
        data.colIndex = colIndex;

        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getPlantDanger");
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

            if (danger == "In Danger")
            {
                Material material = (Material)Resources.Load("Material.006", typeof(Material));
                this.GetComponentInChildren<MeshRenderer>().material = material;
            }

        }
    }

    [System.Serializable]
    public class PlantData
    {
        public string username;
        public int rowIndex;
        public int colIndex;
    };
}
