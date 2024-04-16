using System.Net;
using System.Net.Sockets;

namespace RanceConnect;



public enum Command
{
    GET_STOCK = 0,
    GET_STOCK_COUNT,
    GET_ALERTS_COUNT,
    GET_PROVISIONS,
    GET_PRODUCT,
    GET_CATEGORIES,
    GET_ALERTS,
    GET_LOGS,
    GET_RECENT_ALERTS,
    ADD_PRODUCT,
    ADD_PROVISIONS,
    ADD_CATEGORIES,
    ADD_RULE,
    EDIT_PRODUCT,
    EDIT_CATEGORY,
    EDIT_RULE,
    EDIT_PROVISIONS,
    REMOVE_PRODUCT,
    REMOVE_PROVISIONS,
    REMOVE_CATEGORY,
    REMOVE_RULE,
}
public static class DataSender
{


    public static byte[] Send(byte[] data, Command command)
    {
        //                  SIZE                                TOKEN                       COMMAND                 DATA
        data = [.. BitConverter.GetBytes(data.Length + 7), .. new byte[4], .. BitConverter.GetBytes((int)command), .. data];

        using TcpClient client = new TcpClient("localhost", 11000);
        NetworkStream stream = client.GetStream();
        
        stream.Write(data, 0, data.Length);

        byte[] buffer = new byte[2];
        stream.Read(buffer, 0, 2);
        int len = BitConverter.ToInt16(buffer);
        byte[] response = new byte[len-2];
        
        for(int i = 2; i < len; i += stream.Read(response, 0, len - i));

        client.Close();

        return response;
    }

}