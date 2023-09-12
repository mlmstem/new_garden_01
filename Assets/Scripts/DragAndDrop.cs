using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Vector3 mousePosition;
    //GameObject collideObject;

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

    // private void OnMouseUp()
    // {
    //     if (collideObject)
    //     {
    //         var fieldStatus = collideObject.GetComponent<FieldStatus>();
    //         if (!fieldStatus.isFull)
    //         {
    //             Debug.Log("field is empty");
    //             fieldStatus.setFull();
    //         }
    //         else
    //         {
    //             Debug.Log("field is full");
    //         }
    //     }
    // }

    // void OnTriggerEnter(Collider col)
    // {
    //     if (col.GetComponent<Collider>().name == "Field")
    //     {
    //         collideObject = col.gameObject;
    //     }
    // }
}
