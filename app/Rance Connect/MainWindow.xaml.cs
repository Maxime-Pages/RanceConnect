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

            // Add 20 different products to the Stock list one by one
            //Interactions.AddProduct(CreateProduct("Product 1"));
            //Interactions.AddProduct(CreateProduct("Product 2"));
            //Interactions.AddProduct(CreateProduct("Product 3"));
            //Interactions.AddProduct(CreateProduct("Product 4"));
            //Interactions.AddProduct(CreateProduct("Product 5"));
            //Interactions.AddProduct(CreateProduct("Product 6"));
            //Interactions.AddProduct(CreateProduct("Product 7"));
            //Interactions.AddProduct(CreateProduct("Product 8"));
            //Interactions.AddProduct(CreateProduct("Product 9"));
            //Interactions.AddProduct(CreateProduct("Product 10"));
            //Interactions.AddProduct(CreateProduct("Product 11"));
            //Interactions.AddProduct(CreateProduct("Product 12"));
            //Interactions.AddProduct(CreateProduct("Product 13"));
            //Interactions.AddProduct(CreateProduct("Product 14"));
            //Interactions.AddProduct(CreateProduct("Product 15"));
            //Interactions.AddProduct(CreateProduct("Product 16"));
            //Interactions.AddProduct(CreateProduct("Product 17"));
            //Interactions.AddProduct(CreateProduct("Product 18"));
            //Interactions.AddProduct(CreateProduct("Product 19"));
            //Interactions.AddProduct(CreateProduct("Product 20"));

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