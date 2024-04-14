
namespace RanceConnect;

public class Expiration : RanceRule
{
    TimeSpan deltawarning;
    public override bool IsValid(object input)
    {
        return (DateTime)input - DateTime.Now > deltawarning;
    }


}