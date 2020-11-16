using System.Linq;

public class Block : ShipComponent
{
    public override int Toughness => 1 + Neighbours.Sum(n => n != null && n is Block ? 1 : 0);
}