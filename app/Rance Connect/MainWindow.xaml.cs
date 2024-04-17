using DouglasDwyer.PowerSerializer;
using System.Windows;

namespace Rance_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new Dashboard();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Dashboard();
        }

        private void Stocks_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Stock();
        }

        private void Historique_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Historique();
        }

        private void Alertes_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Alertes();
        }
    }
}