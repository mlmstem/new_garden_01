using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGarden : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject tomato;
    [SerializeField] private GameObject cabbage;
    private int rows = 2;
    private int coloumns = 3;

    // creates garden based on input of field size
    void Start()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {
                Instantiate(field, new Vector3(-4 * j, 0, -4 * i), Quaternion.identity);
                var fieldStatus = field.GetComponent<FieldStatus>();
                // <Fix> suppose to update the index of the field but somehow not working
                fieldStatus.rowIndex = i;
                fieldStatus.colIndex = j;
                // Debug.Log(fieldStatus.getRow());
                // Debug.Log(fieldStatus.getCol());
            }
        }
    }

    public void createTomato()
    {
        Instantiate(tomato, new Vector3(6, 2, 0), Quaternion.identity);
    }

    public void createCabbage()
    {
        Instantiate(cabbage, new Vector3(6, 2, 0), Quaternion.identity);
    }
}
