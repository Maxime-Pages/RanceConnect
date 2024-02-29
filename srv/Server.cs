using System.Net;
using System.Net.Sockets;
using Chroma;

namespace RanceServer
{
    public enum Command
    {
        JOE=0,
        YO=10,
        BYE=100,
    }

    class RanceServer
    {
        public static Chroma.Chroma DB;

        public static void Main(string[] args)
        {
            Chroma.Chroma DB = new Chroma.Chroma("mysql", arg);
            Chroma.Chroma.ShowQueries = true; //Enables Debugging

            
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress,11000);
            
            using Socket listener = new Socket(ipAddress.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            listener.Bind(ipEndPoint);
            listener.Listen();

            //Listening Loop
            while (true){

                Socket handler = listener.Accept();
                Handle(handler);
            }

        }
        
        public static void Handle(Socket socket)
        {
            byte[] data = Receive(socket);
            if (ValidateToken(BitConverter.ToInt32(data,2)))
            {
                switch((Command)BitConverter.ToChar(data,34))
                {
                    case Command.JOE:
                        break;
                    case Command.YO:
                        break;
                    case Command.BYE:
                        break;
                }
            }
        }


        //TODO: Will crash if less than 2 bytes were recieved during first recieve call but I don't want to bother fixing it
        public static byte[] Receive(Socket socket)
        {
            byte[] data = null;
            byte[] bytes;
            int totalRec = 0;
            do{
                bytes = new byte[2048];
                int bytesRec = socket.Receive(bytes);
                data ??= new byte[BitConverter.ToInt16(bytes,0)];
                bytes.CopyTo(data,totalRec);
                totalRec += bytesRec;
            }while(totalRec < data.Length);
            return data;
        }


        //TODO: Implement this
        public static bool ValidateToken(int uid)
        {
            return true;
        }
    }
}