using System;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : ControlledComponent
{
    [SerializeField] protected float force = 10f;
    [SerializeField] protected float consumption = 1f;

    public override bool CanRotate => true;
    
    protected override Vector2 Force => -transform.up * force;
    protected override float Consumption => consumption;
    public override int Toughness => -1;
    public override int MaxFuel => 3;

    public override List<Tuple<Vector2Int, Vector2>> FreeNeghbours => new List<Tuple<Vector2Int, Vector2>>();
}