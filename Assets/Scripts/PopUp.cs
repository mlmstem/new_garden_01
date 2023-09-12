using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUpWindow;

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
        GameObject[] otherWindows = GameObject.FindGameObjectsWithTag("popups");
        foreach (GameObject window in otherWindows)
        {
            window.SetActive(false);
        }
        popUpWindow.SetActive(true);
    }

    public void closePopUp()
    {
        GameObject[] otherWindows = GameObject.FindGameObjectsWithTag("popups");
        foreach (GameObject window in otherWindows)
        {
            window.SetActive(false);
        }
        popUpWindow.SetActive(false);
    }
}
