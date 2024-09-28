using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float mouseSensitivity = 3.0f;  // mouse sensitivity for camera rotation
    public float moveSpeed = 10.0f;        // speed for camera movement
    public float fastMoveSpeed = 20.0f;    // faster movement when holding shift
    public float minHeight = 1.0f;         // minimum height for the camera

    private float rotationX = 0.0f;        // horizontal rotation
    private float rotationY = 0.0f;        // vertical rotation

    void Update()
    {
        // get mouse movement for rotating
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // apply rotation
        rotationX += mouseX;
        rotationY -= mouseY;  
        rotationY = Mathf.Clamp(rotationY, -90, 90);  
        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);

        // get keyboard input (WASD) for moving
        float moveX = Input.GetAxis("Horizontal");  // press A/D or Left/Right arrow for left/right movement
        float moveZ = Input.GetAxis("Vertical");    // press W/S or Up/Down arrow for forward/backward movement

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        // move faster when holding shift (temporaly disabled)
        float speed = Input.GetKey(KeyCode.LeftShift) ? fastMoveSpeed : moveSpeed;

        // apply movement
        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;

        // ensure the camera stays above the minimum height
        newPosition.y = Mathf.Max(newPosition.y, minHeight);

        transform.position = newPosition;
    }
}
