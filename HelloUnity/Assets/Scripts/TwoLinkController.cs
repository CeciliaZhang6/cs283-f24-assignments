using UnityEngine;

public class TwoLinkController : MonoBehaviour
{
    public Transform target;     
    public Transform endEffector;    
    public Vector3 bendAxis = Vector3.forward; 

    private Transform middleJoint; 
    private Transform baseJoint; 

    void Start()
    {
        if (endEffector != null)
        {
            middleJoint = endEffector.parent;
            if (middleJoint != null)
            {
                baseJoint = middleJoint.parent;
            }
        }

        if (baseJoint == null)
        {
            Debug.LogError("TwoLinkController: The hierarchy of joints is not set up correctly.");
        }
    }

    void Update()
    {
        if (target == null || endEffector == null || middleJoint == null || baseJoint == null)
        {
            return;
        }

        // distance from base joint to target
        float distanceToTarget = Vector3.Distance(baseJoint.position, target.position);

        // length of each bone segment
        float length1 = Vector3.Distance(baseJoint.position, middleJoint.position);
        float length2 = Vector3.Distance(middleJoint.position, endEffector.position);

        // check if target is reachable
        if (distanceToTarget > length1 + length2)
        {
            Debug.Log("Target is out of reach, stretching fully.");
            // stretch arm fully 
            Vector3 direction = (target.position - baseJoint.position).normalized;
            middleJoint.position = baseJoint.position + direction * length1;
            endEffector.position = middleJoint.position + direction * length2;
        }
        else
        {
            // find angles for the two joints
            float angle1 = CalculateAngle(length1, distanceToTarget, length2);
            float angle2 = CalculateAngle(length2, length1, distanceToTarget);

            // position middle joint and end effector
            Vector3 baseToTarget = (target.position - baseJoint.position).normalized;
            Vector3 bendDirection = Vector3.Cross(baseToTarget, bendAxis).normalized;
            Quaternion rotation1 = Quaternion.AngleAxis(angle1, bendDirection);
            Quaternion rotation2 = Quaternion.AngleAxis(-angle2, bendDirection);

            middleJoint.position = baseJoint.position + rotation1 * baseToTarget * length1;
            endEffector.position = middleJoint.position + rotation2 * (target.position - middleJoint.position).normalized * length2;
        }

        Debug.DrawLine(baseJoint.position, middleJoint.position, Color.green);
        Debug.DrawLine(middleJoint.position, endEffector.position, Color.blue);
        Debug.DrawLine(baseJoint.position, target.position, Color.red);
        Debug.DrawLine(middleJoint.position, middleJoint.position + bendAxis, Color.yellow);
        Debug.Log($"Distance to target: {distanceToTarget:F2}, Target position: {target.position}");
    }

    // calculates angles using law of cosine
    float CalculateAngle(float a, float b, float c)
    {
        return Mathf.Acos((a * a + c * c - b * b) / (2 * a * c)) * Mathf.Rad2Deg;
    }
}
