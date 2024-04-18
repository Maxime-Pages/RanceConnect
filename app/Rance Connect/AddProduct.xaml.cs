using RanceConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
using System.Xml.Linq;

namespace Rance_App
{
    /// <summary>
    /// Logique d'interaction pour AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        private List<string> selectedCategories = new List<string>();
        private List<Category> categories = new List<Category>();
        public AddProduct()
        {
            InitializeComponent();
            categories = Interactions.QueryCategories();
            UpdateCategories(categories);
        }

        public void FilterUpdate(object sender, RoutedEventArgs e)
        {
            string content = (sender as TextBox).Text;
            List<Category> newcategories = new List<Category>();
            foreach (Category category in categories)
            {
                if (category.Name.ToLower().Contains(content.ToLower())) { newcategories.Add(category);}
            }

            CategoriesPanel.Children.Clear();
            UpdateCategories(newcategories);
        }

        public void UpdateCategories(List<Category> categs)
        {
            foreach (Category category in categs)
            {
                Button button = new Button();
                button.Content = category.Name;
                button.Click += SelectCategory_Click;
                button.Padding = new Thickness(5, 1, 5, 1);
                Style borderStyle = new Style(typeof(Border));
                borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(5)));

                button.Resources.Add(typeof(Border), borderStyle);
                button.Background = Brushes.LightSkyBlue;
                button.Foreground = Brushes.Black;
                button.FontSize = 15;
                if (selectedCategories.Contains(category.Name))
                {
                    CategoriesPanel.Children.Insert(0, button);
                }
                else
                {
                    CategoriesPanel.Children.Add(button);
                }
            }
        }

        public void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            Button b = (sender as Button);
            selectedCategories.Remove(b.Name);
            CategoriesPanel.Children.Remove(b);
            b.Background = Brushes.LightSkyBlue;
            b.Click -= RemoveCategory_Click;
            b.Click += SelectCategory_Click;

            CategoriesPanel.Children.Add(b);
        }

            public void SelectCategory_Click(object sender, RoutedEventArgs e)
        {
            Button b = (sender as Button);
            string content = b.Content.ToString();

            selectedCategories.Add(content);
            Style borderStyle = new Style(typeof(Border));

            Color color = (Color)ColorConverter.ConvertFromString("#FF73FF36"); // Convert hexadecimal color code to Color object
            SolidColorBrush brush = new SolidColorBrush(color);
            b.Background = brush;
            b.Tag = "selected";

            b.Click += RemoveCategory_Click;
            b.Click -= SelectCategory_Click;
            CategoriesPanel.Children.Remove(b);
            CategoriesPanel.Children.Insert(0, b);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.GoBack();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Price.Text = Price.Text.Replace(".", ",");
            SalesPrice.Text = SalesPrice.Text.Replace(".", ",");
            if (!float.TryParse(Price.Text, out float price))
                return;
            if (!float.TryParse(SalesPrice.Text, out float salesPrice))
                return;

            List<RanceRule> rules = new List<RanceRule>();
            rules.Add(new Quota(int.Parse(MinimalStock.Text), int.Parse(StockToReach.Text)));
            rules.Add(new Expiration(TimeSpan.FromDays(double.Parse(ExpirationAlert.Text))));
            List<Category> sCategs = new List<Category>();
            foreach (Category c in categories)
            {
                if (selectedCategories.Contains(c.Name))
                    sCategs.Add(c);
            }
            RanceConnect.Product p = new RanceConnect.Product(Name.Text, EAN.Text, price, salesPrice, 0, DateTime.Now, sCategs.ToArray(), rules.ToArray());
            NavigationService ns = NavigationService.GetNavigationService(this);
            if (Interactions.AddProduct(p) == null)
                ns.Content = new Alertes();

            ns.Content = new Stock();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = (sender as TextBox);
            if (txt.Text.Contains("Insérer le Nom du produit") && txt.Text != "Insérer le Nom du produit")
            {
                txt.Text = txt.Text.Replace("Insérer le Nom du produit", "");
            }
            if (txt.Text.Contains("Insérer l'EAN du produit") && txt.Text != "Insérer l'EAN du produit")
            {
                txt.Text = txt.Text.Replace("Insérer l'EAN du produit", "");
            }
        }
    }
}
