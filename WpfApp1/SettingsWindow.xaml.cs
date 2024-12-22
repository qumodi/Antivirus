using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace Antivirus
{
    public partial class SettingsWindow : Window
    {
        private const string DatabaseFile = "settings.db";
        private const string ConnectionString = "Data Source=" + DatabaseFile;

        public bool SaveReport { get; private set; }
        public bool DeleteFilesImmediately { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadSettings();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(DatabaseFile))
            {
                SQLiteConnection.CreateFile(DatabaseFile);
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand(
                        "CREATE TABLE Settings (SaveReport INTEGER, DeleteFilesImmediately INTEGER)", connection);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand(
                        "INSERT INTO Settings (SaveReport, DeleteFilesImmediately) VALUES (0, 1)", connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void LoadSettings()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT SaveReport, DeleteFilesImmediately FROM Settings", connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SaveReport = reader.GetInt32(0) == 1;
                        DeleteFilesImmediately = reader.GetInt32(1) == 1;
                        checkBoxSaveReport.IsChecked = SaveReport;
                        if (DeleteFilesImmediately)
                        {
                            radioButtonDelete.IsChecked = true;
                        }
                        else
                        {
                            radioButtonQuarantine.IsChecked = true;
                        }
                    }
                }
            }
        }

        private void buttonSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveReport = checkBoxSaveReport.IsChecked == true;
            DeleteFilesImmediately = radioButtonDelete.IsChecked == true;
            SaveSettings();
            this.DialogResult = true;
            this.Close();
        }

        private void SaveSettings()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    "UPDATE Settings SET SaveReport = @SaveReport, DeleteFilesImmediately = @DeleteFilesImmediately", connection);
                command.Parameters.AddWithValue("@SaveReport", SaveReport ? 1 : 0);
                command.Parameters.AddWithValue("@DeleteFilesImmediately", DeleteFilesImmediately ? 1 : 0);
                command.ExecuteNonQuery();
            }
        }
    }
}
