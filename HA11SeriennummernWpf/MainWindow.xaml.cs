using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace HA11SeriennummernWpf
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get filepath of text file and put it into the TextBox.
        /// Activate startButton.
        /// </summary>
        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                filepathText.Text = openFileDialog.FileName;
                startButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Read text file. Filter and sort SN. 
        /// Write new text file. Deactivate startbutton.
        /// </summary>
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> linesInput = new List<string>();
            List<string> linesOutput = new List<string>();

            try
            {
                linesInput = System.IO.File.ReadAllLines(filepathText.Text).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Die Datei konnte nicht geöffnet werden.\n{ex.Message}",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var line in linesInput)
            {
                if (line.Length > 22 && line[22] == 'P')
                {
                    string[] stringArray = line.Split(' ');
                    linesOutput.Add($"{stringArray[2]},{stringArray[1]}");
                }
            }

            linesOutput.Sort();
            linesOutput.Insert(0, "Seriennummer,DateCode");

            try
            {
                System.IO.File.WriteAllLines(filepathText.Text.Split('.')[0] + "_filtered.txt", linesOutput);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Datei\n{ex.Message}",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            startButton.IsEnabled = false;

            MessageBox.Show($"Die Seriennummern wurden unter {filepathText.Text.Split('.')[0] + "_filtered.txt"} gespeichert.",
                "Erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}