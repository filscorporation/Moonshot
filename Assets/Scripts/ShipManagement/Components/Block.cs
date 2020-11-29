using System.Linq;

namespace ShipManagement.Components
{
    public class Block : ShipComponent
    {
        public override int Cost => 1;
        public override int Toughness => 1 + Neighbours.Sum(n => n != null && n is Block ? 1 : 0);
    }
}