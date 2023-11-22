using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] public GameObject popUpWindow;

    // open and close a popup window on click
    public void clickForPopUp()
    {
        if (popUpWindow.activeSelf)
        {
            closePopUp();
        }
        else
        {
            openPopUp();
        }
    }

    public void openPopUp()
    {
        // close all other pop up windows
        GameObject[] otherWindows = GameObject.FindGameObjectsWithTag("popups");
        foreach (GameObject window in otherWindows)
        {
            window.SetActive(false);
            // Additional modification: Set the render mode to Screen Space - Overlay
            Canvas canvas = window.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }
        popUpWindow.SetActive(true);
    }

    public void closePopUp()
    {
        // close all other pop-up windows
        GameObject[] otherWindows = GameObject.FindGameObjectsWithTag("popups");
        foreach (GameObject window in otherWindows)
        {
            window.SetActive(false);
            // Additional modification: Set the render mode to Screen Space - Overlay
            Canvas canvas = window.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }
        popUpWindow.SetActive(false);
    }
}

