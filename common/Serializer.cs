using DouglasDwyer.PowerSerializer;
namespace RanceConnect;

public static class Serializer
{
    public static byte[] Serialize<T>(T obj)
    {
        PowerSerializer serializer = new PowerSerializer();
        return serializer.Serialize(obj);
    }


    public static T Deserialize<T>(byte[] mem)
    {
        PowerSerializer serializer = new PowerSerializer();
        return serializer.Deserialize<T>(mem);
    }
}