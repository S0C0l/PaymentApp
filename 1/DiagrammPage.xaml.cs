using _1;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PaymentApp.Pages
{
    public partial class DiagrammPage : Page
    {
        public DiagrammPage()
        {
            InitializeComponent();
            Tag = "Диаграммы";
            LoadData();
        }

        private void LoadData()
        {
            CmbUser.ItemsSource = Entities.GetContext().Users.ToList();

            CmbDiagram.Items.Add("Столбчатая");
            CmbDiagram.Items.Add("Круговая");
            CmbDiagram.Items.Add("Линейная");
        }
        

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Экспорт в Excel пока не реализован. Добавьте ссылку на Microsoft Excel Object Library.");
        }

        private void ExportWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Экспорт в Word пока не реализован. Добавьте ссылку на Microsoft Word Object Library.");
        }
    }
}