using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace CS283 
{
class AIState
{
    public class Transition
    {
        public System.Func<bool> fn;
        public AIState next;
    }
    public List<Transition> transitions = new List<Transition>();
    private bool isFinished = false;
    private AIState next = null;

    public AIState() { }
    public bool IsTriggered() { return isFinished; }
    public virtual void Enter() { isFinished = false; }
    public virtual void Exit() { }
    public virtual void Update(float dt) 
    {
        foreach (Transition transition in transitions)
        {
            if (transition.fn())
            {
                isFinished = true;
                next = transition.next;
            }
        }
    }
    public AIState Next() { return next; }
}

class AIWander : AIState
{
    public WanderFSM entity;
    public float wanderRadius = 10f;  // distance of NPC can wander from origin

    public AIWander(WanderFSM component)
    {
        entity = component;
    }

    public override void Enter()
    {
       base.Enter();
       NavMeshAgent agent = entity.GetComponent<NavMeshAgent>();

       // Vector3 target = RandomNavSphere(transform.position, wanderRadius, NavMesh.AllAreas);
    //    Utils.RandomPointOnTerrain(
    //       entity.wanderRange.position, 
    //       entity.wanderRange.localScale.x, 
    //       out target);

       //agent.SetDestination(target);
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

    public bool ReachedDestination()
    {
        NavMeshAgent agent = entity.GetComponent<NavMeshAgent>();
        return (agent.remainingDistance < 0.1f);
    }
}

class AIController
{
    private AIState root = null;
    private AIState current = null;

    public AIController(AIState r)
    {
        root = r;
        current = root;
    }

    public void Update(float dt)
    {
       current.Update(dt);
       if (current.IsTriggered())
       {
          current.Exit();
          current = current.Next();
          current.Enter();
       }
    }
}
}

public class WanderFSM : MonoBehaviour
{
    public Transform wanderRange;  // Set to a sphere
    private CS283.AIController m_controller;

    void Start()
    {
        CS283.AIWander wander = new CS283.AIWander(this);

        CS283.AIState.Transition repeat = new CS283.AIState.Transition();
        repeat.fn = wander.ReachedDestination;
        repeat.next = wander;

        wander.transitions.Add(repeat);

        m_controller = new CS283.AIController(wander);
    }

    void Update()
    {
        m_controller.Update(Time.deltaTime);
    }
}