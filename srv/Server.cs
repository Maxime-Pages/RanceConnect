using RanceConnect;
using Command = RanceConnect.Command;
using System.Net;
using System.Net.Sockets;
using LiteDB;

namespace RanceServer;

class RanceServer
{
    static LiteDatabase db;

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


    //Big ugly switch statement, could do some disgusting pointer arithmetic to make it more less wordy
    public static void Handle(Socket socket)
    {
        byte[] data = Receive(socket);
        int validationtoken = BitConverter.ToInt32(data.Skip(2).Take(4).ToArray());//First 2 are size, next 4 are token
        if (ValidateToken(validationtoken)) //TODO: Implement this
        {
            Command command = (Command)BitConverter.ToChar(data.Skip(6).Take(1).ToArray()); //Next 2 are command
            byte[] body = data.Skip(7).ToArray(); 
            switch (command)
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
                    HandleQueryProvisionsOfProduct(Serializer.Deserialize<string>(body));
                    break;
                case Command.GET_PRODUCT:
                    HandleQueryProduct(Serializer.Deserialize<string>(body));
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
                    HandleAddProduct(Serializer.Deserialize<Product>(body));
                    break;
                case Command.ADD_PROVISIONS:
                    HandleAddProvisions(Serializer.Deserialize<Provision>(body));
                    break;
                case Command.ADD_CATEGORIES:
                    HandleAddCategory(Serializer.Deserialize<Category>(body));
                    break;
                case Command.ADD_RULE:
                    HandleAddRule(Serializer.Deserialize<RanceRule>(body));
                    break;
                case Command.EDIT_PRODUCT:
                    HandleEditProduct(Serializer.Deserialize<Product>(body));
                    break;
                case Command.EDIT_CATEGORY:
                    HandleEditCategory(Serializer.Deserialize<Category>(body));
                    break;
                case Command.EDIT_RULE:
                    HandleEditRule(Serializer.Deserialize<RanceRule>(body));
                    break;
                case Command.EDIT_PROVISIONS:
                    HandleEditProvisions(Serializer.Deserialize<Provision>(body));
                    break;
                case Command.REMOVE_PRODUCT:
                    HandleRemoveProduct(Serializer.Deserialize<Product>(body));
                    break;
                case Command.REMOVE_PROVISIONS:
                    HandleRemoveProvisions(Serializer.Deserialize<Provision>(body));
                    break;
                case Command.REMOVE_CATEGORY:
                    HandleRemoveCategory(Serializer.Deserialize<Category>(body));
                    break;
                case Command.REMOVE_RULE:
                    HandleRemoveRule(Serializer.Deserialize<RanceRule>(body));
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


    //TODO: This is untested
    public static void Send(Socket socket, byte[] data)
    {
        socket.Send(data);
    }


    //TODO: Implement this, probably won't, but it's here for the sake of completion
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

    public static byte[] /*void*/ HandleAddProvisions(Provision provision)
    {
        db.GetCollection<Provision>("provisions").Insert(provision);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleAddCategory(Category category)
    {
        db.GetCollection<Category>("categories").Insert(category);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleAddRule(RanceRule rule)
    {
        db.GetCollection<RanceRule>("rules").Insert(rule.GetHashCode(),rule);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleEditProduct(Product product)
    {
        db.GetCollection<Product>("products").Update(product);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleEditCategory(Category category)
    {
        db.GetCollection<Category>("categories").Update(category);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleEditRule(RanceRule rule)
    {
        db.GetCollection<RanceRule>("rules").Update(rule.GetHashCode(), rule);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleEditProvisions(Provision provision)
    {
        db.GetCollection<Provision>("provisions").Update(provision);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleRemoveProduct(Product product)
    {
        db.GetCollection<Product>("products").Delete(product.EAN);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleRemoveProvisions(Provision provision)
    {
        db.GetCollection<Provision>("provisions").Delete(provision.ID);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleRemoveCategory(Category category)
    {
        db.GetCollection<Category>("categories").Delete(category.Name);
        return new byte[0];
    }

    public static byte[] /*void*/ HandleRemoveRule(RanceRule rule)
    {
        db.GetCollection<RanceRule>("rules").Delete(rule.GetHashCode());
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