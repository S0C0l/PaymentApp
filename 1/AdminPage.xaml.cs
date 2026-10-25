using System.Windows;
using System.Windows.Controls;
using PaymentApp.Pages;

namespace PaymentApp.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            Tag = "Администратор";
        }

        private void BtnTab1_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new UsersTabPage());
        private void BtnTab2_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new CategoryTabPage());
        private void BtnTab3_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new PaymentTabPage());
        private void BtnTab4_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new DiagrammPage());
    }
}