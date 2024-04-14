using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RanceConnect;

public static class DataSender
{
    public static string remote = "";
    public static int port  = 11000;
    private static IPEndPoint? iPEndPoint;

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