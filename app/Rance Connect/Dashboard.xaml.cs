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
    /// Logique d'interaction pour Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            ObservableCollection<Alert> alerts = new ObservableCollection<Alert>();
            //TODO Get Alerts
            Alerts.ItemsSource = alerts;
        }

        private void Alerts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void GoTo_Click(object sender, RoutedEventArgs e)
        {
            string EAN = ((Button)sender).Tag.ToString();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Content = new Product(ean: EAN);
        }
    }
}
