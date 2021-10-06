using System.Linq;

public class Army
{
    public Unit[] units;
    public int money;

    public int worth => units.Sum(u => u.price);
}
