using UnityEngine;

public abstract class ForceComponent : ControlledComponent
{
    protected abstract Vector2 Force { get; }
    protected abstract float Consumption { get; }
    protected override bool NeedFuel => true;

    private void Update()
    {
        if (!IsEnabled)
            return;
        
        if (Input.GetKey(ControlKey))
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
        Rigidbody.AddForce(Force, ForceMode2D.Force);
    }
    
    protected override KeyCode GetDefaultControlKey()
    {
        if (Mathf.Abs(Force.x) < Mathf.Abs(Force.y))
        {
            if (Force.y < 0) return KeyCode.S;
            if (Force.y >= 0) return KeyCode.W;
        }
        else
        {
            if (Force.x < 0) return KeyCode.A;
            if (Force.x >= 0) return KeyCode.D;
        }

        return 0;
    }
}