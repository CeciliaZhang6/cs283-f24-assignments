using UnityEngine;

public class GazeController : MonoBehaviour
{
    public Transform target;            // gaze target
    public Transform lookJoint;         // head joint (used Ethan's body in my character - can't find his head)

    void Update()
    {
        if (target == null || lookJoint == null)
        {
            return; // exit if no target or look joint
        }

        // direction from look joint to target
        Vector3 directionToTarget = target.position - lookJoint.position;

        // visualize line from look joint to target
        Debug.DrawLine(lookJoint.position, target.position, Color.red);

        // current forward direction of look joint
        Vector3 currentForward = lookJoint.forward;

        // rotation axis using cross product
        Vector3 rotationAxis = Vector3.Cross(currentForward, directionToTarget.normalized);

        // angle between current forward direction and direction to the target
        float angle = Vector3.Angle(currentForward, directionToTarget);

        // rotation needed to target
        Quaternion rotationToTarget = Quaternion.AngleAxis(angle, rotationAxis);

        // apply rotation
        if (lookJoint.parent != null)
        { 
            lookJoint.parent.rotation = rotationToTarget * lookJoint.parent.rotation;
        }
    }
}
