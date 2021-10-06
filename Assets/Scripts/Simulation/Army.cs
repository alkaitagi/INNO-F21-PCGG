using System.Linq;

public class Army
{
    public Unit[] units;
    public int money;

    public Army(int size, int money = 0)
    {
        units = new Unit[size];
        this.money = money;
    }

    public int worth => money + units.Sum(u => u?.price ?? 0);
}
