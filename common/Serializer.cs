using System.Runtime.InteropServices;

namespace RanceConnect;

public static class Serializer
{
    public static byte[] Serialize<T>(T obj)
    {
        int size = Marshal.SizeOf(obj);
        //Keep 4 bytes for objectsize
        byte[] mem = new byte[size + 4];

        //Allocate Memory for Shenanigans
        nint ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, mem, 4, size);

        //Free Memory
        Marshal.FreeHGlobal(ptr);

        BitConverter.GetBytes(size).CopyTo(mem, 0);
        return mem;
    }


    public static T Deserialize<T>(byte[] mem)
    {
        //Size is stored at the start of array
        int size = BitConverter.ToInt32(mem);

        byte[] bytes = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(bytes, 0, ptr, size);

        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);

        return obj;
    }
}