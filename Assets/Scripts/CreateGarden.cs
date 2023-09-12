using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGarden : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private int rows = 3;
    [SerializeField] private int coloumns = 5;

    // creates garden based on input of field size
    void Start()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {
                Instantiate(field, new Vector3(-4 * j, 0, -4 * i), Quaternion.identity);
            }
        }
    }
}
