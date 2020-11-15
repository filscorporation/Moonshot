using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public List<Component> Components { get; set; }

    public void Initialize()
    {
        Components = new List<Component>();
        Cabin cabin = Instantiate(ShipBuilder.Instance.CabinPrefab).GetComponent<Cabin>();
        cabin.transform.SetParent(transform);
        cabin.Initialize();
        cabin.X = 0;
        cabin.Y = 0;
        cabin.Disable();
        Components.Add(cabin);
    }

    public void AddComponent(Component newComponent)
    {
        newComponent.transform.SetParent(transform);
        
        foreach (Component component in Components)
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
        
        Components.Add(newComponent);
    }

    public void Simulate()
    {
        foreach (Component component in Components)
        {
            component.Enable();
        }
    }
}