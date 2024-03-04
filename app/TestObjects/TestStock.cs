using RanceConnect;
using System;

namespace TestObjects
{
    public class TestCategory
    {
        public List<Category> categories = new List<Category>();
        public List<Category> Categories { get => categories; set => categories = value; }

        public TestCategory()
        {
            Categories.Add(new Category("Laitier", null));
            Categories.Add(new Category("Frais", null));
            Categories.Add(new Category("Viande", null));
            Categories.Add(new Category("Legume", null));
            Categories.Add(new Category("Mer", null));
        }

    }

    public class TestStock
    {
        public List<Product> stock = new List<Product>();
        public List<Product> Stock { get => stock; set => stock = value; }

        public List<Product> QueryStock()
        {
            return Stock;
        }
        public int QueryCount()
        {
            return Stock.Count;
        }

        public void AddProduct(string name)
        {
            Stock.Add(CreateProduct(name));
        }

        public TestStock()
        {
            // Add different products to the Stock list one by one
            for (int i = 0; i < 500; i++) 
            {
                Stock.Add(CreateProduct("Product " + i.ToString()));
            }
        }
        // Create a product with random properties
        Product CreateProduct(string name)
        {
            Random random = new Random();
            TestCategory testCategory = new TestCategory();
            string ean = GenerateRandomEAN(random);
            float price = GenerateRandomPrice(random);
            float salesAmount = 0f; // Set a sample sales amount
            int quantity = 100; // Set a sample quantity
            DateTime expirationDate = DateTime.Now.AddDays(random.Next(30, 365)); // Generate a random expiration date between 30 and 365 days from now
            DateTime dateAdded = DateTime.Now; // Set the current date as the date added

            Category category = testCategory.Categories[random.Next(0, 4)];

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
