using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTAI;

public class BehaviorMinion : MonoBehaviour
{
    public Transform player;
    public Transform playerSafeZone;
    public Transform npcHomePosition;
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float attackRange = 10f;         // distance within the minion will attack
    public float safeZoneRadius = 10f;      // radius of the player's safe zone
    public float followDistance = 20f;     // distance where the minion will follow the player
    public float attackCooldown = 1f;      // cooldown between attacks
    public float projectileSpeed = 10f;    // speed of projectile
    public float moveSpeed = 3.5f;         // enemy npc moving speed
    private Root m_btRoot = BT.Root();
    private float nextAttackTime = 0f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed; 

        m_btRoot.OpenBranch(
            BT.Selector().OpenBranch(
                BT.Sequence().OpenBranch(
                    BT.Condition(() => PlayerInSafeZone()),
                    BT.Call(() => RetreatToHome())
                ),
                BT.Sequence().OpenBranch(
                    BT.Condition(() => PlayerInRange()),
                    BT.Call(() => Attack())
                ),
                BT.Sequence().OpenBranch(
                    BT.Condition(() => PlayerFarAway()),
                    BT.Call(() => FollowPlayer())
                )
            )
        );
    }

    void Update()
    {
        m_btRoot.Tick();
    }

    // check if the player is within attack range
    bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    // check if the player is in their safe zone
    bool PlayerInSafeZone()
    {
        return Vector3.Distance(player.position, playerSafeZone.position) <= safeZoneRadius;
    }

    // check if the player is far enough to follow
    bool PlayerFarAway()
    {
        return Vector3.Distance(transform.position, player.position) > attackRange
               && Vector3.Distance(transform.position, player.position) <= followDistance
               && !PlayerInSafeZone();
    }

    // attack/shoots a projectile at the player if within range and cooldown allows
    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            Debug.Log("Minion shoots at the player!");
            ShootProjectile();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && spawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (player.position - spawnPoint.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }

            Destroy(projectile, 5f);
        }
    }

    // retreat to NPC home when the player is in safe zone
    void RetreatToHome()
    {
        Debug.Log("Minion retreats to its home position.");
        agent.SetDestination(npcHomePosition.position);
    }

    // follow the player when out of attack range
    void FollowPlayer()
    {
        Debug.Log("Minion follows the player.");
        agent.SetDestination(player.position);
    }
}
