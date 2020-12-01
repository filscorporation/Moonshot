using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ShipManagement.Components
{
    public class Propeller : ForceComponent
    {
        [SerializeField] protected float force = 2.5f;
        [SerializeField] protected float consumption = 0.2f;
        
        public override string Name => nameof(Propeller);
        public override string Description => $"Gives weak instant force, consumes {Consumption.ToString("F1", CultureInfo.InvariantCulture)} fuel per second, " +
                                              $"constains {MaxFuel} fuel. Cost {Cost}. Weights {Weight}. Gives {Toughness} toughness";
    
        public override bool CanRotate => true;

        public override int Cost => 6;
        protected override Vector2 Force => transform.up * force;
        protected override float Consumption => consumption;
        public override int Toughness => -2;
        public override int Weight => 1;
        public override int MaxFuel => 1;

        public override List<Tuple<Vector2Int, Vector2>> FreeNeghbours => new List<Tuple<Vector2Int, Vector2>>();
        
        public override List<Vector2Int> PossibleNeghbours
        {
            get
            {
                List<Vector2Int> result = new List<Vector2Int>();
                switch (Mathf.RoundToInt(transform.eulerAngles.z / 90))
                {
                    case 0:
                        result.Add(new Vector2Int(X, Y - 1));
                        break;
                    case 1:
                        result.Add(new Vector2Int(X - 1, Y));
                        break;
                    case 2:
                        result.Add(new Vector2Int(X, Y + 1));
                        break;
                    case 3:
                        result.Add(new Vector2Int(X + 1, Y));
                        break;
                }

                return result;
            }
        }
    }
}