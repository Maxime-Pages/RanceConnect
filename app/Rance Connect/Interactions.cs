using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestObjects;
using RanceConnect;

namespace Rance_App
{
    public class Interactions
    {
        static TestStock Stocks= new TestStock();

        public static List<RanceConnect.Product> QueryStock()
        {
            return Stocks.QueryStock();
        }

        public static int QueryStockCount()
        {
            return Stocks.QueryStockCount();
        }

        public static int QueryAlertsCount()
        {
            return Stocks.QueryAlertsCount();
        }

        public static List<Provision> QueryProvisionsOfProduct(string EAN)
        {
            return Stocks.QueryProvisionsOfProduct(EAN);
        }

        public static RanceConnect.Product QueryProduct(string EAN)
        {
            return Stocks.QueryProduct(EAN);
        }

        public static List<Category> QueryCategories()
        {
            return Stocks.QueryCategories();
        }

        public static List<Alert> QueryAlerts()
        {
            return Stocks.QueryAlerts();
        }

        public static List<Log> QueryLogs()
        {
            return Stocks.QueryLogs();
        }

        public static List<Alert> QueryRecentAlerts()
        {
            return Stocks.QueryPrioritaryANDRecentAlerts();
        }

        public static void AddProduct(string name, string ean, float price, float salesAmount, DateTime dateAdded, int lowStock, int MaxStock)
        {
            //TODO : Add Rules Support
            RanceConnect.Product p = new RanceConnect.Product(name, ean, price, salesAmount, 0, dateAdded, null, null);
        }

        internal static void UpdateProduct(RanceConnect.Product product)
        {
            Stocks.UpdateProduct(product);
        }

        internal static void AddCategory(Category category)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveCategory(Category category)
        {
            throw new NotImplementedException();
        }

        internal static void AddProvisionOfProduct(Provision provision)
        {
            throw new NotImplementedException();
        }
    }
}
