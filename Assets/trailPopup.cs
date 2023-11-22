using UnityEngine;
using UnityEngine.SceneManagement;

public class trailPopup : MonoBehaviour
{
    // Add the name of the scene you want to load
    private string barGraphSceneName = "BarGraph Overview";

    // Start is called before the first frame update
    void Start()
    {
        // Add any initialization code if needed
    }

    // Update is called once per frame
    void Update()
    {
        // Add any update code if needed
    }

    // Function to be called when the button is clicked
    public void OnButtonClick()
    {
        // Call this function when the button is clicked
        LoadBarGraphScene();
    }

    // Function to load the BarGraph Overview scene
    private void LoadBarGraphScene()
    {
        SceneManager.LoadScene("BarGraph Overview");
    }


    public void OnReturnClick()
    {
        // Call this function when the button is clicked
        LoadMainSence();
    }

    // Function to load the BarGraph Overview scene
    private void LoadMainSence()
    {
        SceneManager.LoadScene("MainTestScene");
    }
}