using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerView : MonoBehaviour
{
    // Start is called before the first frame update

    // sets up viewer

    public Transform viewer;
    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // collect Mouse Input

        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the Camera around its local X axis

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Rotate the camera around its y axis

        viewer.Rotate(Vector3.up * inputX);
        viewer.rotation = Quaternion.Euler(viewer.rotation.eulerAngles.x, viewer.rotation.eulerAngles.y, 0f);



        
    }
}