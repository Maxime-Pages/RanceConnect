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
using RanceConnect;


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
            List<Alert> alerts = Interactions.QueryRecentAlerts();
            Alerts.ItemsSource = alerts;
            //provi 1
            ThresholdReached.Text = alerts.Where(x => x.Type == 1).Count().ToString();
            //expi 0
            OutdatedSoon.Text = alerts.Where(x => x.Type == 0).Count().ToString();
            StockCount.Text = Interactions.QueryStockCount().ToString();
            AlertsCount.Text = Interactions.QueryAlertsCount().ToString();
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
