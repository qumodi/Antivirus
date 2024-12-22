using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Antivirus
{
    public static class ReportsDatabase
    {
        private static string ConnectionString = "Data Source=AntivirusReports.db;Version=3;";

        static ReportsDatabase()
        {
            // Створення таблиці, якщо вона ще не існує
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Reports (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ReportText TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Додати новий звіт
        public static void AddReport(string report)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Reports (ReportText) VALUES (@ReportText)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ReportText", report);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Отримати всі звіти
        public static List<string> GetReports()
        {
            var reports = new List<string>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT ReportText FROM Reports ORDER BY CreatedAt DESC";
                using (var command = new SQLiteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reports.Add(reader.GetString(0));
                    }
                }
            }
            return reports;
        }

        // Очистити всі звіти
        public static void ClearReports()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Reports";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
