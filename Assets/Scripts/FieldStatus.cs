using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStatus : MonoBehaviour
{
    // describes the status of the field
    [SerializeField] public bool isFull = false;

    public void setFull()
    {
        this.isFull = true;
    }

    public void setEmpty()
    {
        this.isFull = false;
    }
}
