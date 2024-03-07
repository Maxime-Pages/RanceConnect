using RanceConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Rance_App
{
    /// <summary>
    /// Logique d'interaction pour Stock.xaml
    /// </summary>
    public partial class Stock : Page
    {
        private List<string> selectedCategories = new List<string>();
        private string searchBar = "";
        private float minPrice;
        private float maxPrice;
        private List<RanceConnect.Product> Products = new List<RanceConnect.Product>();

        public Stock()
        {
            InitializeComponent();
            Products = Interactions.QueryStock();
            Stocks.ItemsSource = Products;
            foreach (Category category in Interactions.QueryCategories()) 
            {
                AddFilter(Categories, category.Name);
            }

        }

        /*Update any of the added Filters to change the product list*/
        public void FilterUpdate(object sender, RoutedEventArgs e)
        {
            string content;
            string name;
            try
            {
                content = (sender as Button).Content.ToString();
                name = (((sender as Button).Parent as StackPanel).Parent as StackPanel).Name;
            } catch (Exception)
            {
                content = (sender as TextBox).Text;
                name = (sender as TextBox).Name;
            }
            switch (name)
            {
                case "Categories":
                    selectedCategories.Add(content); break;
                case "SearchBar":
                    searchBar = content; break;
                case "MinPrice":
                    bool isN = float.TryParse(content, out minPrice); if (!isN) MinPrice.Text = ""; break;
                case "MaxPrice":
                    bool isN2 = float.TryParse(content, out maxPrice); if (!isN2) MaxPrice.Text = ""; break;

                // if other selective filter needed, add to corresponding list
                //then create a foreach in Search() following the one on Category's name
            }

            Search();

        }


        /*Add a filter to a select menu with the menu's panel and the new elm's name*/
        public void AddFilter(StackPanel parent, string name)
        {
            StackPanel panel = new StackPanel();
            panel.Height = 25;
            Button btn = new Button();
            btn.Height = 25;
            btn.Content = name;
            btn.Background = null;
            btn.Foreground = new SolidColorBrush(Colors.White);
            btn.Click += FilterUpdate;
            panel.Children.Add(btn);
            parent.Children.Add(panel);
            parent.MaxHeight = parent.Children.Count * 25;
        }

        /*Search in the list of product*/
        public void Search()
        {
            List<RanceConnect.Product> newProductList = new List<RanceConnect.Product>();
            foreach (RanceConnect.Product p in Products)
            {

                int countcategories = p.Categories.Length;
                foreach (Category c in p.Categories)
                {
                    if (selectedCategories.Contains(c.Name))
                    {
                        countcategories--;
                    }
                }
                if (countcategories > 0)
                    continue;

                if (minPrice!=0 && minPrice > p.Price || maxPrice!=0 && p.Price > maxPrice)
                {
                    continue;
                }

                if (searchBar == "" || !p.Name.Contains(searchBar) && !p.EAN.Contains(searchBar))
                    continue;



                newProductList.Add(p);
            }

            Stocks.ItemsSource = newProductList;
        }


        /*Makes The Selected Menu Drop Down*/
        private void DropMenu_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            StackPanel parent = (btn.Parent as StackPanel).Parent as StackPanel;
            string dropped = btn.Tag.ToString();
            if (dropped == "")
            {
                parent.Height = parent.MaxHeight;
                btn.Tag = "dropped";
            } else
            {
                parent.Height = 25;
                btn.Tag = "";
            }
        }

        /*Makes The "Add Product" popup Appear*/
        public void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductFrame.Content = new AddProduct();
        }

        /*Go to Selected Product Page passing EAN identifier*/
        private void GoTo_Click(object sender, RoutedEventArgs e)
        {
            string EAN = (sender as Button).Tag.ToString();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Content = new Product(ean: EAN);
        }
    }
}
