namespace RanceConnect;

public class Quota(int max, int min) : RanceRule
{
    public int Minimum;
    public int Maximum;

    public override bool IsValid(object input)
    {
        int stock = (int)input;
        return stock >= Minimum && stock <= Maximum;
    }
}