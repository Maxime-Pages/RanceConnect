using LiteDB;
namespace RanceConnect;

public class Category
{
    string name;
    [BsonRef("rules")]
    RanceRule[] rules;

    public string Name { get => name; set => name = value; }
    public RanceRule[] Rules { get => rules; set => rules = value; }

    public Category(string name, RanceRule[] rules)
    {
        this.name = name;
        this.rules = rules;
    }
}