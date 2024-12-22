using Antivirus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace AntivirusApp
{
    public partial class ScanningWindow : Window
    {
        private Dictionary<string, string> signatures = new Dictionary<string, string>();
        private readonly string quarantineFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Quarantine");

        public ScanningWindow()
        {
            InitializeComponent();
            LoadSignatures();

            // Перевіряємо наявність папки карантину
            if (!Directory.Exists(quarantineFolder))
            {
                Directory.CreateDirectory(quarantineFolder);
            }
        }

        private void buttonScan_Click(object sender, RoutedEventArgs e)
        {
            string filePath = textBoxPath.Text;

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Вказаного файлу не існує", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                StartScan(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час сканування: {ex.Message}", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartScan(string file)
        {
            bool isMalwareDetected = false;

            try
            {
                // Читаємо файл як текст
                string fileContent = Encoding.UTF8.GetString(File.ReadAllBytes(file));

                // Перевіряємо кожну сигнатуру
                foreach (var signature in signatures)
                {
                    if (fileContent.Contains(signature.Key))
                    {
                        isMalwareDetected = true;

                        string message =
                            $"Файл \"{file}\" містить загрозу:\n" +
                            $"Сигнатура: {signature.Key}\n" +
                            $"Тип загрози: {signature.Value}";

                        MessageBox.Show(message, "Виявлено загрозу!", MessageBoxButton.OK, MessageBoxImage.Warning);

                        // Переміщуємо файл у карантин
                        MoveToQuarantine(file);

                        // Записуємо результат у базу даних
                        SaveScanResult(file, signature.Value, "Загрозу виявлено");
                        break;
                    }
                }

                // Якщо загроз не виявлено
                if (!isMalwareDetected)
                {
                    MessageBox.Show("Файл не містить загроз.", "Чисто!", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Записуємо результат у базу даних
                    SaveScanResult(file, "Без загроз", "Чисто");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка обробки файлу: {ex.Message}", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);

                // У разі помилки також записуємо її у звіт
                SaveScanResult(file, "Помилка", ex.Message);
            }
        }

        private void buttonChoose_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                textBoxPath.Text = dialog.FileName;
            }
        }

        private void LoadSignatures()
        {
            signatures.Add("CreateRemoteThread", "Trojan");
            signatures.Add("GetAsyncKeyState", "Trojan");
            signatures.Add("GetForegroundWindow", "Keylogger");
            signatures.Add("GetWindowText", "Keylogger");
            signatures.Add("JOIN", "Trojan");
            signatures.Add("MD5CryptoServiceProvider", "Crypter");
            signatures.Add("NtUnmapViewOfSection", "Trojan");
            signatures.Add("PRIVMSG", "Trojan");
            signatures.Add("RijndaelManaged", "Crypter");
            signatures.Add("SetWindowsHookEx", "Keylogger");
            signatures.Add(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*", "Virus");
            signatures.Add(@"NURE is virus", "Virus");
        }

        private void SaveScanResult(string filePath, string result, string status)
        {
            try
            {
                ReportsDatabase.AddReport($"Файл: {filePath}, Результат: {result}, Статус: {status}, Час: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка запису в базу даних: {ex.Message}", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MoveToQuarantine(string filePath)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                string quarantinePath = Path.Combine(quarantineFolder, fileName);

                if (File.Exists(quarantinePath))
                {
                    quarantinePath = Path.Combine(quarantineFolder, $"{Guid.NewGuid()}_{fileName}");
                }

                File.Move(filePath, quarantinePath);

                MessageBox.Show($"Файл переміщено в карантин: {quarantinePath}", "Карантин", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка переміщення в карантин: {ex.Message}", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
