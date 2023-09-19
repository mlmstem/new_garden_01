using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class testGet : MonoBehaviour
{
    [SerializeField] private string authenticationEndpoint = "http://127.0.0.1:13756/getData";
    [SerializeField] private TextMeshProUGUI testText;

    void Start()
    {
        StartCoroutine(TryGet());
    }

    private IEnumerator TryGet()
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        // Debug.Log(username);
        using (UnityWebRequest request = UnityWebRequest.Get(authenticationEndpoint))
        {
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(username);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.isNetworkError) // Error
            {
                Debug.Log(request.error);
            }
            else // Success
            {
                testText.text = request.downloadHandler.text;
            }
        }
    }
}
