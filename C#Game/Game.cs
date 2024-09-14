using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

public class Game
{
    private Player player;
    private List<Enemy> enemies;
    private Random random;
    private int roundCounter;
    private GameState currentState;
    private HotelNPC hotelKeeper;
    private MentorNPC mentor;
    private string interactMsg = "";
    private static Random rand = new Random();
    private bool isAuthorStampVisible = false;


    private enum GameState
    {
        MainMenu,
        Hotel,
        Hotel_Mentor,
        Hotel_Keeper,
        ShowStats,
        Combat_Start,   
        Combat_PlayerTurn, 
        Combat_EnemyTurn,   
        Combat_Result,  
        Combat_End,   
        GameOver
    }


    public Game()
    {
        enemies = new List<Enemy>();
        random = new Random();
    }

    public void Setup()
    {
        player = new Player("Hero");
        hotelKeeper = new HotelNPC();
        mentor = new MentorNPC();
        roundCounter = 1;
        currentState = GameState.MainMenu;
    }   

    public void Update(float dt)
    {
        // auto hp regen under hp of 20
        player.Update(dt);
        
        // goblins lose sanity over time and will become stronger
        foreach (var enemy in enemies)
        {
            enemy.Update(dt);
        }

    }

    public void Draw(Graphics g)
    {
        g.Clear(Color.Black);

        switch (currentState)
        {
            case GameState.MainMenu:
                g.Clear(Color.Black);
                DrawMainMenu(g);
                break;

            case GameState.Hotel:
                g.Clear(Color.Black);
                DrawHotelUI(g);
                break;

            case GameState.Hotel_Mentor:
                g.Clear(Color.Black);
                DrawHotelMentor(g);
                break;

            case GameState.Hotel_Keeper:
                g.Clear(Color.Black);
                DrawHotelKeeper(g);
                break;

            case GameState.ShowStats:
                g.Clear(Color.Black);
                DrawShowStats(g);
                break;

            case GameState.Combat_Start:
                g.Clear(Color.Black);
                DrawCombatStart(g);
                break;

            case GameState.Combat_PlayerTurn:
                g.Clear(Color.Black);
                DrawCombatPlayerTurn(g);
                break;

            case GameState.Combat_EnemyTurn:
                g.Clear(Color.Black);
                DrawCombatEnemyTurn(g);
                break;

            case GameState.Combat_Result:
                g.Clear(Color.Black);
                DrawCombatResult(g);
                break;

            case GameState.Combat_End:
                g.Clear(Color.Black);
                DrawCombatEnd(g);  
                break;

            case GameState.GameOver:
                g.Clear(Color.Black);
                DrawGameOver(g);
                break;
        }


        // draw the interaction message if it exists
        if (!string.IsNullOrEmpty(interactMsg))
        {
            DrawMsg(g);
        }

        DrawAuthorStamp(g);
    }


    public void MouseClick(MouseEventArgs mouse)
    {
        if (mouse.Button == MouseButtons.Left)
        {
            // no mouse clicking for this game. 
        }
    }

