using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoSyncShowDanger : MonoBehaviour
{
    private GameObject[] toPutDanger;
    [SerializeField] private Material material;

    private void Start()
    {
        toPutDanger = GameObject.FindGameObjectsWithTag("danger");
    }

    public void syncAndDanger()
    {
        foreach (GameObject plant in toPutDanger)
        {
            plant.GetComponent<MeshRenderer>().material = material;
        }
    }
}
