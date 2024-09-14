using System;
using System.Drawing;

public class Player
{
    public string Name { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Health { get; set; }

    private static Random rand = new Random();

    public Player(string name)
    {
        Name = name;
        Strength = rand.Next(1, 11);
        Dexterity = rand.Next(1, 11);
        Health = 20;
    }
    
    public void Update(float dt)
    {
        // heal 3 HP per sec when under 20 health
        if (Health < 20)
        {
            Health += (int)(dt * 3); 
            Health = Math.Min(Health, 20);
        }
    }

    // attack deals damage based on Strength
    public int Attack(Enemy enemy)
    {
        Console.WriteLine($"{Name} attacking!");

        int isCritical = rand.Next(0, 101);
        int dmg;

        if (isCritical < Dexterity * 5)  
        {
            dmg = (int)(Strength * 1.5);
            Console.WriteLine("Critical hit!");
        }
        else  
        {
            dmg = Strength;
        }

        enemy.TakeDamage(dmg);
        return dmg;  
    }


    // returns dmg taken
    public int TakeDamage(int damage)
    {
        Health -= damage;
        return Health;
    }
}
