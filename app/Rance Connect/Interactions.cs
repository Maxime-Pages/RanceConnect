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
    }
}
