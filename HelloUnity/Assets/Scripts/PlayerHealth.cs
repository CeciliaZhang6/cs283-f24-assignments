using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth => currentHealth;  
    public int MaxHealth => maxHealth;         

    void Start()
    {
        currentHealth = 60;  // start game with 60 HP
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log($"Player health restored. Current health: {currentHealth}");
    }
}
