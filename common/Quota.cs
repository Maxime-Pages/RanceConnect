using LiteDB;

namespace RanceConnect;

public class Quota : RanceRule
{
    public int Minimum;
    public int Maximum;

    public Quota()
    {
        Minimum = 0;
        Maximum = 0;
    }

    [BsonCtor]
    public Quota(int max, int min)
    {
        Minimum = min;
        Maximum = max;
    }

    public override bool IsValid(object input)
    {
        int stock = (int)input;
        return stock >= Minimum && stock <= Maximum;
    }
}