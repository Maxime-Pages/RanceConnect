namespace RanceConnect;

public class Alert
{
    private string name;
    private ushort type;
    private string ean;
    DateTime dateAdded;
    public string Name { get => name; set => name = value; }
    public ushort Type { get => type; set => type = value; }
    public string EAN { get => ean; set => ean = value; }
    public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }

    public Alert(string name, ushort type, string ean, DateTime dateAdded)
    {
        Name = name;
        Type = type;
        EAN = ean;
        DateAdded = dateAdded;
    }
}