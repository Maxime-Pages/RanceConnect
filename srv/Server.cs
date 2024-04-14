using RanceConnect;
using System.Net;
using System.Net.Sockets;
using LiteDB;

namespace RanceServer;

public enum Command
{
    GET_STOCK = 0,
    GET_STOCK_COUNT = 1,
    GET_ALERTS_COUNT = 2,
    GET_PROVISIONS = 3,
    GET_PRODUCT = 4,
    GET_CATEGORIES = 5,
    GET_ALERTS = 6,
    GET_LOGS = 7,
    GET_RECENT_ALERTS = 8,
    ADD_PRODUCT = 9,
}

class RanceServer
{
    static LiteDatabase? db;

    public static void Main(string[] args)
    {
        db = new LiteDatabase("./temp.db");
        AppDomain.CurrentDomain.ProcessExit += (s, e) => db?.Dispose();

        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);

        using Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(ipEndPoint);
        listener.Listen(100);

        Console.WriteLine("Ready to accept connections");
        //Listening Loop
        while (true) {

            Socket handler = listener.Accept();
            Handle(handler);
        }

    }

    public static void Handle(Socket socket)
    {
        byte[] data = Receive(socket);
        if (ValidateToken(BitConverter.ToInt32(data, 2)))
        {
            switch ((Command)BitConverter.ToChar(data, 34))
            {
                case Command.GET_STOCK:
                    HandleQueryStock();
                    break;
                case Command.GET_STOCK_COUNT:
                    HandleQueryStockCount();
                    break;
                case Command.GET_ALERTS_COUNT:
                    HandleQueryAlertsCount();
                    break;
                case Command.GET_PROVISIONS:
                    HandleQueryProvisionsOfProduct(Serializer.Deserialize<string>(data.Skip(34).ToArray()));
                    break;
                case Command.GET_PRODUCT:
                    HandleQueryProduct(Serializer.Deserialize<string>(data.Skip(34).ToArray()));
                    break;
                case Command.GET_CATEGORIES:
                    HandleQueryCategories();
                    break;
                case Command.GET_ALERTS:
                    HandleQueryAlerts();
                    break;
                case Command.GET_LOGS:
                    HandleQueryLogs();
                    break;
                case Command.GET_RECENT_ALERTS:
                    HandleQueryRecentAlerts();
                    break;
                case Command.ADD_PRODUCT:
                    HandleAddProduct(Serializer.Deserialize<Product>(data.Skip(34).ToArray()));
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
        do {
            bytes = new byte[2048];
            int bytesRec = socket.Receive(bytes);
            data ??= new byte[BitConverter.ToInt16(bytes, 0)];
            bytes.CopyTo(data, totalRec);
            totalRec += bytesRec;
        } while (totalRec < data.Length);
        return data;
    }

    public static void Send(Socket socket, byte[] data)
    {
        socket.Send(data);
    }


    //TODO: Implement this
    public static bool ValidateToken(int uid)
    {
        return true;
    }

    public static byte[] /*List<Product>*/ HandleQueryStock()
    {
        List<Product> products = db.GetCollection<Product>("products").FindAll().ToList();
        return Serializer.Serialize(products);
    }

    public static byte[] /*int*/ HandleQueryStockCount()
    {
        return Serializer.Serialize(db.GetCollection<Product>("products").Count());
    }

    public static byte[] /*int*/ HandleQueryAlertsCount()
    {
        ProcessAlerts();
        return Serializer.Serialize(db.GetCollection<Alert>("alerts").Count());
    }

    public static byte[] /*List<Provision>*/ HandleQueryProvisionsOfProduct(string EAN)
    {
        return Serializer.Serialize(db.GetCollection<Provision>("provisions").Query().Where(provision => provision.EAN == EAN).ToList());
    }

    public static byte[] /*Product*/ HandleQueryProduct(string EAN)
    {
        return Serializer.Serialize(db.GetCollection<Product>("products").FindOne(product => product.EAN == EAN));
    }

    public static byte[] /*List<Category>*/ HandleQueryCategories()
    {
        return Serializer.Serialize(db.GetCollection<Category>("categories").FindAll().ToList());
    }

    public static byte[] /*List<Alert>*/ HandleQueryAlerts()
    {
        return Serializer.Serialize(db.GetCollection<Alert>("alerts").FindAll().ToList());
    }

    public static byte[] /*List<Log>*/ HandleQueryLogs()
    {
        return Serializer.Serialize(db.GetCollection<Log>("logs").FindAll().ToList());
    }

    public static byte[] /*List<Alert>*/ HandleQueryRecentAlerts()
    {
        return Serializer.Serialize(db.GetCollection<Alert>("alerts").FindAll().OrderByDescending(alert => alert.DateAdded).ToList());
    }

    public static byte[] /*void*/ HandleAddProduct(Product product)
    {
        db.GetCollection<Product>("products").Insert(product);
        return new byte[0];
    }

    public static void ProcessAlerts()
    {
        List<Alert> alerts = db.GetCollection<Alert>("alerts").FindAll().ToList();
        foreach (Product product in db.GetCollection<Product>("products").FindAll())
        {
            foreach (RanceRule rule in product.Rules)
            {
                switch (rule.GetType().Name)
                {
                    case "Expiration":
                        if(alerts.Find(alert => alert.EAN == product.EAN && alert.Type == 0) == null )
                        {
                            List<Provision> provisions = db.GetCollection<Provision>("provisions").Query().Where(provision => provision.EAN == product.EAN).ToList();
                            foreach(Provision provision in provisions)
                            {
                                if(!rule.IsValid(provision.ExpirationDate))
                                {
                                    db.GetCollection<Alert>("alerts").Insert(new Alert(product.Name,0,product.EAN,DateTime.Now));
                                }
                            }
                        }
                        break;
                    case "Provision":
                        if (alerts.Find(alert => alert.EAN == product.EAN && alert.Type == 1) == null && !rule.IsValid(product.Quantity))
                        {
                            db.GetCollection<Alert>("alerts").Insert(new Alert(product.Name,1,product.EAN,DateTime.Now));
                        }
                        break;
                }
            }
        }
    }

}