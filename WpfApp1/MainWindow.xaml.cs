using Antivirus;
using System.Windows;
using System.Windows.Controls;

namespace AntivirusApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TextBlock { Text = "Сканування", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center };
        }
        private void OpenScanningWindow_Click(object sender, RoutedEventArgs e)
        {
            ScanningWindow scanningWindow = new ScanningWindow();
            scanningWindow.Show();                                
            //this.Close();                                        
        }

        private void Quarantine_Click(object sender, RoutedEventArgs e)
        {
            QuarantineWindow quarantineWindow = new QuarantineWindow();
            quarantineWindow.Show();
            MainContent.Content = new TextBlock { Text = "Керування карантином", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center };
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow();
            reportsWindow.Show();
            MainContent.Content = new TextBlock { Text = "Звіт сканування", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center };
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
            MainContent.Content = new TextBlock { Text = "Налаштування", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center };
        }
    }
}
