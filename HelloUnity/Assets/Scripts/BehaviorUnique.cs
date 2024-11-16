using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BTAI;

public class BehaviorUnique : MonoBehaviour
{
    public Transform player;
    public float healingRange = 5f;
    public int healingAmount = 20;
    public float healingCooldown = 5f;

    private float nextHealTime = 0f;

    public Text playerHealthText;  

    private Root m_btRoot = BT.Root();

    void Start()
    {
        // initial health display
        playerHealthText.text = "Health: 60 / 100";

        // behavior tree
        m_btRoot.OpenBranch(
            BT.Selector().OpenBranch(
                BT.Sequence().OpenBranch(
                    BT.Condition(() => PlayerInHealingRange()),
                    BT.Call(() => HealPlayer())
                )
            )
        );
    }

    void Update()
    {
        m_btRoot.Tick();
    }

    bool PlayerInHealingRange()
    {
        return Vector3.Distance(transform.position, player.position) <= healingRange;
    }

    void HealPlayer()
    {
        if (Time.time >= nextHealTime)
        {
            Debug.Log("Priest heals the player!");

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healingAmount);
                UpdateHealthUI();
            }

            nextHealTime = Time.time + healingCooldown;
        }
    }

    void UpdateHealthUI()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealthText.text = $"Health: {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}";
        }
    }
}
