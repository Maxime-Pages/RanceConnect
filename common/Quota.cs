namespace RanceConnect;

public class Quota : RanceRule
{
    public int Minimum;
    public int Maximum;

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