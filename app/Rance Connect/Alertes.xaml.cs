﻿using RanceConnect;
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
    /// Logique d'interaction pour Alertes.xaml
    /// </summary>
    public partial class Alertes : Page
    {
        public Alertes()
        {
            InitializeComponent();
            List<Alert> alerts = Interactions.QueryAlerts();
            Alerts.ItemsSource = alerts;
        }
        private void GoTo_Click(object sender, RoutedEventArgs e)
        {
            string EAN = ((Button)sender).Tag.ToString();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Content = new Product(ean: EAN);
        }
    }
}
