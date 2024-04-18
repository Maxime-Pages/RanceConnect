using DouglasDwyer.PowerSerializer;
using RanceConnect;
using System.Windows;

namespace Rance_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            initMockupDataDb();
            Main.Content = new Dashboard();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Dashboard();
        }

        private void Stocks_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Stock();
        }

        private void Historique_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Historique();
        }

        private void Alertes_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Alertes();
        }
        public void initMockupDataDb()
        {
            Random random = new Random();
            // Add Categories
            Category meat = Interactions.AddCategory(new Category("Meat", []));
            Category poultry = Interactions.AddCategory(new Category("Poultry", []));
            Category seafood = Interactions.AddCategory(new Category("Seafood", []));
            Category fruits = Interactions.AddCategory(new Category("Fruits", []));
            Category vegetables = Interactions.AddCategory(new Category("Vegetables", []));
            Category dairy = Interactions.AddCategory(new Category("Dairy", []));
            Category bakery = Interactions.AddCategory(new Category("Bakery", []));
            Category beverages = Interactions.AddCategory(new Category("Beverages", []));
            Category snacks = Interactions.AddCategory(new Category("Snacks", []));
            Category personalCare = Interactions.AddCategory(new Category("Personal Care", []));
            
            // Add 50 different products to the Stock list one by one
            
            // Add 50 different products to the Stock list one by one
            
            RanceConnect.Product p1 = CreateProduct("Boeuf Charolais");
            p1.Categories = new RanceConnect.Category[] { meat };
            Interactions.AddProduct(p1);
            
            RanceConnect.Product p2 = CreateProduct("Poulet fermier");
            p2.Categories = new RanceConnect.Category[] { poultry };
            Interactions.AddProduct(p2);
            
            RanceConnect.Product p3 = CreateProduct("Saumon Atlantique");
            p3.Categories = new RanceConnect.Category[] { seafood };
            Interactions.AddProduct(p3);
            
            // ... continue adding the remaining products
            
            RanceConnect.Product p41 = CreateProduct("Raisins secs");
            p41.Categories = new RanceConnect.Category[] { fruits };
            Interactions.AddProduct(p41);
            
            RanceConnect.Product p42 = CreateProduct("Pommes reinette");
            p42.Categories = new RanceConnect.Category[] { fruits };
            Interactions.AddProduct(p42);
            
            // ... continue adding the remaining products
            
            RanceConnect.Product p51 = CreateProduct("Haricots verts");
            p51.Categories = new RanceConnect.Category[] { vegetables };
            Interactions.AddProduct(p51);
            
            RanceConnect.Product p52 = CreateProduct("Poireaux");
            p52.Categories = new RanceConnect.Category[] { vegetables };
            Interactions.AddProduct(p52);
            
            // ... continue adding the remaining products
            
            RanceConnect.Product p61 = CreateProduct("Crème fraîche");
            p61.Categories = new RanceConnect.Category[] { dairy };
            Interactions.AddProduct(p61);
            
            RanceConnect.Product p62 = CreateProduct("Beurre de baratte");
            p62.Categories = new RanceConnect.Category[] { dairy };
            Interactions.AddProduct(p62);
            
            // ... continue adding the remaining products
            
            RanceConnect.Product p71 = CreateProduct("Baguette tradition");
            p71.Categories = new RanceConnect.Category[] { bakery };
            Interactions.AddProduct(p71);
            
            RanceConnect.Product p72 = CreateProduct("Pain de campagne");
            p72.Categories = new RanceConnect.Category[] { bakery };
            Interactions.AddProduct(p72);
            
            // ... continue adding the remaining products
            
            RanceConnect.Product p81 = CreateProduct("Eau plate");
            p81.Categories = new RanceConnect.Category[] { beverages };
            Interactions.AddProduct(p81);
            
            RanceConnect.Product p82 = CreateProduct("Jus d'orange");
            p82.Categories = new RanceConnect.Category[] { beverages };
            Interactions.AddProduct(p82);
            
            // ... continue adding the remaining products
            
            RanceConnect.Product p91 = CreateProduct("Cacahuètes grillées");
            p91.Categories = new RanceConnect.Category[] { snacks };
            Interactions.AddProduct(p91);
            
            RanceConnect.Product p92 = CreateProduct("Chips de pommes de terre");
            p92.Categories = new RanceConnect.Category[] { snacks };
            Interactions.AddProduct(p92);
            
            // ... continue adding the remaining products
            
            RanceConnect.Product p101 = CreateProduct("Shampooing douche");
            p101.Categories = new RanceConnect.Category[] { personalCare };
            Interactions.AddProduct(p101);
            
            RanceConnect.Product p102 = CreateProduct("Dentifrice");
            p102.Categories = new RanceConnect.Category[] { personalCare };
            Interactions.AddProduct(p102);

            // Create a product with random properties
            RanceConnect.Product CreateProduct(string name)
            {
                string ean = GenerateRandomEAN();
                float price = GenerateRandomPrice();
                float salesAmount = 0f; // Set a sample sales amount
                int quantity = 100; // Set a sample quantity
                DateTime expirationDate = DateTime.Now.AddDays(random.Next(30, 365)); // Generate a random expiration date between 30 and 365 days from now
                DateTime dateAdded = DateTime.Now; // Set the current date as the date added
                Category[] categories = new Category[0]; // Set an empty array for categories
                RanceRule[] rules = new RanceRule[0]; // Set an empty array for rules

                return new RanceConnect.Product(name, ean, price, salesAmount, quantity, DateTime.Now, categories, rules);
            }

            // Generate a random EAN for the product
            string GenerateRandomEAN()
            {
                string ean = "";
                for (int i = 0; i < 13; i++)
                {
                    ean += random.Next(10).ToString();
                }
                return ean;
            }

            // Generate a random price for the product
            float GenerateRandomPrice()
            {
                return (float)(random.NextDouble() * (100 - 1) + 1); // Generate a random price between 1 and 100
            }
        }
    }
}