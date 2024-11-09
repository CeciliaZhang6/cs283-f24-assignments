using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
    public float wanderRadius = 10f;  // distance of NPC can wander from origin
    public float minDistanceToTarget = 1f;  // distance for arrival, finding next destination

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewRandomDestination(); // init
    }

    void Update()
    {
        // check if close to current destination
        if (!agent.pathPending && agent.remainingDistance <= minDistanceToTarget)
        {
            SetNewRandomDestination(); 
        }
    }

    void SetNewRandomDestination()
    {
        // get random point on NavMesh within radius
        Vector3 randomPoint = RandomNavSphere(transform.position, wanderRadius, NavMesh.AllAreas);
        agent.SetDestination(randomPoint);  // move NPC
    }

    // generate a random point on NavMesh within a given distance
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int areaMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;  // random direction within a sphere
        randomDirection += origin;

        NavMeshHit navHit;
        // nearest valid point on NavMesh
        NavMesh.SamplePosition(randomDirection, out navHit, distance, areaMask);

        return navHit.position;
    }
}
