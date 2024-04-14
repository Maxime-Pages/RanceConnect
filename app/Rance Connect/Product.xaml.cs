using Microsoft.VisualBasic;
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
        private RanceConnect.Product product;
        private List<Category> categories;

        public Product(string ean)
        {
            InitializeComponent();
            EAN.Text = ean;
            products = Interactions.QueryProvisionsOfProduct(ean);
            product = Interactions.QueryProduct(ean);
            categories = Interactions.QueryCategories();
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
        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            List<mutableTuple> g = new List<mutableTuple>(); // <|°_°|>
            int index = 0;
            foreach (Category category in categories)
            {
                if (product.Categories != null && product.Categories.Contains(category))
                {
                    g.Add(new mutableTuple(index, true, category.Name));
                } else
                {
                    g.Add(new mutableTuple(index, false, category.Name));
                }
                index++;
            }
            categoriesGrid.ItemsSource = g;
            CategoriesPopup.Visibility = Visibility.Visible;
        }
        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoriesPopup.Visibility = Visibility.Collapsed;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            List<Category> tmp = product.Categories == null ? new List<Category>() : product.Categories.ToList();
            if(chk.IsChecked.Value && tmp.Contains(categories[(int)chk.Tag]))
            {
                return;
            }
            if(chk.IsChecked.Value)
            {
                tmp.Add(categories[(int)chk.Tag]);
            } else
            {
                tmp.Remove(categories[(int)chk.Tag]);
            }
            product.Categories = tmp.ToArray();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Interactions.UpdateProduct(product);
        }
    }

    internal class mutableTuple (int ct, bool b, string name)
    {
        public int IDCategory { get; set; } = ct;
        public bool TCheck { get; set; } = b;
        public string Name { get; set; } = name;
    }
}
