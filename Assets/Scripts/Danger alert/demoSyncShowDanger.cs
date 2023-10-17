using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoSyncShowDanger : MonoBehaviour
{
    [SerializeField] private GameObject[] toPutDanger;
    [SerializeField] private Material material;

    public void syncAndDanger()
    {
        foreach (GameObject plant in toPutDanger)
        {
            Object.GetComponent<MeshRenderer>().material = material;
        }
    }
}
