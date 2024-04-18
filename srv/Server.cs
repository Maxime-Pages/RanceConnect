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

        TcpListener listener = new TcpListener(IPAddress.Any, 11000);

        listener.Start();

        Console.WriteLine("Ready to accept connections");
        while (true) {

            TcpClient handler = listener.AcceptTcpClient() ;
            Handle(handler, handler.Client.RemoteEndPoint.ToString());
        }
         
    }


    //Big ugly switch statement, could do some disgusting pointer arithmetic to make it more less wordy
    public static void Handle(TcpClient client,string remoteIP)
    {
        NetworkStream stream = client.GetStream();
        byte[] data = Receive(stream);
        int validationtoken = BitConverter.ToInt32(data.Take(4).ToArray());//First 4 are token
        if (ValidateToken(validationtoken)) //TODO: Implement this
        {
            Command command = (Command)BitConverter.ToChar(data.Skip(4).Take(2).ToArray()); //Next 1 is command
            byte[] body = data.Skip(6).ToArray();
            byte[] response = null;
            Log log = new Log($"User {remoteIP} sent {command}", DateTime.Now);
            db.GetCollection<Log>("logs").Insert(log.GetHashCode(),log);
            Console.WriteLine(log.ToString());
            switch (command)
            {
                case Command.GET_STOCK:
                    response = HandleQueryStock();
                    break;
                case Command.GET_STOCK_COUNT:
                    response = HandleQueryStockCount();
                    break;
                case Command.GET_ALERTS_COUNT:
                    response = HandleQueryAlertsCount();
                    break;
                case Command.GET_PROVISIONS:
                    response = HandleQueryProvisionsOfProduct(Serializer.Deserialize<List<string>>(body));
                    break;
                case Command.GET_PRODUCT:
                    response = HandleQueryProduct(Serializer.Deserialize<List<string>>(body));
                    break;
                case Command.GET_CATEGORIES:
                    response = HandleQueryCategories();
                    break;
                case Command.GET_ALERTS:
                    response = HandleQueryAlerts();
                    break;
                case Command.GET_LOGS:
                    response = HandleQueryLogs();
                    break;
                case Command.GET_RECENT_ALERTS:
                    response = HandleQueryRecentAlerts();
                    break;
                case Command.ADD_PRODUCT:
                    response = HandleAddProduct(Serializer.Deserialize<Product>(body));
                    break;
                case Command.ADD_PROVISIONS:
                    response = HandleAddProvisions(Serializer.Deserialize<Provision>(body));
                    break;
                case Command.ADD_CATEGORIES:
                    response = HandleAddCategory(Serializer.Deserialize<Category>(body));
                    break;
                case Command.ADD_RULE:
                    response = HandleAddRule(Serializer.Deserialize<RanceRule>(body));
                    break;
                case Command.EDIT_PRODUCT:
                    response = HandleEditProduct(Serializer.Deserialize<Product>(body));
                    break;
                case Command.EDIT_CATEGORY:
                    response = HandleEditCategory(Serializer.Deserialize<Category>(body));
                    break;
                case Command.EDIT_RULE:
                    response = HandleEditRule(Serializer.Deserialize<RanceRule>(body));
                    break;
                case Command.EDIT_PROVISIONS:
                    response = HandleEditProvisions(Serializer.Deserialize<Provision>(body));
                    break;
                case Command.REMOVE_PRODUCT:
                    response = HandleRemoveProduct(Serializer.Deserialize<Product>(body));
                    break;
                case Command.REMOVE_PROVISIONS:
                    response = HandleRemoveProvisions(Serializer.Deserialize<Provision>(body));
                    break;
                case Command.REMOVE_CATEGORY:
                    response = HandleRemoveCategory(Serializer.Deserialize<Category>(body));
                    break;
                case Command.REMOVE_RULE:
                    response = HandleRemoveRule(Serializer.Deserialize<RanceRule>(body));
                    break;
            }
            Send(stream, response);
        }
    }


    //TODO: Will crash if less than 2 bytes were recieved during first recieve call but I don't want to bother fixing it
    public static byte[] Receive(NetworkStream stream)
    {
        byte[] buffer = new byte[2];
        stream.Read(buffer, 0, 2);
        int len = BitConverter.ToInt16(buffer);
        byte[] data = new byte[len - 2];

        for (int i = 2; i < len; i += stream.Read(data, 0, len - i)) ;
        
        return data;
    }

    public static void Send(NetworkStream stream, byte[] data)
    {
        data = [.. BitConverter.GetBytes((short)(data.Length + 2)), .. data];
        stream.Write(data, 0, data.Length);
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

    public static byte[] /*List<Provision>*/ HandleQueryProvisionsOfProduct(List<string> EAN)
    {
        string ean = EAN[0];
        return Serializer.Serialize(db.GetCollection<Provision>("provisions").Query().Where(provision => provision.EAN == ean).ToList());
    }

    public static byte[] /*Product*/ HandleQueryProduct(List<string> EAN)
    {
        string ean = EAN[0];
        Product product = db.GetCollection<Product>("products").FindOne(product => product.EAN == ean);
        //Console.WriteLine(product.Name);
        return Serializer.Serialize(product);
    }

    public static byte[] /*List<Category>*/ HandleQueryCategories()
    {
        return Serializer.Serialize(db.GetCollection<Category>("categories").FindAll().ToList());
    }

    public static byte[] /*List<Alert>*/ HandleQueryAlerts()
    {
        ProcessAlerts();
        return Serializer.Serialize(db.GetCollection<Alert>("alerts").FindAll().ToList());
    }

    public static byte[] /*List<Log>*/ HandleQueryLogs()
    {
        return Serializer.Serialize(db.GetCollection<Log>("logs").FindAll().ToList());
    }

    public static byte[] /*List<Alert>*/ HandleQueryRecentAlerts()
    {
        ProcessAlerts();
        return Serializer.Serialize(db.GetCollection<Alert>("alerts").FindAll().OrderByDescending(alert => alert.DateAdded).ToList());
    }

    public static byte[] /*Product*/ HandleAddProduct(Product product)
    {
        Console.WriteLine(product.Name);
        db.GetCollection<Product>("products").Insert(product.EAN, product);
        return Serializer.Serialize(product);
    }

    public static byte[] /*Provision*/ HandleAddProvisions(Provision provision)
    {
        provision.ID = Guid.NewGuid().ToString();
        db.GetCollection<Provision>("provisions").Insert(provision.ID,provision);
        return Serializer.Serialize(provision);
    }

    public static byte[] /*Category*/ HandleAddCategory(Category category)
    {
        db.GetCollection<Category>("categories").Insert(category.GetHashCode(), category);
        return Serializer.Serialize(category);
    }

    public static byte[] /*Rule*/ HandleAddRule(RanceRule rule)
    {
        db.GetCollection<RanceRule>("rules").Insert(rule.GetHashCode(),rule);
        return Serializer.Serialize(rule);
    }

    public static byte[] /*bool*/ HandleEditProduct(Product product)
    {
        return Serializer.Serialize(db.GetCollection<Product>("products").Update(product.EAN, product));
    }

    public static byte[] /*bool*/ HandleEditCategory(Category category)
    {
        return Serializer.Serialize(db.GetCollection<Category>("categories").Update(category.GetHashCode(),category));
    }

    public static byte[] /*bool*/ HandleEditRule(RanceRule rule)
    {
        return Serializer.Serialize(db.GetCollection<RanceRule>("rules").Update(rule.GetHashCode(), rule));
    }

    public static byte[] /*bool*/ HandleEditProvisions(Provision provision)
    {
        return Serializer.Serialize(db.GetCollection<Provision>("provisions").Update(provision.ID,provision));
    }

    public static byte[] /*bool*/ HandleRemoveProduct(Product product)
    {
        return Serializer.Serialize(db.GetCollection<Product>("products").Delete(product.EAN));
    }

    public static byte[] /*bool*/ HandleRemoveProvisions(Provision provision)
    {
        return Serializer.Serialize(db.GetCollection<Provision>("provisions").Delete(provision.ID));
    }

    public static byte[] /*bool*/ HandleRemoveCategory(Category category)
    {
        return Serializer.Serialize(db.GetCollection<Category>("categories").Delete(category.GetHashCode()));
        
    }

    public static byte[] /*bool*/ HandleRemoveRule(RanceRule rule)
    {
        return Serializer.Serialize(db.GetCollection<RanceRule>("rules").Delete(rule.GetHashCode()));
        
    }






    public static void ProcessAlerts()
    {
        List<Alert> alerts = db.GetCollection<Alert>("alerts").FindAll().ToList();
        foreach (Product product in db.GetCollection<Product>("products").FindAll())
        {
            if (product.Rules == null) continue;
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