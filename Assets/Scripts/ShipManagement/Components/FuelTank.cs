using UnityEngine;

namespace ShipManagement.Components
{
    public class FuelTank : ShipComponent
    {
        private const int FULL_WEIGHT = 3;
        private const int EMPTY_WEIGHT = 1;

        [SerializeField] private SpriteRenderer fuelSprite;
        
        public override string Name => "Fuel tank";
        public override string Description => $"Contains {MaxFuel} fuel, transfers it through neighbour tanks to the engines. " +
                                              $"Cost {Cost}. Weights {FULL_WEIGHT} full and {EMPTY_WEIGHT} empty. Gives {Toughness} toughness";

        public override int Cost => 8;
        public override int Toughness => -1;
        public override int Weight => FULL_WEIGHT;
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