namespace RanceConnect;

public class Quota : RanceRule
{
    int minimum;
    int maximum;

    public override bool IsValid(object input)
    {
        int stock = (int)input;
        return stock >= minimum && stock <= maximum;
    }
}