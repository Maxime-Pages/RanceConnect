using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RanceConnect;

namespace Rance_App
{
    public class Interactions
    {
        public static List<RanceConnect.Product> QueryStock()
        {
            return Serializer.Deserialize<List<RanceConnect.Product>>(DataSender.Send([], RanceConnect.Command.GET_STOCK));
//            Serializer.Deserialize<List<RanceConnect.Product>>(DataSender.Send(Serializer.Serialize()))
        }

        public static int QueryStockCount()
        {
            return Serializer.Deserialize<int>(DataSender.Send([], Command.GET_STOCK_COUNT));
        }

        public static int QueryAlertsCount()
        {
            return Serializer.Deserialize<int>(DataSender.Send([], Command.GET_ALERTS_COUNT));
        }

        public static List<Provision> QueryProvisionsOfProduct(string EAN)
        {
            return Serializer.Deserialize<List<Provision>>(DataSender.Send(Serializer.Serialize(new List<string> { EAN }), Command.GET_PROVISIONS));
        }

        public static RanceConnect.Product QueryProduct(string EAN)
        {
            return Serializer.Deserialize<RanceConnect.Product>(DataSender.Send(Serializer.Serialize(new List<string> { EAN }), Command.GET_PRODUCT));
        }

        public static bool RemoveProduct(RanceConnect.Product p)
        {
            return Serializer.Deserialize<bool>(DataSender.Send(Serializer.Serialize(p), Command.REMOVE_PRODUCT));
        }

        public static List<Category> QueryCategories()
        {
            return Serializer.Deserialize<List<Category>>(DataSender.Send([], Command.GET_CATEGORIES));
        }

        public static List<Alert> QueryAlerts()
        {
            return Serializer.Deserialize<List<Alert>>(DataSender.Send([], Command.GET_ALERTS));
        }

        public static List<Log> QueryLogs()
        {
            return Serializer.Deserialize<List<Log>>(DataSender.Send([], Command.GET_LOGS));
        }

        public static List<Alert> QueryRecentAlerts()
        {
            return Serializer.Deserialize<List<Alert>>(DataSender.Send([], Command.GET_RECENT_ALERTS));
        }

        public static void AddProduct(string name, string ean, float price, float salesAmount, DateTime dateAdded, int lowStock, int MaxStock)
        {
           /* //TODO : Add Rules Support
            List<RanceRule> rules = new List<RanceRule>();
            rules.Add(new Expiration())*/
            RanceConnect.Product p = new RanceConnect.Product(name, ean, price, salesAmount, 0, dateAdded, null, null);
            DataSender.Send(Serializer.Serialize(p), Command.ADD_PRODUCT);
        }

        internal static void UpdateProduct(RanceConnect.Product product)
        {
            DataSender.Send(Serializer.Serialize(product), Command.EDIT_PRODUCT);
        }

        internal static void AddCategory(Category category)
        {
            DataSender.Send(Serializer.Serialize(category), Command.EDIT_CATEGORY);
        }

        internal static bool RemoveCategory(Category category)
        {
            return Serializer.Deserialize<bool>(DataSender.Send(Serializer.Serialize(category), Command.REMOVE_CATEGORY));
        }

        internal static bool RemoveProvision(Provision p)
        {
            return Serializer.Deserialize<bool>(DataSender.Send(Serializer.Serialize(p), Command.REMOVE_PROVISIONS));
        }

        internal static Provision AddProvisionOfProduct(Provision provision)
        {
            return Serializer.Deserialize<Provision>(DataSender.Send(Serializer.Serialize(provision), Command.ADD_PROVISIONS));
        }
    }
}
