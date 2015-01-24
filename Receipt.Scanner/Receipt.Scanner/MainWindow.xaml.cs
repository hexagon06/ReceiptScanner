using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Receipt.Scanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FileName { get; set; }
        private string ScanResult { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
            {
                FileName = fileDialog.FileName;
                labelFile.Content = FileName;
                buttonStartScan.IsEnabled = true;
            }
        }

        private void buttonStartScan_Click(object sender, RoutedEventArgs e)
        {
            using (var stream = File.OpenRead(FileName))
            {
                ScanResult = Scanner.Scan(stream);
            }
            fieldOutput.Text = ScanResult;
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
