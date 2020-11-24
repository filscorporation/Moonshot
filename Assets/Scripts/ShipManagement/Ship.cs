using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ShipManagement.Components;
using UnityEngine;
using WorldManagement;

namespace ShipManagement
{
    public class Ship : MonoBehaviour
    {
        #region Constants

        private const float MIN_SAFE_MAGNITUDE = 4f;
        private const float MAGNITUDE_TO_DAMAGE = 0.1f;

        #endregion

        #region Properties

        public Player Player { get; set; }
        public List<ShipComponent> Components { get; private set; }

        public int Toughness => toughness ?? (toughness = Components.Sum(c => c.Toughness)).Value;
        public float DamageTaken { get; set; }
        public bool IsActive { get; private set; }
        public bool IsLoaded { get; private set; }

        #endregion

        #region Attributes

        private int? toughness;

        #endregion

        #region Data

        [Serializable]
        public class ShipData
        {
            public List<ShipComponent.ComponentData> Components;
            public float DamageTaken;
        }

        public ShipData ToData()
        {
            return new ShipData
            {
                Components = Components.Select(c => c.ToData()).ToList(),
                DamageTaken = DamageTaken,
            };
        }

        public static Ship FromData(ShipData data)
        {
            Ship ship = ShipBuilder.Instance.Initialize();
            List<ShipComponent> components = new List<ShipComponent>();
            foreach (ShipComponent.ComponentData componentData in data.Components)
            {
                if (componentData.TypeIndex == 0) // Cabin created on ShipBuilder initialization
                    continue;
                components.Add(ShipComponent.FromData(componentData));
            }

            // Waiting one frame of physiscs for components collider and rigidbody to setup
            // Otherwise fixedjoint corrupts on rotated components
            ship.StartCoroutine(FromDataCoroutine(ship, components));

            ship.DamageTaken = data.DamageTaken;

            return ship;
        }

        private static IEnumerator FromDataCoroutine(Ship ship, List<ShipComponent> components)
        {
            yield return new WaitForFixedUpdate();
            foreach (ShipComponent shipComponent in components)
            {
                ship.AddComponent(shipComponent);
            }

            ship.IsLoaded = true;
        }

        #endregion

        #region Public methods

        public void Initialize()
        {
            Components = new List<ShipComponent>();
            Cabin cabin = (Cabin) ShipBuilder.Instance.InstantiateComponent(0);
            cabin.OnPlaced();
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
            newComponent.transform.position = transform.position + new Vector3(newComponent.X, newComponent.Y);
            newComponent.OnPlaced();
            toughness = null;
        }

        public bool RemoveComponent(ShipComponent removedComponent)
        {
            if (GetDependantComponents(removedComponent).Any())
                return false;
        
            Components.RemoveAt(removedComponent.Index);
            for (int i = removedComponent.Index; i < Components.Count; i++)
            {
                Components[i].Index = i;
            }
            Destroy(removedComponent.gameObject);
            
            toughness = null;

            return true;
        }

        public void ForceRemoveComponent(ShipComponent removedComponent)
        {
            foreach (ShipComponent dependantComponent in GetDependantComponents(removedComponent).ToList())
            {
                Components.Remove(dependantComponent);
                Destroy(dependantComponent);
            }
        
            Components.Remove(removedComponent);
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Index = i;
            }
            Destroy(removedComponent.gameObject);
            
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

            if (DamageTaken > Toughness)
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

        private IEnumerable<ShipComponent> GetDependantComponents(ShipComponent baseComponent)
        {
            if (baseComponent is Cabin)
            {
                foreach (ShipComponent component in Components)
                {
                    yield return component;
                }
            }

            if (baseComponent.Neighbours.Count(n => n != null) <= 1)
                yield break;

            foreach (ShipComponent neighbour in baseComponent.Neighbours.Where(n => n != null))
            {
                bool[] passedComponents = new bool[Components.Count];
                bool AttachedToCabin(ShipComponent component, ShipComponent toAvoid)
                {
                    if (passedComponents[component.Index] || component.Index == toAvoid.Index)
                        return false;
                    
                    if (component is Cabin)
                        return true;

                    passedComponents[component.Index] = true;
                    
                    return component.Neighbours.Any(n => n != null && AttachedToCabin(n, baseComponent));
                }

                if (!AttachedToCabin(neighbour, baseComponent))
                {
                    for (int i = 0; i < passedComponents.Length; i++)
                    {
                        if (passedComponents[i])
                            yield return Components[i];
                    }
                }
            }
        }

        #endregion
    }
}