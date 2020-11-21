using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShipManagement.Components
{
    public class Propeller : ForceComponent
    {
        [SerializeField] protected float force = 2.5f;
        [SerializeField] protected float consumption = 0.2f;
    
        public override bool CanRotate => true;

        protected override Vector2 Force => transform.up * force;
        protected override float Consumption => consumption;
        public override int Toughness => -2;
        public override int MaxFuel => 1;

        public override List<Tuple<Vector2Int, Vector2>> FreeNeghbours => new List<Tuple<Vector2Int, Vector2>>();
    }
}