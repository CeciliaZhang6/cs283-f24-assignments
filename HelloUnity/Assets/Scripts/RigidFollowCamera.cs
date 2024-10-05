using UnityEngine;

public class RigidFollowCamera : MonoBehaviour
{
    public Transform target;           
    // offsets  
    public float horizontalDistance = 5f; 
    public float verticalDistance = 3f; 
    public float cameraTiltAngle = 10f;  

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 tPos = target.position;
            Vector3 tUp = target.up;
            Vector3 tForward = target.forward;

            Vector3 eye = tPos - tForward * horizontalDistance + tUp * verticalDistance;

            Vector3 cameraForward = tPos - eye;

            transform.position = eye;

            Quaternion rotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = rotation * Quaternion.Euler(cameraTiltAngle, 0, 0);
        }
    }
}
