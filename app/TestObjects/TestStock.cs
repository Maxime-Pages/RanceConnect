using RanceConnect;
using System;

namespace TestObjects
{
    public class TestStock
    {
        public List<Category> categories = new List<Category>();
        public List<Category> Categories { get => categories; set => categories = value; }


        public List<Category> QueryCategories() { return categories; }
        public List<Alert> alerts = new List<Alert>();
        public List<Alert> Alerts { get => alerts; set => alerts = value; }
        public List<Log> logs = new List<Log>();
        public List<Log> Logs { get => logs; set => logs = value; }

        public void AddLog(string username, int type, string secondary, string tertiary)
        {
            switch (type)
            {
                case 0:
                    Logs.Add(new Log(username + " requested " + secondary, DateTime.Now));
                    break;
                case 1:
                    Logs.Add(new Log(username + " requested product page with ean : " + secondary, DateTime.Now)); break;
                case 2:
                    Logs.Add(new Log(username + " modified product with ean : " + secondary, DateTime.Now)); break;
                case 3:
                    Logs.Add(new Log(username + " deleted " + secondary + " with ean : " + tertiary, DateTime.Now)); break;
                case 4:
                    Logs.Add(new Log(username + " added " + secondary + " with ean : " + tertiary, DateTime.Now)); break;
            }
        }

        public List<Alert> QueryAlerts() {
            AddLog("user", 0, "Alerts", "");
            return Alerts; }

        public int QueryAlertsCount()
        {
            AddLog("user", 0, "Alerts Count", "");
            return Alerts.Count;
        }

        public List<Alert> QueryPrioritaryANDRecentAlerts()
        {
            AddLog("user", 0, "Prioritary Recent Alerts", "");
            List<Alert> newAlerts = new List<Alert>();
            int count = 0;
            foreach (Alert alert in Alerts)
            {
                if (count == 10)
                    break;
                if (alert.Type > 1)
                    continue;
                newAlerts.Add(alert); count++;
            }
            return newAlerts; 
        }
        public List<Product> stock = new List<Product>();
        public List<Product> Stock { get => stock; set => stock = value; }

        public List<Product> QueryStock()
        {
            AddLog("user", 0, "Stock", "");
            return Stock;
        }
        public List<Provision> provisions = new List<Provision>();
        public List<Provision> Provisions { get => provisions; set => provisions = value; }

        public List<Provision> QueryProvisionsOfProduct(string ean)
        {
            AddLog("user", 0, "Provisions of Product", "");
            List<Provision> newProvisions = new List<Provision>();
            foreach (Provision product in provisions)
            {
                if (product.EAN == ean)
                    newProvisions.Add(product);
            }
            return newProvisions;
        }

        public int QueryStockCount()
        {
            AddLog("user", 0, "Stock Count", "");
            return Stock.Count;
        }

        public void AddProduct(string name)
        {
            Product p = CreateProduct(name);
            AddLog("user", 4, "Product", p.EAN);
            Stock.Add(p);
        }

        public TestStock()
        {

            Alerts.Add(new Alert("test urgent alert", 0, "EQCEPCAPCcf", DateTime.Now));
            Alerts.Add(new Alert("test important alert", 1, "B49TB84TT38", DateTime.Now));

            Categories.Add(new Category("Laitier", null));
            Categories.Add(new Category("Frais", null));
            Categories.Add(new Category("Viande", null));
            Categories.Add(new Category("Legume", null));
            Categories.Add(new Category("Mer", null));

            Random random = new Random();
            DateTime expirationDate = DateTime.Now.AddDays(random.Next(30, 365)); // Generate a random expiration date between 30 and 365 days from now
            DateTime dateAdded = DateTime.Now; // Set the current date as the date added
            Stock.Add(new Product("tst product 1", "EQCEPCAPCcf", (float)99.99, (float)79.99, 55, expirationDate, dateAdded, null, null));
            Provisions.Add(new Provision("52", "EQCEPCAPCcf", 8, expirationDate, dateAdded));
            Provisions.Add(new Provision("56", "EQCEPCAPCcf", 18, expirationDate, dateAdded));
            Provisions.Add(new Provision("57", "EQCEPCAPCcf", 2, expirationDate, dateAdded));
            Provisions.Add(new Provision("58", "EQCEPCAPCcf", 6, expirationDate, dateAdded));
            Provisions.Add(new Provision("59", "EQCEPCAPCcf", 2, expirationDate, dateAdded));
            Provisions.Add(new Provision("80", "EQCEPCAPCcf", 20, expirationDate, dateAdded));

            // Add different products to the Stock list one by one
            for (int i = 0; i < 500; i++) 
            {
                Stock.Add(CreateProduct("Product " + i.ToString()));
            }
        }

        public Product QueryProduct(string EAN)
        {
            AddLog("user", 1, EAN, "");
            foreach (Product item in Stock)
            {
                if (item.EAN == EAN)
                    return item;
            }
            return null;
        }
        public List<Log> QueryLogs()
        {
            AddLog("user", 0, "Logs", "");
            return Logs;
        }

        // Create a product with random properties
        Product CreateProduct(string name)
        {
            Random random = new Random();
            string ean = GenerateRandomEAN(random);
            float price = GenerateRandomPrice(random);
            float salesAmount = 0f; // Set a sample sales amount
            int quantity = 100; // Set a sample quantity
            DateTime expirationDate = DateTime.Now.AddDays(random.Next(30, 365)); // Generate a random expiration date between 30 and 365 days from now
            DateTime dateAdded = DateTime.Now; // Set the current date as the date added

            Category category = Categories[random.Next(0, 4)];

            Category[] categories = new Category[1]; // Set an empty array for categories
            categories[0] = category;
            RanceRule[] rules = new RanceRule[0]; // Set an empty array for rules

            return new Product(name, ean, price, salesAmount, quantity, expirationDate, dateAdded, categories, rules);
        }

        // Generate a random EAN for the product
        string GenerateRandomEAN(Random random)
        {
            string ean = "";
            for (int i = 0; i < 13; i++)
            {
                ean += random.Next(10).ToString();
            }
            return ean;
        }

        // Generate a random price for the product
        float GenerateRandomPrice(Random random)
        {
            return (float)(random.NextDouble() * (100 - 1) + 1); // Generate a random price between 1 and 100
        }
    }
}
