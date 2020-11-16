using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    private const float MIN_SAFE_MAGNITUDE = 4f;
    private const float MAGNITUDE_TO_DAMAGE = 0.1f;
    
    public List<ShipComponent> Components { get; set; }
    public int Toughness => Components.Sum(c => c.Toughness);
    public float DamageTaken { get; set; }

    public void Initialize()
    {
        Components = new List<ShipComponent>();
        Cabin cabin = Instantiate(ShipBuilder.Instance.CabinPrefab).GetComponent<Cabin>();
        cabin.transform.SetParent(transform);
        cabin.Ship = this;
        cabin.Index = 0;
        cabin.Initialize();
        cabin.X = 0;
        cabin.Y = 0;
        cabin.Disable();
        Components.Add(cabin);
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
    }

    public void RemoveComponent(ShipComponent removedComponent)
    {
        // TODO: check if none of next components depends (attached) to this, recalulate all indexes
        
        throw new NotImplementedException();
    }

    public void Simulate()
    {
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
            Debug.Log("Ship destroyed");
        }
    }
}