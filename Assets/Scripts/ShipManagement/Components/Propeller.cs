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
    }
}