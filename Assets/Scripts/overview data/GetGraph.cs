using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetGraph : MonoBehaviour
{
    [SerializeField] private string imgID = "6503fac1ded2acd4330d936a";
    [SerializeField] private Transform img;
    public Sprite graph;

    void Start()
    {
        StartCoroutine(GetGraphImg());
    }

    private IEnumerator GetGraphImg()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getGraph");
        // UnityWebRequest request = UnityWebRequest.Get("/account/getGraph");
        //request.SetRequestHeader("Content-Type", "application/json");
        string requestBody = JsonUtility.ToJson(imgID);
        byte[] requestRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(requestRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) // Error
        {
            Debug.Log(request.error);
        }
        else // Success
        {
            //var texture = DownloadHandlerTexture.GetContent(request);
            //graph = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            //img.GetComponent<Image>().overrideSprite = graph;
        }
    }
}
