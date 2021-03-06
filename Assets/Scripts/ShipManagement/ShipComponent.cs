﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShipManagement
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class ShipComponent : MonoBehaviour
    {
        #region Properties

        public ShipComponent Up
        {
            get => up;
            set
            {
                if (up != null)
                    throw new InvalidOperationException();
            
                up = value;
                value.down = this;
                Attach(value);
            }
        }
        public ShipComponent Down
        {
            get => down;
            set
            {
                if (down != null)
                    throw new InvalidOperationException();
            
                down = value;
                value.up = this;
                Attach(value);
            }
        }
        public ShipComponent Right
        {
            get => right;
            set
            {
                if (right != null)
                    throw new InvalidOperationException();
            
                right = value;
                value.left = this;
                Attach(value);
            }
        }
        public ShipComponent Left
        {
            get => left;
            set
            {
                if (left != null)
                    throw new InvalidOperationException();
            
                left = value;
                value.right = this;
                Attach(value);
            }
        }
    
        public List<ShipComponent> Neighbours => new List<ShipComponent> { up, down, right, left };

        /// <summary>
        /// Returns pair of free neighbour slot index and its own position, for angle calculation
        /// </summary>
        public virtual List<Tuple<Vector2Int, Vector2>> FreeNeghbours
        {
            get
            {
                List<Tuple<Vector2Int, Vector2>> result = new List<Tuple<Vector2Int, Vector2>>();
                if (up == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X, Y + 1), transform.position));
                if (down == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X, Y - 1), transform.position));
                if (right == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X + 1, Y), transform.position));
                if (left == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X - 1, Y), transform.position));

                return result;
            }
        }
        
        public virtual List<Vector2Int> PossibleNeghbours =>
            new List<Vector2Int>
            {
                new Vector2Int(X, Y + 1),
                new Vector2Int(X, Y - 1),
                new Vector2Int(X + 1, Y),
                new Vector2Int(X - 1, Y),
            };

        public int TypeIndex { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public virtual bool CanRotate => false;
        
        public abstract string Name { get; }
        public abstract string Description { get; }
    
        public Ship Ship { get; set; }
        public int Index { get; set; }
        protected bool IsEnabled { get; private set; }
        protected bool IsPlaced { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
    
        public abstract int Cost { get; }
        public abstract int Toughness { get; }
        public abstract int Weight { get; }
        public virtual int MaxFuel => 0;
        public float Fuel { get; set; }
        protected virtual bool NeedFuel => false;
        protected virtual bool CanTransferFuel => false;

        #endregion

        #region Attributes

        private ShipComponent up;
        private ShipComponent down;
        private ShipComponent right;
        private ShipComponent left;
    
        private Queue<int> fuelComponentOpenQueue = new Queue<int>();
        private List<bool> fuelComponentCloseList = new List<bool>();

        #endregion

        #region Data

        [Serializable]
        public class ComponentData
        {
            public int TypeIndex;
            public int X;
            public int Y;
            public float Angle;
        }

        public ComponentData ToData()
        {
            return new ComponentData
            {
                TypeIndex = TypeIndex,
                X = X,
                Y = Y,
                Angle = transform.eulerAngles.z,
            };
        }

        public static ShipComponent FromData(ComponentData data)
        {
            ShipComponent component = ShipBuilder.Instance.InstantiateComponent(data.TypeIndex);
            component.X = data.X;
            component.Y = data.Y;
            component.transform.eulerAngles = new Vector3(0, 0, data.Angle);

            return component;
        }

        #endregion

        #region Public methods

        public virtual void Initialize(int typeIndex)
        {
            TypeIndex = typeIndex;
            Rigidbody = GetComponent<Rigidbody2D>();

            Fuel = MaxFuel;
        }

        public virtual void OnPlaced()
        {
            IsPlaced = true;
        }
    
        public void Disable()
        {
            IsEnabled = false;
            Rigidbody.bodyType = RigidbodyType2D.Static;
        }
    
        public void Enable()
        {
            IsEnabled = true;
            Rigidbody.bodyType = RigidbodyType2D.Dynamic;

            if (NeedFuel)
                ConnectFuelSystem();
        }

        public void Disconnect()
        {
            IsEnabled = false;

            foreach (FixedJoint2D joint in GetComponents<FixedJoint2D>())
            {
                Destroy(joint);
            }
        }

        #endregion

        #region Private methods

        protected virtual void Attach(ShipComponent component)
        {
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = component.Rigidbody;
        }

        private void ConnectFuelSystem()
        {
            for (int i = 0; i < Ship.Components.Count; i++)
            {
                fuelComponentCloseList.Add(false);
            }

            fuelComponentOpenQueue.Enqueue(Index);
            fuelComponentCloseList[Index] = true;
        }

        public void RefreshFuelSystem()
        {
            for (int i = 0; i < Ship.Components.Count; i++)
            {
                fuelComponentCloseList.Add(false);
            }

            fuelComponentOpenQueue.Clear();
            fuelComponentOpenQueue.Enqueue(Index);
            fuelComponentCloseList[Index] = true;
        }

        protected bool DrainFuel(float fuel)
        {
            while (fuelComponentOpenQueue.Any())
            {
                ShipComponent component = Ship.Components[fuelComponentOpenQueue.Peek()];
                if (component.DrainFuelInner(fuel))
                    return true;

                fuelComponentOpenQueue.Dequeue();

                foreach (ShipComponent nextComponent in component.Neighbours
                    .Where(c => c != null && c.CanTransferFuel && !fuelComponentCloseList[c.Index]))
                {
                    fuelComponentOpenQueue.Enqueue(nextComponent.Index);
                    fuelComponentCloseList[nextComponent.Index] = true;
                }
            }

            return false;
        }

        protected virtual bool DrainFuelInner(float fuel)
        {
            if (fuel > Fuel)
            {
                Fuel = 0;
                return false;
            }

            Fuel -= fuel;
        
            return true;
        }

        protected virtual void LeftClicked() { }

        protected virtual void RightClicked()
        {
            if (IsPlaced && !IsEnabled)
            {
                ShipBuilder.Instance.TryRemoveComponent(this);
            }
        }

        #endregion

        #region Event methods

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeftClicked();
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                RightClicked();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<ShipComponent>() != null)
                return;
        
            Ship.RegisterDamage(other.relativeVelocity.magnitude);
        }

        #endregion
    }
}