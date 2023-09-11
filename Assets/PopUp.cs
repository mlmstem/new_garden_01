using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUpWindow;
    private bool opened;
    // Start is called before the first frame update

    private void Start()
    {
        opened = false;
    }

    public void clickPopUpButton()
    {
        Debug.Log("button pressed");
        Debug.Log(opened);
        if (opened)
        {
            closePopUp();
            opened = false;
            Debug.Log("button opened set to" + opened);
        }
        else
        {
            openPopUp();
            opened = true;
            Debug.Log("button opened set to" + opened);
        }
    }

    public void openPopUp()
    {
        popUpWindow.SetActive(true);
    }

    public void closePopUp()
    {
        popUpWindow.SetActive(false);
    }
}
