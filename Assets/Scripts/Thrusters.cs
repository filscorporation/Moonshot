using System.Collections.Generic;
using UnityEngine;

public class Thrusters : ShipComponent
{
    [SerializeField] private float force = 10f;
    [SerializeField] private float consumption = 1f;

    private Rigidbody2D body;
    
    public override int Toughness => -1;
    public override int MaxFuel => 3;

    public override List<Vector2Int> FreeNeghbours => new List<Vector2Int>();

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsEnabled)
            return;
        
        if (Input.GetKey(KeyCode.W))
        {
            TryApplyForce();
        }
    }

    private void TryApplyForce()
    {
        if (!DrainFuel(consumption * Time.deltaTime))
            return;
        
        ApplyForce();
    }

    private void ApplyForce()
    {
        body.AddForce(transform.up * force, ForceMode2D.Force);
    }
}