    public void KeyDown(KeyEventArgs key)
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                HandleMainMenuKeyPress(key.KeyCode);
                break;
            case GameState.Combat_Start:
                HandleCombatStartKeyPress(key.KeyCode);
                break;
            case GameState.Combat_PlayerTurn:
                HandlePlayerAttackKeyPress(key.KeyCode);
                break;
            case GameState.Combat_EnemyTurn:
                HandleEnemyTurn(key.KeyCode);
                break;
            case GameState.Combat_Result:
                HandleCombatResultKeyPress(key.KeyCode);
                break;
            case GameState.Combat_End:
                HandleCombatEnd();
                break;
            case GameState.Hotel:
                HandleHotelKeyPress(key.KeyCode);
                break;
            case GameState.Hotel_Mentor:
                HandleMentorKeyPress(key.KeyCode);
                break;
            case GameState.Hotel_Keeper:
                HandleKeeperKeyPress(key.KeyCode);
                break;
            case GameState.ShowStats:
                HandleStatsKeyPress(key.KeyCode);
                break;
        }

        HandleKeyPress(key.KeyCode);
    }

    // event handlers
    private void HandleHotelKeyPress(Keys keyCode)
    {
        if (keyCode == Keys.D1)
        {
            currentState = GameState.Hotel_Keeper;
        }
        else if (keyCode == Keys.D2)
        {
            currentState = GameState.Hotel_Mentor;
        }
        else if (keyCode == Keys.D3)
        {
            if (enemies.Count <= 0)
            {
                GenerateEnemies(2 + roundCounter / 10);
            }
            currentState = GameState.Combat_Start;  // go back to battle
            interactMsg = "";
        }

    }

    private void HandleMainMenuKeyPress(Keys keyCode)
    {
        if (keyCode == Keys.D1)
        {
            // entered digit 1
            currentState = GameState.Hotel;
        }
        else if (keyCode == Keys.D2)
        {
            // entered digit 2
            Application.Exit();
        }
    }

    public void HandleMentorKeyPress(Keys keyCode)
    {   
        // mentor randomly increases 1-3 points to the specified stat
        if (keyCode == Keys.D1)
        {
            player.Strength += rand.Next(1, 4);
            interactMsg = "Mentor improved your strength!";
        }
        else if (keyCode == Keys.D2)
        {
            player.Dexterity += rand.Next(1, 4);
            interactMsg = "Mentor improved your dexterity!";
        }
        else if (keyCode == Keys.D3)
        {
            currentState = GameState.Hotel;  // go back to the hotel
            interactMsg = "";
            return;
        }

        currentState = GameState.ShowStats;

    }

    public void HandleKeeperKeyPress(Keys keyCode)
    {
        Random random = new Random();

        if (keyCode == Keys.D1 || keyCode == Keys.D2)
        {
            int hpGain = random.Next(10, 31);  // randomly gain 10-30 HP
            player.Health += hpGain;
            interactMsg = "Health increased.";
            currentState = GameState.ShowStats;
        }
        else if (keyCode == Keys.D3)
        {
            currentState = GameState.Hotel;  // go back to the hotel
            interactMsg = "";
            return;
        }

    }

    private void HandleCombatStartKeyPress(Keys keyCode)
    {
        if (keyCode == Keys.A)
        {
            currentState = GameState.Combat_PlayerTurn;
        }
        else if (keyCode == Keys.F)  
        {
            interactMsg = "You fled the battle!";
            currentState = GameState.Hotel;  // go back to hotel
        }

    }

    private void HandlePlayerAttackKeyPress(Keys keyCode)
    {
        
        if (enemies.Count > 0) 
        {
            int damage = player.Attack(enemies[0]);  // player attacks
            Console.WriteLine($"Dealt {damage} damage.");

      

            if (enemies[0].Health <= 0)
            {
                interactMsg = $"{enemies[0].Name} is defeated!";
                enemies.RemoveAt(0);  
            }
            //else
            //{
            //    currentState = GameState.Combat_EnemyTurn;
            //}
            

        }
        else
        {
            currentState = GameState.Combat_EnemyTurn;
        }


        //if (keyCode == Keys.Space)
        //{
        //    currentState = GameState.Combat_EnemyTurn;
        //}

    }

    private void HandleEnemyTurn(Keys keyCode)
    {
        if (keyCode == Keys.Space && enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                int playerHP = enemy.Attack(player);
                player.Health = playerHP;

                if (player.Health <= 0)
                {
                    interactMsg = "You have been defeated!";
                    currentState = GameState.GameOver;  
                }
                else
                {
                    currentState = GameState.Combat_Result; 
                }
            }

        }

    }

    private void HandleCombatResultKeyPress(Keys keyCode)
    {
        if (keyCode == Keys.Space)
        {
            if (enemies.Count > 0)
            {
                currentState = GameState.Combat_Start; 
            }
            else
            {
                currentState = GameState.Combat_End;  
            }

        }
    }

    private void HandleCombatEnd()
    {
        roundCounter++;
        if (player.Health > 0)
        {
            currentState = GameState.Hotel;  // return to hotel after victory
        }
        else
        {
            currentState = GameState.GameOver;  // game over if player was defeated
        }
    }


    public void HandleStatsKeyPress(Keys keyCode)
    {
        if (keyCode == Keys.Space)
        {
            if (enemies.Count <= 0)
            {
                GenerateEnemies(2 + roundCounter / 10);
            }
            currentState = GameState.Combat_Start;
            interactMsg = "";  
        }
    }

    // key press for toggle author stamp
    private void HandleKeyPress(Keys keyCode)
    {
        if (keyCode == Keys.Oemplus || keyCode == Keys.Add)  // "+" key to show/hide
        {
            isAuthorStampVisible = !isAuthorStampVisible;  // toggle visibility
        }
    }

    // combat system
    private void GenerateEnemies(int count)
    {
        string[] enemyNames = {"Goblin", "Goblin Dwarf", "Tall Goblin", "Thin Goblin"};

        for (int i = 0; i < count; i++)
        {
            string name = enemyNames[random.Next(enemyNames.Length)];
            int strength = 0;
            int health = 0;

            // generate goblin stats by progress
            if (roundCounter <= 10)
            {
                strength = random.Next(1, 4);
                health = random.Next(10, 15);
            }
            else if (roundCounter <= 25 && roundCounter > 10)
            {
                strength = random.Next(8, 14);
                health = random.Next(20, 34);
            }
            else if (roundCounter > 25)
            {
                strength = random.Next(16, 31);
                health = random.Next(50, 71);
            }

            enemies.Add(new Enemy(name, strength, health));
        }
    }

    // draw methods
    private void DrawMainMenu(Graphics g)
    {
        g.DrawString("2000s RPG Farming Game", new Font("Arial", 24), Brushes.White, 10, 10);
        g.DrawString("1. Start Game", new Font("Arial", 16), Brushes.White, 10, 50);
        g.DrawString("2. Quit", new Font("Arial", 16), Brushes.White, 10, 80);
    }

    private void DrawHotelUI(Graphics g)
    {
        g.DrawString("You have entered the hotel.", new Font("Arial", 16), Brushes.White, 10, 10);
        g.DrawString("1. Visit Hotel Keeper", new Font("Arial", 16), Brushes.White, 10, 50);
        g.DrawString("2. Visit Mentor", new Font("Arial", 16), Brushes.White, 10, 80);
        g.DrawString("3. Leave Hotel. Back to the battle ground.", new Font("Arial", 16), Brushes.White, 10, 110);
    }

    private void DrawGameOver(Graphics g)
    {
        g.DrawString("Game Over", new Font("Arial", 28), Brushes.Red, 10, 10);
        
    }

    public void DrawHotelMentor(Graphics g)
    {
        g.DrawString("Mentor Options:", new Font("Arial", 16), Brushes.White, 10, 10);
        g.DrawString("1. Improve Strength", new Font("Arial", 16), Brushes.White, 10, 50);
        g.DrawString("2. Improve Dexterity", new Font("Arial", 16), Brushes.White, 10, 80);
        g.DrawString("3. Exit Mentor Corner", new Font("Arial", 16), Brushes.White, 10, 110);
    }

    public void DrawHotelKeeper(Graphics g)
    {
        g.DrawString("Hotel Keeper offered you a room to rest.", new Font("Arial", 16), Brushes.White, 10, 10);
        g.DrawString("1. Thank you!", new Font("Arial", 16), Brushes.White, 10, 50);
        g.DrawString("2. For free? Lucky me!", new Font("Arial", 16), Brushes.White, 10, 80);
        g.DrawString("3. I want to chat with the Mentor. Leave front desk. ", new Font("Arial", 16), Brushes.White, 10, 110);
    }

    private void DrawMsg(Graphics g)
    {
        g.DrawString(interactMsg, new Font("Arial", 16), Brushes.White, 10, 200);
        interactMsg = ""; // reset
    }

    public void DrawShowStats(Graphics g)
    {
        g.DrawString("Player Stats:", new Font("Arial", 16), Brushes.White, 10, 10);
        g.DrawString($"Strength: {player.Strength}", new Font("Arial", 16), Brushes.White, 10, 40);
        g.DrawString($"Dexterity: {player.Dexterity}", new Font("Arial", 16), Brushes.White, 10, 70);
        g.DrawString($"Health: {player.Health}", new Font("Arial", 16), Brushes.White, 10, 100);

        if (!string.IsNullOrEmpty(interactMsg))
        {
            g.DrawString(interactMsg, new Font("Arial", 16), Brushes.White, 10, 200);
        }

        g.DrawString("Press Space to continue.", new Font("Arial", 16), Brushes.White, 10, 250);

    }

    private void DrawCombatScene(Graphics g)
    {
        g.DrawString($" ~~ Round: {roundCounter} ~~ Scene: {currentState}", new Font("Arial", 16), Brushes.White, 10, 10);
        g.DrawString($" ~~ Player HP: {player.Health} Player Strength: {player.Strength}", new Font("Arial", 16), Brushes.White, 10, 40);
        for (int i = 0; i < enemies.Count; i++)
        {
            g.DrawString($" ** [{enemies[i].Name}] HP: {enemies[i].Health} Strength: {enemies[i].Strength}", new Font("Arial", 16), Brushes.White, 10, 70 + i * 30);
        }
    }

    private void DrawCombatStart(Graphics g)
    {
        g.Clear(Color.Black);
        DrawCombatScene(g);
        g.DrawString("A. Attack", new Font("Arial", 16), Brushes.White, 10, 80 + enemies.Count * 30);
        g.DrawString("F. Fleet", new Font("Arial", 16), Brushes.White, 10, 110 + enemies.Count * 30);
    }


    public void DrawCombatPlayerTurn(Graphics g)
    {
        g.Clear(Color.Black);

        g.DrawString($"~~ Round: {roundCounter} ~~ ", new Font("Arial", 16), Brushes.White, 10, 10);

        g.FillRectangle(Brushes.Black, 10, 40, 300, 30);  
        
        g.DrawString($"++ Player HP: {player.Health} Player Strength: {player.Strength} ++", new Font("Arial", 16), Brushes.White, 10, 40);

        
        for (int i = 0; i < enemies.Count; i++)
        {
            g.FillRectangle(Brushes.Black, 10, 70 + i * 30, 300, 30);  
            g.DrawString($"** [{enemies[i].Name}] HP: {enemies[i].Health} Strength: {enemies[i].Strength} **", new Font("Arial", 16), Brushes.White, 10, 70 + i * 30);
        }
     
        g.DrawString("You attacked the enemy! Press SPACE to continue.", new Font("Arial", 16), Brushes.White, 10, 150 + enemies.Count * 30);
    }


    public void DrawCombatEnemyTurn(Graphics g)
    {
        g.Clear(Color.Black);
        DrawCombatScene(g);
        g.DrawString("Enemy's turn...", new Font("Arial", 16), Brushes.White, 10, 80 + enemies.Count * 30);
        for (int i = 0; i < enemies.Count; i++)
        {
            g.DrawString($" ** [{enemies[i].Name}] dealt {enemies[i].Strength} damage. ", new Font("Arial", 16), Brushes.White, 10, 70 + i * 30);
        }
        g.DrawString($"Press SPACE to continue.", new Font("Arial", 16), Brushes.White, 10, 110 + enemies.Count * 30);
    }

    public void DrawCombatResult(Graphics g)
    {
        g.Clear(Color.Black);
        g.DrawString("Combat Results:", new Font("Arial", 16), Brushes.White, 10, 10);
        DrawCombatScene(g);
        for (int i = 0; i < enemies.Count; i++)
        {
            g.DrawString($" ** [{enemies[i].Name}] dealt {enemies[i].Strength} damage. ", new Font("Arial", 16), Brushes.White, 10, 70 + i * 30);
        }
        g.DrawString("Press SPACE to continue", new Font("Arial", 16), Brushes.White, 10, 110 + enemies.Count * 30);
    }

    public void DrawCombatEnd(Graphics g)
    {
        if (player.Health > 0)
        {
            g.DrawString("Well done, Hero! You have defeated the goblins.", new Font("Arial", 16), Brushes.White, 10, 10);
            g.DrawString("Press SPACE to head back to the hotel.", new Font("Arial", 16), Brushes.White, 10, 40);
        }
        else
        {
            g.DrawString("You were defeated...", new Font("Arial", 16), Brushes.White, 10, 10);
            g.DrawString("Press SPACE to continue.", new Font("Arial", 16), Brushes.White, 10, 40);
        }
    }

    public void DrawAuthorStamp(Graphics g)
    {
        if (isAuthorStampVisible)
        {
            // stamp size & location (top left)
            Rectangle stampRect = new Rectangle(500, 10, 150, 50);

            // black rectangle
            g.FillRectangle(Brushes.Black, stampRect);

            g.DrawString("Cecilia Zhang '25", new Font("Arial", 11), Brushes.White, 500, 15);
            g.DrawString("2000s RPG Farming Game", new Font("Arial", 11), Brushes.White, 500, 30);
        }
    }



}