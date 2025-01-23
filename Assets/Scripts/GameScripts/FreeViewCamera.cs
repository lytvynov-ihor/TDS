using UnityEngine;

public class FreeViewCamera : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of movement
    public float rotateSpeed = 150f; // Speed of rotation
    private float pitch = 0f; // Vertical rotation (local X-axis)
    private float yaw = 0f; // Horizontal rotation (local Y-axis)
    private bool isRightMouseDown = false; // Check if RMB is held

    void Start()
    {
        // Initialize yaw and pitch with the current rotation of the object
        Vector3 rotation = transform.eulerAngles;
        yaw = rotation.y;
        pitch = rotation.x;
    }

    void Update()
    {
        HandleMouseInput();
        HandleMovement();
    }

    void HandleMouseInput()
    {
        // Check if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            isRightMouseDown = true;
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center
            Cursor.visible = false; // Hide the cursor
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseDown = false;
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
        }

        // If RMB is held, rotate the camera with mouse movement
        if (isRightMouseDown)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            // Update yaw (horizontal) and pitch (vertical) values
            yaw += mouseX;
            pitch -= mouseY;

            // Clamp the pitch to avoid flipping the camera upside down
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            // Apply rotation to the camera (using local axes)
            transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    void HandleMovement()
    {
        // Get WASD input
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        // Move the player (forward, backward, left, right)
        Vector3 moveDirection = transform.forward * moveZ + transform.right * moveX;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
