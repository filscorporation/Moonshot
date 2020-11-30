namespace ShipManagement.Components
{
    public class Baloon : ShipComponent
    {
        public override string Name => nameof(Baloon);
        public override string Description => $"Has negative weight {Weight}. Loses its effectiveness at altitude 50 or higher. Gives {Toughness} toughness. Cost {Cost}";
        
        public override int Cost => 4;
        public override int Toughness => -4;
        public override int Weight => -3;
    }
}