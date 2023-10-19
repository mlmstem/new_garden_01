using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStatus : MonoBehaviour
{
    // describes the status of the field
    [SerializeField] public bool isFull = false;
    public int rowIndex;
    public int colIndex;

    private void Update()
    {
        if (this.transform.childCount == 0)
        {
            setEmpty();
        }
    }

    public void setIndex(int row, int col)
    {
        rowIndex = row;
        colIndex = col;
    }

    public void setFull()
    {
        this.isFull = true;
    }

    public void setEmpty()
    {
        this.isFull = false;
    }
}
