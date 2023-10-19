using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoSyncShowDanger : MonoBehaviour
{
    private GameObject[] toPutDanger;
    [SerializeField] private Material material;

    public void syncAndDanger()
    {
        toPutDanger = GameObject.FindGameObjectsWithTag("danger");
        foreach (GameObject plant in toPutDanger)
        {
            // plant.GetComponent<MeshRenderer>().material = material;
            plant.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
