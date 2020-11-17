using UnityEngine;

public abstract class ControlledComponent : ShipComponent
{
    private Rigidbody2D body;
    
    protected abstract Vector2 Force { get; }
    protected abstract float Consumption { get; }
    protected override bool NeedFuel => true;

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
        if (!DrainFuel(Consumption * Time.deltaTime))
            return;
        
        ApplyForce();
    }

    private void ApplyForce()
    {
        body.AddForce(Force, ForceMode2D.Force);
    }
}