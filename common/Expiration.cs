
namespace RanceConnect;

public class Expiration: RanceRule
{
    public TimeSpan Deltawarning;

    public Expiration()
    {
        Deltawarning = new TimeSpan(0, 0, 0);
    }

    public Expiration(TimeSpan dW)
    {
        Deltawarning = dW;
    }
    public override bool IsValid(object input)
    {
        return (DateTime)input - DateTime.Now > Deltawarning;
    }


}