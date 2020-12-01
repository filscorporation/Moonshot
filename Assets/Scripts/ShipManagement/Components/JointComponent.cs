using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShipManagement.Components
{
    public class JointComponent : ControlledComponent
    {
        public override string Name => "Joint";
        public override string Description => $"Can detatch on key pressed (for multistage rockets). Cost {Cost}. Weights almost 0. Gives {Toughness} toughness";
        
        public override bool CanRotate => true;
        
        public override int Cost => 2;
        public override int Toughness => -3;
        public override int Weight => 0;
        protected override KeyCode GetDefaultControlKey() => KeyCode.Space;

        public override List<Tuple<Vector2Int, Vector2>> FreeNeghbours
        {
            get
            {
                List<Tuple<Vector2Int, Vector2>> result = new List<Tuple<Vector2Int, Vector2>>();
                bool vertical = Mathf.Abs(transform.eulerAngles.z % 180 - 0) < 0.1f;
                if (vertical  && Up == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X, Y + 1), transform.position));
                if (vertical  && Down == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X, Y - 1), transform.position));
                if (!vertical && Right == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X + 1, Y), transform.position));
                if (!vertical && Left == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X - 1, Y), transform.position));

                return result;
            }
        }
        
        public override List<Vector2Int> PossibleNeghbours
        {
            get
            {
                bool vertical = Mathf.Abs(transform.eulerAngles.z % 180 - 0) < 0.1f;
                List<Vector2Int> result = new List<Vector2Int>();
                
                if (vertical)
                {
                    result.Add(new Vector2Int(X, Y + 1));
                    result.Add(new Vector2Int(X, Y - 1));
                }
                else
                {
                    result.Add(new Vector2Int(X + 1, Y));
                    result.Add(new Vector2Int(X - 1, Y));
                }

                return result;
            }
        }

        private void Update()
        {
            if (!IsEnabled)
                return;
        
            if (Input.GetKeyDown(ControlKey))
            {
                Detach();
            }
        }

        private void Detach()
        {
            Ship.ForceRemoveComponent(this);
        }
    }
}