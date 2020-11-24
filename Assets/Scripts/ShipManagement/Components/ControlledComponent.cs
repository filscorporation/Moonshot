using System.Collections;
using UnityEngine;

namespace ShipManagement.Components
{
    public abstract class ControlledComponent : ShipComponent
    {
        private bool justPlaced = false;
    
        public KeyCode ControlKey { get; set; }

        protected override void LeftClicked()
        {
            base.LeftClicked();
            
            if (IsPlaced && !justPlaced)
            {
                ControlKeysManager.Instance.SelectComponent(this);
            }
        }

        public override void OnPlaced()
        {
            base.OnPlaced();

            ControlKey = GetDefaultControlKey();

            // Skip selecting when component was placed
            justPlaced = true;
            StartCoroutine(ResetJustPlaced());
        }

        private IEnumerator ResetJustPlaced()
        {
            yield return null;
            justPlaced = false;
        }

        protected abstract KeyCode GetDefaultControlKey();
    }
}