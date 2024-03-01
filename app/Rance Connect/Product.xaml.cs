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
        private List<RanceConnect.Product> products;
        public Product(String ean)
        {
            InitializeComponent();
            EAN.Text = ean;
            products = new List<RanceConnect.Product>();
            //TODO Add Product Connection

            Packs.ItemsSource = products;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
