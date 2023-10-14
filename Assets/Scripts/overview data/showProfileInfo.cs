using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class showProfileInfo : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;

    public string usernameGot;
    public string emailGot;
    public string passwordGot;

    void Start()
    {
        StartCoroutine(GetProfileInfo());
    }

    private IEnumerator GetProfileInfo()
    {
        string username = PlayerPrefs.GetString("Username", "DefaultUsername");
        userData data = new userData();
        data.username = username;

        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:13756/account/getProfileData");
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

            // Changing the placeholder text of the fields to data received
            usernameField.transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text = "Name: " + usernameGot;
            emailField.transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text = "Email: " + emailGot;
            passwordField.transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text = "Password: " + passwordGot;

            // <Question> how to update profile info?
        }
    }

    [System.Serializable]
    public class userData
    {
        public string username;
    }
}
