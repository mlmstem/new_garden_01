using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUpWindow;

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
        }
        popUpWindow.SetActive(false);
    }
}
