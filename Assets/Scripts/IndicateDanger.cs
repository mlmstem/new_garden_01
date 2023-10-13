using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IndicateDanger : MonoBehaviour
{
    public int rowIndex;
    public int colIndex;
    public bool status;

    private void Start()
    {
        var ren = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        var mat = ren.materials;
        mat[0].color = Color.red;
        StartCoroutine(GetStatus());
    }

    private IEnumerator GetStatus()
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        userData data = new userData();
        data.username = username;

        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getPlantStatus");
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

            // change plant colour based on status
            var ren = gameObject.transform.GetChild(0).GetComponent<Renderer>();
            var mat = ren.materials;
            if (status)
            {
                foreach (var m in mat)
                {
                    m.color = Color.red;
                }
            }
            else
            {
                foreach (var m in mat)
                {
                    m.color = Color.white;
                }
            }
        }
    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}
