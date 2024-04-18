
namespace RanceConnect;

public class Expiration(TimeSpan dW) : RanceRule
{
    public TimeSpan Deltawarning = dW;
    public override bool IsValid(object input)
    {
        return (DateTime)input - DateTime.Now > Deltawarning;
    }


}