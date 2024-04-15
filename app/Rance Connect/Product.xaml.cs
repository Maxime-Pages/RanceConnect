using Microsoft.VisualBasic;
using RanceConnect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<mutableTuple> categoriesObserved;

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
            categoriesObserved = new ObservableCollection<mutableTuple>(); // <|°_°|>
            foreach (Category category in categories)
            {
                if (product.Categories != null && product.Categories.Contains(category))
                {
                    categoriesObserved.Add(new mutableTuple(true, category.Name));
                } else
                {
                    categoriesObserved.Add(new mutableTuple(false, category.Name));
                }
            }
            categoriesGrid.ItemsSource = categoriesObserved;
            CategoriesPopup.Visibility = Visibility.Visible;
        }

        private void RemoveCategoryWindow_Click(object sender, RoutedEventArgs e)
        {
            CategoriesPopup.Visibility = Visibility.Collapsed;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {

            if (!categories.Exists(x => x.Name == NewCategText.Text))
            {
                Category categ = new Category(NewCategText.Text, []);
                Interactions.AddCategory(categ);
                categories.Add(categ);
                categoriesObserved.Add(new mutableTuple(false, NewCategText.Text)); 
            }
        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Tag.ToString();
            mutableTuple observed = categoriesObserved.First(x => x.Name == name);
            Category resCategory = categories.First(x => x.Name == observed.Name);
            categories.Remove(resCategory);
            Interactions.RemoveCategory(resCategory);
            
            categoriesObserved.Remove(observed);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            List<Category> tmp = product.Categories == null ? new List<Category>() : product.Categories.ToList();
            Category resCategory = categories.First(x => x.Name == chk.Tag.ToString());
            if (chk.IsChecked.Value && tmp.Contains(resCategory))
            {
                return;
            }
            if(chk.IsChecked.Value)
            {
                tmp.Add(resCategory);
            } else
            {
                tmp.Remove(resCategory);
            }
            product.Categories = tmp.ToArray();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Interactions.UpdateProduct(product);
        }

        private void Provision_Click(object sender, RoutedEventArgs e)
        {
            int qt = int.Parse(ProvisionQt.Text);
            DateTime qDte = DateTime.Parse(ProvisionDate.Text);
            Interactions.AddProvisionOfProduct(new Provision("0", product.EAN, qt, qDte, DateTime.Now));
        }
    }

    internal class mutableTuple (bool b, string name)
    {
        public bool TCheck { get; set; } = b;
        public string Name { get; set; } = name;
    }
}
