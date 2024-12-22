using System.Collections.Generic;
using System.IO;

namespace Antivirus
{
    public static class ReportsStorage
    {
        private static string ReportsFilePath = "reports.txt";

        // Отримати список звітів
        public static List<string> GetReports()
        {
            if (File.Exists(ReportsFilePath))
            {
                return new List<string>(File.ReadAllLines(ReportsFilePath));
            }
            return new List<string>();
        }

        // Додати новий звіт
        public static void AddReport(string report)
        {
            File.AppendAllText(ReportsFilePath, report + "\n");
        }

        // Очистити усі звіти
        public static void ClearReports()
        {
            if (File.Exists(ReportsFilePath))
            {
                File.Delete(ReportsFilePath);
            }
        }
    }
}
