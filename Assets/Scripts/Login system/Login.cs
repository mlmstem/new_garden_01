using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [SerializeField] private string authenticationEndpoint = "http://127.0.0.1:13756/account";

    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private Button signupButton;
    [SerializeField] private TMP_InputField usernameInputField1;
    [SerializeField] private TMP_InputField passwordInputField2;
    [SerializeField] private float alertTextFont = 13f;

    public float transitionTime = 1f;

    public void OnLoginClick()
    {
        alertText.text = "Signing in...";
        // alertText.fontSize = alertTextFont;
        loginButton.interactable = false;

        StartCoroutine(TryLogin());
    }

    public void OnSignUpClick(){

        alertText.text = "registrating...";

        signupButton.interactable = false;

        StartCoroutine(TrySignUp());

    }

    private IEnumerator TrySignUp()
    {
        string username = usernameInputField1.text;
        string password = passwordInputField2.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            signupButton.interactable = true;
            yield break;
        }

        if (password.Length < 3 || password.Length > 24)
        {
            alertText.text = "Invalid password";
            signupButton.interactable = true;
            yield break;
        }

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
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;

            // Check if the response contains the "Registration success" message
            if (!string.IsNullOrEmpty(responseText))
            {
                // Successfully received a response, consider it a registration success
                alertText.text = "Registration success";
            }
            else
            {
                // The response is null or empty, consider it a registration failure
                alertText.text = "Registration failure";
            }
        }
        else
        {
            // Request failed, consider it a registration failure
            alertText.text = "Registration failure";
    }

    signupButton.interactable = true;


    }

    private IEnumerator TryLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            loginButton.interactable = true;
            yield break;
        }

        if (password.Length < 3 || password.Length > 24)
        {
            alertText.text = "Invalid password";
            loginButton.interactable = true;
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get($"{authenticationEndpoint}?rUsername={username}&rPassword={password}");
        var handler = request.SendWebRequest();
        Debug.Log($"{username}:{password}");

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

        if (!request.isNetworkError && !request.isHttpError) // Fixed the condition here
        {

            if (request.downloadHandler.text != "Invalid credentials")
            {
                loginButton.interactable = false;
                GameAccount returnedAccount = JsonUtility.FromJson<GameAccount>(request.downloadHandler.text);
                Debug.Log(returnedAccount);
                // store in playerPref
                PlayerPrefs.SetString("Username", returnedAccount.username);

                alertText.text = "Welcome " + returnedAccount.username;
                LoadNextScene(); // Fixed the method name

            }
            else
            {
                alertText.text = "Invalid credentials";
                loginButton.interactable = true;
            }
        }
        else
        {
            Debug.Log("Request error: " + request.error);
            alertText.text = "Error connecting to the server...";
            loginButton.interactable = true;
        }

        yield return null;
    }

    public void LoadNextScene() // Fixed the method declaration
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex); // Fixed the method call
    }
}

