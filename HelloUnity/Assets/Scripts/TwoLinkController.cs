using UnityEngine;

public class TwoLinkController : MonoBehaviour
{
    public Transform target;          // The target the end effector should reach
    public Transform endEffector;     // The end effector (e.g., hand or foot)

    private Transform middleJoint;    // The middle joint (e.g., elbow)
    private Transform baseJoint;      // The base joint (e.g., shoulder)

    void Start()
    {
        // Initialize the middle and base joints based on the hierarchy
        if (endEffector != null)
        {
            middleJoint = endEffector.parent;
            if (middleJoint != null)
            {
                baseJoint = middleJoint.parent;
            }
        }

        // Ensure the setup is correct
        if (baseJoint == null)
        {
            Debug.LogError("TwoLinkController: The hierarchy of joints is not set up correctly.");
        }
    }

    void Update()
    {
        if (target == null || endEffector == null || middleJoint == null || baseJoint == null)
        {
            return; // Exit if any of the required components are missing
        }

        // Step 1: Calculate the distance from the base joint to the target
        float distanceToTarget = Vector3.Distance(baseJoint.position, target.position);

        // Step 2: Calculate the length of each bone segment
        float length1 = Vector3.Distance(baseJoint.position, middleJoint.position);
        float length2 = Vector3.Distance(middleJoint.position, endEffector.position);

        // Step 3: Check if the target is reachable
        if (distanceToTarget > length1 + length2)
        {
            Debug.LogWarning("Target is out of reach.");
            // Stretch the arm fully in the direction of the target
            Vector3 direction = (target.position - baseJoint.position).normalized;
            middleJoint.position = baseJoint.position + direction * length1;
            endEffector.position = middleJoint.position + direction * length2;
        }
        else
        {
            // Step 4: Use the Law of Cosines to find the angles for the two joints
            float angle1 = CalculateAngle(length1, distanceToTarget, length2);
            float angle2 = CalculateAngle(length2, length1, distanceToTarget);

            // Step 5: Position the middle joint and end effector
            Vector3 bendDirection = Vector3.Cross(baseJoint.forward, target.position - baseJoint.position).normalized;
            Quaternion rotation1 = Quaternion.AngleAxis(angle1, bendDirection);
            Quaternion rotation2 = Quaternion.AngleAxis(-angle2, bendDirection);

            middleJoint.position = baseJoint.position + rotation1 * (target.position - baseJoint.position).normalized * length1;
            endEffector.position = middleJoint.position + rotation2 * (target.position - middleJoint.position).normalized * length2;
        }

        // Draw the bend axis for visualization
        Debug.DrawLine(baseJoint.position, middleJoint.position, Color.green);
        Debug.DrawLine(middleJoint.position, endEffector.position, Color.blue);
        Debug.DrawLine(baseJoint.position, target.position, Color.red);

        // Step 6: Check if the end effector is at the target
        float distanceToEndEffector = Vector3.Distance(baseJoint.position, endEffector.position);
        if (Mathf.Abs(distanceToEndEffector - distanceToTarget) < 0.01f)
        {
            Debug.Log("End effector is correctly positioned on the target.");
        }
        else
        {
            Debug.LogWarning("End effector is not correctly aligned with the target.");
        }
    }

    // Helper function to calculate angles using the Law of Cosines
    float CalculateAngle(float a, float b, float c)
    {
        // Angle opposite of side b
        return Mathf.Acos((a * a + c * c - b * b) / (2 * a * c)) * Mathf.Rad2Deg;
    }
}
