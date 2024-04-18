
using LiteDB;

namespace RanceConnect;

public class Product
{
    string name;

    string ean;
    
    float price;
    
    float salesAmount;
    
    int quantity;

    DateTime dateAdded;
    [BsonRef("categories")]
    Category[] categories;
    [BsonRef("rules")]

    RanceRule[] rules;

    public string Name { get => name; set => name = value; }
    public string EAN { get => ean; set => ean = value; }
    public float Price { get => price; set => price = value; }
    public float Salesamount { get => salesAmount; set => salesAmount = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
    public Category[] Categories { get => categories; set => categories = value; }
    public RanceRule[] Rules { get => rules; set => rules = value; }

    public Product(string name, string ean, float price, float salesAmount, int quantity, DateTime dateAdded, Category[] categories, RanceRule[] rules)
    {
        Name = name;
        EAN = ean;
        Price = price;
        Salesamount = salesAmount;
        Quantity = quantity;
        DateAdded = dateAdded;
        Categories = categories;
        Rules = rules;
    }
}