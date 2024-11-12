using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BTAI;

public class WanderBehavior : MonoBehaviour
{
    // public Transform wanderRange;  // Set to a sphere
    public float wanderRadius = 10f;  // distance of NPC can wander from origin
    private Root m_btRoot = BT.Root(); 

    void Start()
    {
        BTNode moveTo = BT.RunCoroutine(MoveToRandom);

        Sequence sequence = BT.Sequence();
        sequence.OpenBranch(moveTo);

        m_btRoot.OpenBranch(sequence);
    }

    void Update()
    {
        m_btRoot.Tick();
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

    IEnumerator<BTState> MoveToRandom()
    {
       NavMeshAgent agent = GetComponent<NavMeshAgent>();

       Vector3 target = RandomNavSphere(transform.position, wanderRadius, NavMesh.AllAreas);;
    //    Utils.RandomPointOnTerrain(
    //       wanderRange.position, wanderRange.localScale.x, out target);
        
       agent.SetDestination(target);

       // wait for agent to reach destination
       while (agent.remainingDistance > 0.1f)
       {
          yield return BTState.Continue;
       }

       yield return BTState.Success;
    }
}