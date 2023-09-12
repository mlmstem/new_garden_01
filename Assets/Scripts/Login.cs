using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;


public class Login : MonoBehaviour
{
    [SerializeField] private string authenticationEndpoint = "http://127.0.0.1:13756/account";
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;


    public void OnLoginClick()
    {

        StartCoroutine(TryLogin());

        string username = usernameInputField.text;
        string password = passwordInputField.text;

        Debug.Log($"{username}:{password}");

    }

    private IEnumerator TryLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        UnityWebRequest request = UnityWebRequest.Get($"{authenticationEndpoint}?rUsername={username}&rPassword={password}");
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

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);

            }
            else
            {
                Debug.Log("Unable to connect to the server ...");

            }


            yield return null;


        }

    }
}
