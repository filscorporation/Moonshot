using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShipManagement.Components
{
    public class JointComponent : ControlledComponent
    {
        public override bool CanRotate => true;
        
        public override int Toughness => -3;
        protected override KeyCode GetDefaultControlKey() => KeyCode.Space;

        public override List<Tuple<Vector2Int, Vector2>> FreeNeghbours
        {
            get
            {
                List<Tuple<Vector2Int, Vector2>> result = new List<Tuple<Vector2Int, Vector2>>();
                bool vertical = Mathf.Approximately(transform.eulerAngles.z % 180, 0);
                if (vertical  && Up == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X, Y + 1), transform.position));
                if (vertical  && Down == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X, Y - 1), transform.position));
                if (!vertical && Right == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X + 1, Y), transform.position));
                if (!vertical && Left == null) result.Add(new Tuple<Vector2Int, Vector2>(new Vector2Int(X - 1, Y), transform.position));

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