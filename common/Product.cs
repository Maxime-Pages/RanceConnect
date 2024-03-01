using Chroma;

namespace RanceConnect;

[Chroma.Name("Products")]
public class Product
{

    string name;

    string ean;
    
    float price;
    
    float salesAmount;
    
    int quantity;

    DateTime expirationDate;
    DateTime dateAdded;

    Category[] categories;
    
    RanceRule[] rules;

    public string Name { get => name; set => name = value; }
    [Chroma.Primary]
    public string EAN { get => ean; set => ean = value; }
    public float Price { get => price; set => price = value; }
    public float Salesamount { get => salesAmount; set => salesAmount = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public DateTime ExpirationDate { get => expirationDate; set => expirationDate = value; }
    public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
    public Category[] Categories { get => categories; set => categories = value; }
    public RanceRule[] Rules { get => rules; set => rules = value; }

    public Product(string name, string ean, float price, float salesAmount, int quantity, DateTime expirationDate, DateTime dateAdded, Category[] categories, RanceRule[] rules)
    {
        Name = name;
        EAN = ean;
        Price = price;
        Salesamount = salesAmount;
        Quantity = quantity;
        ExpirationDate = expirationDate;
        DateAdded = dateAdded;
        Categories = categories;
        Rules = rules;
    }

}