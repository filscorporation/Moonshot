using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Constants

    private const float MIN_SAFE_MAGNITUDE = 4f;
    private const float MAGNITUDE_TO_DAMAGE = 0.1f;

    #endregion

    #region Properties

    public Player Player { get; set; }
    public List<ShipComponent> Components { get; set; }

    public int Toughness => toughness ?? (toughness = Components.Sum(c => c.Toughness)).Value;
    public float DamageTaken { get; set; }
    public bool IsActive { get; private set; }

    #endregion

    #region Attributes

    private int? toughness;

    #endregion

    #region Public methods

    public void Initialize()
    {
        Components = new List<ShipComponent>();
        Cabin cabin = Instantiate(ShipBuilder.Instance.CabinPrefab).GetComponent<Cabin>();
        cabin.OnPlaced();
        cabin.Initialize();
        cabin.Disable();
        cabin.X = 0;
        cabin.Y = 0;
        AddComponent(cabin);
    }

    public void AddComponent(ShipComponent newComponent)
    {
        newComponent.transform.SetParent(transform);
        newComponent.Ship = this;
        
        foreach (ShipComponent component in Components)
        {
            if (component.X == newComponent.X && component.Y == newComponent.Y)
                throw new InvalidOperationException();

            if (component.X == newComponent.X + 1 && component.Y == newComponent.Y)
                newComponent.Right = component;

            if (component.X == newComponent.X - 1 && component.Y == newComponent.Y)
                newComponent.Left = component;

            if (component.X == newComponent.X && component.Y == newComponent.Y + 1)
                newComponent.Up = component;

            if (component.X == newComponent.X && component.Y == newComponent.Y - 1)
                newComponent.Down = component;
        }

        newComponent.Index = Components.Count;
        Components.Add(newComponent);
        newComponent.OnPlaced();
        toughness = null;
    }

    public void RemoveComponent(ShipComponent removedComponent)
    {
        // TODO: check if none of next components depends (attached) to this, recalulate all indexes
        
        throw new NotImplementedException();
        
        toughness = null;
    }

    public void Simulate()
    {
        IsActive = true;
        
        foreach (ShipComponent component in Components)
        {
            component.Enable();
        }
    }

    public void RegisterDamage(float magnitude)
    {
        float damage = Mathf.Pow(Mathf.Max(0, magnitude - MIN_SAFE_MAGNITUDE), 2) * MAGNITUDE_TO_DAMAGE;
        DamageTaken += damage;

        if (damage > Toughness)
        {
            Destroy();
        }
    }

    public void PickUp(PickupObject picked)
    {
        if (!IsActive)
            return;
        
        switch (picked)
        {
            case Scrap scrap:
                Player.Scrap += scrap.ScrapAmount;
                break;
            case Blueprint blueprint:
                Player.AddBlueprint(blueprint);
                break;
            default:
                throw new IndexOutOfRangeException();
        }
        
        picked.Destroy();
    }

    #endregion

    #region Private methods

    private void Destroy()
    {
        IsActive = false;
        
        foreach (ShipComponent component in Components)
        {
            component.Disconnect();
        }
    }

    #endregion
}