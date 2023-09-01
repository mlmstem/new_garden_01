using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 0.8f;


    [SerializeField] float maxZoomOut = -10.0f;  // Adjust the maximum zoom-out level


    Vector3 originalCameraPosition;




    float cameraPitch = 0.0f;



    void Start()
    {
         originalCameraPosition = playerCamera.localPosition;

        // Set the initial camera position to face the front of the terrain
        /**
        float initialDistance = 5.0f; 
        // Adjust the distance from the terrain
        Vector3 initialPosition = transform.position + Vector3.forward * initialDistance;
        playerCamera.localPosition = initialPosition;
        **/

        
    }

    // Update is called once per frame
    void Update()

    {
        UpdateMouseLook();   
    }

    
void UpdateMouseLook()
{
    Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


    // camemraPitch -> vertical movement
    cameraPitch += mouseDelta.y * mouseSensitivity;

    // clamp movement is calculated with the angles /(direction)

    cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

    playerCamera.localEulerAngles = Vector3.right * cameraPitch;

    transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);


    // zoom in (out) setting 

    float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
    const float zoomSpeed = 2.0f;

    Vector3 newCameraPosition = playerCamera.localPosition + Vector3.forward * zoomAmount * zoomSpeed;

    // Apply limits to zoom out
    newCameraPosition.z = Mathf.Clamp(newCameraPosition.z, maxZoomOut, float.MaxValue);

    playerCamera.localPosition = newCameraPosition;
}
}
