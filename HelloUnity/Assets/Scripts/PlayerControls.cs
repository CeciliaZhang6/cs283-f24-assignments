using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;  // speed for forward/backward movement
    public float turnSpeed = 100f;  // speed for turning left/right

    void Update()
    {
        // transformation for moving
        if (Input.GetKey(KeyCode.W))
        {
            if(Input.GetKey(KeyCode.LeftShift)){
                moveSpeed = 10f;
            }
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if(Input.GetKey(KeyCode.LeftShift)){
                moveSpeed = 10f;
            }
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // transformation for turning
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

        // prevent rotation falling over
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, currentRotation.y, 0);
    }
}
