using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float scrollWheel = 5f;

    void Update()
    {
        //get the Input from Horizontal axis
        float horizontalInput = Input.GetAxis("Horizontal");
        //get the Input from Vertical axis
        float verticalInput = Input.GetAxis("Vertical");
        // Capture mouse wheel input
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        //update the position/rotation
        transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, 0, verticalInput * movementSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, scrollWheelInput * scrollWheel, Space.World);
    }
}
