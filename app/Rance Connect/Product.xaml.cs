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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rance_App
{
    /// <summary>
    /// Logique d'interaction pour Stocks.xaml
    /// </summary>
    public partial class Product : Page
    {
        private List<Provision> provisions;
        private RanceConnect.Product product;
        private List<Category> categories;
        private ObservableCollection<mutableTuple> categoriesObserved;

        public Product(string ean)
        {
            InitializeComponent();
            EAN.Text = ean;
            provisions = Interactions.QueryProvisionsOfProduct(ean);
            product = Interactions.QueryProduct(ean);
            categories = Interactions.QueryCategories();
            Name.Text = product.Name;
            Price.Text = product.Price.ToString();
            SalesPrice.Text = product.Salesamount.ToString();
            int quantity = 0;
            foreach (Provision p in provisions)
            {
                quantity += p.Quantity;
            }
            TotalQuantity.Text = quantity.ToString();
            Packs.ItemsSource = provisions;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            if (!Interactions.RemoveProduct(product))
                ns.Content = new Alertes();
            ns.Content = new Stock();
        }

        private void RemoveProvisions_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse((sender as Button).Tag.ToString());
            NavigationService ns = NavigationService.GetNavigationService(this);
            if (!Interactions.RemoveProvision(provisions[id]))
                ns.Content = new Alertes();
            
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }
        private void BackSave_Click(object sender, RoutedEventArgs e)
        {
            Price.Text = Price.Text.Replace(".", ",");
            SalesPrice.Text = SalesPrice.Text.Replace(".", ",");
            if (!float.TryParse(Price.Text, out float price))
                return;
            if (!float.TryParse(SalesPrice.Text, out float salesPrice))
                return;

            product.Price = price;
            product.Salesamount = salesPrice;
            NavigationService ns = NavigationService.GetNavigationService(this);
            if (!Interactions.UpdateProduct(product))
                ns.Content = new Alertes();
            ns.Content = new Stock();
        }

        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            categoriesObserved = new ObservableCollection<mutableTuple>(); // <|°_°|>
            foreach (Category category in categories)
            {
                string cName = category.Name;
                if (product.Categories != null && product.Categories.First(x => x.Name == cName) != null)
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


                if (Interactions.AddCategory(categ) == null)
                {
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns.Content = new Alertes();
                }
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


            if (!Interactions.RemoveCategory(resCategory))
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Content = new Alertes();
            }
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
            Provision provision = Interactions.AddProvisionOfProduct(new Provision("0", product.EAN, qt, qDte, DateTime.Now));
            if (provision == null)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Content = new Alertes();
            }
            List<Provision> provisions = Packs.ItemsSource != null ? Packs.ItemsSource as List<Provision> : new List<Provision>();
            provisions.Add(provision);
            Packs.ItemsSource = provisions;
            TotalQuantity.Text = (int.Parse(TotalQuantity.Text)+1).ToString();
        }
    }

    internal class mutableTuple (bool b, string name)
    {
        public bool TCheck { get; set; } = b;
        public string Name { get; set; } = name;
    }
}
