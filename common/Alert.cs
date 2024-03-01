namespace RanceConnect;

[Chroma.Name("Alerts")]
public class Alert
{
    private string name;
    private ushort type;
    private string ean;
    public string Name { get => name; set => name = value; }
    public ushort Type { get => type; set => type = value; }
    public string EAN { get => ean; set => ean = value; }

    public Alert(string name, ushort type, string eAN)
    {
        Name = name;
        Type = type;
        EAN = eAN;
    }
}