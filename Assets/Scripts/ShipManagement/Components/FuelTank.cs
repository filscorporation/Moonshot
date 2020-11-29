using UnityEngine;

namespace ShipManagement.Components
{
    public class FuelTank : ShipComponent
    {
        private const float FULL_WEIGHT = 3f;
        private const float EMPTY_WEIGHT = 1f;

        [SerializeField] private SpriteRenderer fuelSprite;

        public override int Cost => 8;
        public override int Toughness => -1;
        public override int MaxFuel => 10;
        protected override bool CanTransferFuel => true;

        protected override bool DrainFuelInner(float fuel)
        {
            bool result = base.DrainFuelInner(fuel);
            Rigidbody.mass = EMPTY_WEIGHT + (FULL_WEIGHT - EMPTY_WEIGHT) * (Fuel / MaxFuel);
            fuelSprite.transform.localScale = new Vector3(1, Fuel / MaxFuel, 1);
        
            return result;
        }
    }
}