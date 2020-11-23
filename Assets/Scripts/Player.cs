﻿using System;
using ShipManagement;
using WorldManagement;

public class Player
{
    public Player(Ship ship)
    {
        Ship = ship;
        ship.Player = this;
    }
    
    public Ship Ship { get; set; }
    public int Scrap { get; set; } = 0;

    public void AddBlueprint(Blueprint blueprint)
    {
        throw new NotImplementedException();
    }
}