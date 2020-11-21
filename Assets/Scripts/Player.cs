using System;

public class Player
{
    public Player(Ship ship)
    {
        Ship = ship;
        ship.Player = this;
    }
    
    public Ship Ship { get; private set; }
    public int Scrap { get; set; } = 0;

    public void AddBlueprint(Blueprint blueprint)
    {
        throw new NotImplementedException();
    }
}