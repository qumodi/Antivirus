using System.Collections.Generic;
using System.Windows;

namespace Antivirus
{
    public partial class ReportsWindow : Window
    {
        private List<string> reports;

        public ReportsWindow()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            reports = ReportsDatabase.GetReports();
            ReportsListBox.ItemsSource = reports;
        }

        private void ClearReportsButton_Click(object sender, RoutedEventArgs e)
        {
            ReportsDatabase.ClearReports();
            reports.Clear();
            ReportsListBox.ItemsSource = null; // Оновити список
            MessageBox.Show("Усі звіти видалено.", "Очищення", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
