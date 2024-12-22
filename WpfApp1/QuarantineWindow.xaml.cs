using System;
using System.IO;
using System.Windows;

namespace AntivirusApp
{
    public partial class QuarantineWindow : Window
    {
        private readonly string quarantineFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Quarantine");

        public QuarantineWindow()
        {
            InitializeComponent();
            LoadQuarantineFiles();
        }

        private void LoadQuarantineFiles()
        {
            if (!Directory.Exists(quarantineFolder))
            {
                MessageBox.Show("Папка карантину не знайдена.", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var files = Directory.GetFiles(quarantineFolder);
            listBoxQuarantine.ItemsSource = files;
        }

        private void buttonClearQuarantine_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ви впевнені, що хочете очистити карантин?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                foreach (var file in Directory.GetFiles(quarantineFolder))
                {
                    File.Delete(file);
                }

                LoadQuarantineFiles();
                MessageBox.Show("Карантин очищено.", "Успіх!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
