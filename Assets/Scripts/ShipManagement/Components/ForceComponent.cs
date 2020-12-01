using UnityEngine;

namespace ShipManagement.Components
{
    public abstract class ForceComponent : ControlledComponent
    {
        protected abstract Vector2 Force { get; }
        protected abstract float Consumption { get; }
        protected override bool NeedFuel => true;

        private bool isApplyingForce = false;
        protected bool IsApplyingForce
        {
            get => isApplyingForce;
            set
            {
                if (isApplyingForce == value)
                    return;

                isApplyingForce = value;
                ApplyingForceChanged();
            }
        }

        protected virtual void ApplyingForceChanged()
        {
            
        }

        private void Update()
        {
            if (!IsEnabled)
            {
                IsApplyingForce = false;
                return;
            }
        
            if (Input.GetKey(ControlKey))
            {
                TryApplyForce();
            }
            else
            {
                IsApplyingForce = false;
            }
        }

        private void TryApplyForce()
        {
            if (!DrainFuel(Consumption * Time.deltaTime))
            {
                IsApplyingForce = false;
                return;
            }
        
            ApplyForce();
            IsApplyingForce = true;
        }

        protected virtual void ApplyForce()
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
}