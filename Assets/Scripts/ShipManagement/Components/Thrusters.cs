using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ShipManagement.Components
{
    public class Thrusters : ForceComponent
    {
        [SerializeField] protected float force = 10f;
        [SerializeField] protected float consumption = 1f;

        public override string Name => nameof(Thrusters);
        public override string Description => $"Gives strong increasing force, consumes {Consumption.ToString("F1", CultureInfo.InvariantCulture)} fuel per second, " +
                                              $"constains {MaxFuel} fuel. Cost {Cost}. Weights {Weight}. Gives {Toughness} toughness";

        public override bool CanRotate => true;
    
        public override int Cost => 6;
        protected override Vector2 Force => -transform.up * force;
        protected override float Consumption => consumption;
        public override int Toughness => -1;
        public override int Weight => 1;
        public override int MaxFuel => 3;

        public override List<Tuple<Vector2Int, Vector2>> FreeNeghbours => new List<Tuple<Vector2Int, Vector2>>();
    }
}