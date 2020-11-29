using System.Collections.Generic;
using ShipManagement;
using WorldManagement;

public class Player
{
    public Player(Ship ship)
    {
        Ship = ship;
        ship.Player = this;
        Scrap = 5;
        DiscoveredComponents = new List<int> { 0, 1, 2 };
        
        ShipBuilder.Instance.RefreshComponentsUI();
    }
    
    public Ship Ship { get; set; }
    public int Scrap { get; set; } = 0;
    public List<int> DiscoveredComponents { get; }

    public void AddBlueprint(Blueprint blueprint)
    {
        if (DiscoveredComponents.Contains(blueprint.ComponentToDicover))
            return;
        DiscoveredComponents.Add(blueprint.ComponentToDicover);
        ShipBuilder.Instance.RefreshComponentsUI();
    }
}