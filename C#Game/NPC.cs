using System;

public abstract class NPC
{
    public string Name { get; set; }

}

public class HotelNPC : NPC
{
    public HotelNPC()
    {
        Name = "Hotel Keeper";
    }
}

public class MentorNPC : NPC
{
    public MentorNPC()
    {
        Name = "Mentor";
    }

}
