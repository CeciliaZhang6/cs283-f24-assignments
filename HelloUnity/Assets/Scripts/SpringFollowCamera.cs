using UnityEngine;

public class SpringFollowCamera : MonoBehaviour
{
    public Transform target;               
    public float horizontalDistance = 5f;  
    public float verticalDistance = 3f;    
    public float springConstant = 50f;    
    public float dampConstant = 5f;        // control how fast it slows down
    public float tiltAngle = 10f;

    private Vector3 velocity = Vector3.zero;  
    private Vector3 actualPosition; 

    void Start()
    {
        if (target != null)
        {
            Vector3 tPos = target.position;
            Vector3 tUp = target.up;
            Vector3 tForward = target.forward;

            actualPosition = tPos - tForward * horizontalDistance + tUp * verticalDistance;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 tPos = target.position;
            Vector3 tUp = target.up;
            Vector3 tForward = target.forward;

            Vector3 idealEye = tPos - tForward * horizontalDistance + tUp * verticalDistance;

            Vector3 cameraForward = tPos - actualPosition;

            Vector3 displacement = actualPosition - idealEye;

            Vector3 springAccel = (-springConstant * displacement) - (dampConstant * velocity);

            velocity += springAccel * Time.deltaTime;

            actualPosition += velocity * Time.deltaTime;

            transform.position = actualPosition;

            Quaternion rotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = rotation * Quaternion.Euler(tiltAngle, 0, 0); 
        }
    }
}
