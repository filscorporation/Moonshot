using System.Linq;

namespace ShipManagement.Components
{
    public class Block : ShipComponent
    {
        public override string Name => nameof(Block);
        public override string Description => $"Base component. Gives 1 toughness and 1 more for every neighbour block. Cost {Cost}. Weights {Weight}";
        
        public override int Cost => 1;
        public override int Toughness => 1 + Neighbours.Sum(n => n != null && n is Block ? 1 : 0);
        public override int Weight => 1;
    }
}