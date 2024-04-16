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

    public static string remote = "";
    public static int port  = 11000;
    private static IPEndPoint iPEndPoint;

    public static byte[] Send(byte[] data)
    {
        //Create IPEndpoint if it doesn't exist
        if (iPEndPoint == null){
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            iPEndPoint = new (ipAddress,port);
        }


        using Socket client = new(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(iPEndPoint);
    
        client.Send(data);
        
        byte[] response = null;
        byte[] bytes;
        int totalRec = 0;
        
        do{
            bytes = new byte[2048];
            int bytesRec = client.Receive(bytes);
            response ??= new byte[BitConverter.ToInt16(bytes,0)];
            bytes.CopyTo(response,totalRec);
            totalRec += bytesRec;
        }while(totalRec < data.Length);
        
        client.Shutdown(SocketShutdown.Both);

        return response;
    }

}