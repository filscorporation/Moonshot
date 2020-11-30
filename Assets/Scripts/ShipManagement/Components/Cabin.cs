namespace ShipManagement.Components
{
    public class Cabin : ShipComponent
    {
        public override string Name => nameof(Cabin);
        public override string Description => $"Central component";
        
        public override int Cost => 0;
        public override int Toughness => 5;
        public override int Weight => 1;
    }
}