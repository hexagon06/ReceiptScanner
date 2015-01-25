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
        private IDictionary<string, decimal> ConvertedResult { get; set; }

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
            buttonConvert.IsEnabled = ScanResult.Length > 0;
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonConvert_Click(object sender, RoutedEventArgs e)
        {
            ConvertedResult = Scanner.Convert(ScanResult);

            SetData(ConvertedResult);

            fieldOutput.Visibility = System.Windows.Visibility.Collapsed;
            datagridResults.Visibility = System.Windows.Visibility.Visible;
        }

        private void SetData(IDictionary<string, decimal> convertedResult)
        {
            List<DataObject> list = new List<DataObject>();
            foreach (var result in convertedResult)
	{
		 var data = new DataObject(){
              Name = result.Key,
              Value = result.Value
         };
                list.Add(data);
            }

            datagridResults.ItemsSource = list;
        }

        public class DataObject
        {
            public string Name { get; set; }
            public decimal Value { get; set; }
        }
    }
}
