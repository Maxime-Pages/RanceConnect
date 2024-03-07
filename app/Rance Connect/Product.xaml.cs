using RanceConnect;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour Stocks.xaml
    /// </summary>
    public partial class Product : Page
    {
        private List<Provision> products;
        public Product(string ean)
        {
            InitializeComponent();
            EAN.Text = ean;
            products = Interactions.QueryProvisionsOfProduct(ean);
            RanceConnect.Product product = Interactions.QueryProduct(ean);
            Name.Text = product.Name;
            Price.Text = product.Price.ToString();
            SalesPrice.Text = product.Salesamount.ToString();
            int quantity = 0;
            foreach (Provision p in products)
            {
                quantity += p.Quantity;
            }
            TotalQuantity.Text = quantity.ToString();
            Packs.ItemsSource = products;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }
    }
}
