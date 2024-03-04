using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RanceConnect;

namespace Rance_App_TestObjects
{
    public class TestStock
    {
        public List<Product> stock = new List<Product>;
        public List<Product> Stock { get => stock; set => stock = value; }

        public List<Product> QueryStock()
        {
            return Stock;
        }

        public TestStock() {
            List<Product> Stock = new List<Product>();
            Random random = new Random();

            // Add 20 different products to the Stock list one by one
            Stock.Add(CreateProduct("Product 1"));
            Stock.Add(CreateProduct("Product 2"));
            Stock.Add(CreateProduct("Product 3"));
            Stock.Add(CreateProduct("Product 4"));
            Stock.Add(CreateProduct("Product 5"));
            Stock.Add(CreateProduct("Product 6"));
            Stock.Add(CreateProduct("Product 7"));
            Stock.Add(CreateProduct("Product 8"));
            Stock.Add(CreateProduct("Product 9"));
            Stock.Add(CreateProduct("Product 10"));
            Stock.Add(CreateProduct("Product 11"));
            Stock.Add(CreateProduct("Product 12"));
            Stock.Add(CreateProduct("Product 13"));
            Stock.Add(CreateProduct("Product 14"));
            Stock.Add(CreateProduct("Product 15"));
            Stock.Add(CreateProduct("Product 16"));
            Stock.Add(CreateProduct("Product 17"));
            Stock.Add(CreateProduct("Product 18"));
            Stock.Add(CreateProduct("Product 19"));
            Stock.Add(CreateProduct("Product 20"));

            // Create a product with random properties
            Product CreateProduct(string name)
            {
                string ean = GenerateRandomEAN();
                float price = GenerateRandomPrice();
                float salesAmount = 0f; // Set a sample sales amount
                int quantity = 100; // Set a sample quantity
                DateTime expirationDate = DateTime.Now.AddDays(random.Next(30, 365)); // Generate a random expiration date between 30 and 365 days from now
                DateTime dateAdded = DateTime.Now; // Set the current date as the date added
                Category[] categories = new Category[0]; // Set an empty array for categories
                RanceRule[] rules = new RanceRule[0]; // Set an empty array for rules

                return new Product(name, ean, price, salesAmount, quantity, expirationDate, dateAdded, categories, rules);
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
