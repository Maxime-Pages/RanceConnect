using Chroma;

namespace RanceConnect;

[Chroma.Name("Products")]
public class Product
{

    string name;

    string ean;
    
    float price;
    
    float salesamount;
    
    int quantity;

    Category[] categories;
    
    RanceRule[] rules;

    public string Name { get => name; set => name = value; }
    [Chroma.Primary]
    public string EAN { get => ean; set => ean = value; }
    public float Price { get => price; set => price = value; }
    public float Salesamount { get => salesamount; set => salesamount = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public Category[] Categories { get => categories; set => categories = value; }
    public RanceRule[] Rules { get => rules; set => rules = value; }


    
}