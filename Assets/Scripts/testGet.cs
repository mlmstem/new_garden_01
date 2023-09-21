using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class testGet : MonoBehaviour
{
    [SerializeField] private string authenticationEndpoint = "http://127.0.0.1:13756/account/getData";
    [SerializeField] private TextMeshProUGUI testText;

    void Start()
    {
        StartCoroutine(TryGet());
    }

    private IEnumerator TryGet()
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        // Debug.Log(username);
        userData data = new userData();
        data.username = username;

        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getData");
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
            testText.text = request.downloadHandler.text;
        }

    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}
