using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        // Vector3 mousePos = Input.mousePosition;
        // mousePos.z = Camera.main.nearClipPlane;
        // mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;
        var targetPos = transform.position;

        if (plane.Raycast(ray, out point))
        {
            targetPos = ray.GetPoint(point);
        }
        targetPos.x = targetPos.x - 2;
        targetPos.z = targetPos.z - 3;
        transform.position = targetPos;
    }
}
