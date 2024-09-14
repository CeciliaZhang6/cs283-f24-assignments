using System;
using System.Drawing;

public class Enemy
{
    public string Name { get; set; }
    public int Strength { get; set; }
    public int Health { get; set; }
    public int Sanity { get; set; }

    public Enemy(string name, int strength, int health)
    {
        Name = name;
        Strength = strength;
        Health = health;
        Sanity = 40;
    }

    public void Update(float dt)
    {
        // enemy gains 5 extra strength after sanity <= 0
        // enemy loses 2 sanity per sec
        if (Sanity > 0)
        {
            Sanity -= (int)(dt * 2); 
            Sanity = Math.Max(Sanity, 0);
            if (Sanity < 0)
            {
                Strength += 5;
                Name += " (null sanity)";
            }
        }

    }


    public int TakeDamage(int damage)
    {
        Health -= damage;
        return Health;
    }

    public int Attack(Player player)
    {
        return player.TakeDamage(Strength);
    }
}
