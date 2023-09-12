using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Vector3 mousePosition;
    private bool onField = false;

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        //Debug.Log(this.gameObject.name);
    }

    private void OnMouseDrag()
    {
        var moveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        // make sure object cannot pass through ground
        if (moveTo.y >= 0)
        {
            transform.position = moveTo;
        }
        else
        {
            moveTo.y = 0;
            transform.position = moveTo;
        }
    }

    // <Implement> destroy object when moved out of field
    void OnTriggerEnter(Collider col)
    {
        if (!onField)
        {
            // Debug.Log("hits something");
            // Debug.Log(col.GetComponent<Collider>().name);
            // check if object hit is a field
            if (col.GetComponent<Collider>().name == "Field(Clone)")
            {
                // Debug.Log("hits field");
                var fieldStatus = col.GetComponent<FieldStatus>();
                // check if field is full
                if (!fieldStatus.isFull)
                {
                    // Debug.Log("field is empty");
                    // allocate object to the field
                    onField = true;
                    fieldStatus.setFull();
                    this.gameObject.transform.parent = col.gameObject.transform;
                    this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    // Debug.Log("field is full");
                    // <Implement> destroy object
                }
            }
        }
    }
}
