using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed for forward/backward movement
    public float turnSpeed = 100f;  // Speed for turning left/right

    void Update()
    {
        // Adjust move speed based on LeftShift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = 5f;
        }

        // Moving forward/backward
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
            Debug.Log("Position after W: " + transform.position);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
            Debug.Log("Position after S: " + transform.position);
        }

        // Turning left/right
        if (Input.GetKey(KeyCode.A))
        {
            Quaternion turnLeft = Quaternion.Euler(0, -turnSpeed * Time.deltaTime, 0);
            transform.rotation = transform.rotation * turnLeft;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Quaternion turnRight = Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
            transform.rotation = transform.rotation * turnRight;
        }

        // Prevent tilting
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, currentRotation.y, 0);
    }
}
