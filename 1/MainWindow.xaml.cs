using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PaymentApp.Pages; 

namespace PaymentApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private const string ThemeFile = "theme.txt";
        private const string LightTheme = "Dictionary.xaml";
        private const string DarkTheme = "DarkTheme.xaml";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (o, t) => DateTimeNow.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            _timer.Start();
            DateTimeNow.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            string savedTheme = LightTheme;
            if (System.IO.File.Exists(ThemeFile))
                savedTheme = System.IO.File.ReadAllText(ThemeFile).Trim();
            ApplyTheme(savedTheme);

            MainFrame.Navigate(new AuthPage());
        }

        private void ButtonTheme_Click(object sender, RoutedEventArgs e)
        {
            string currentTheme = LightTheme;
            if (System.IO.File.Exists(ThemeFile))
                currentTheme = System.IO.File.ReadAllText(ThemeFile).Trim();

            string newTheme = (currentTheme == DarkTheme) ? LightTheme : DarkTheme;
            ApplyTheme(newTheme);
        }

        private void ApplyTheme(string themeFile)
        {
            try
            {
                var uri = new Uri(themeFile, UriKind.Relative);
                var dict = Application.LoadComponent(uri) as ResourceDictionary;
                if (dict != null)
                {
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                    System.IO.File.WriteAllText(ThemeFile, themeFile);
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки темы: " + ex.Message); }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content is Page page)
            {
                string title = page.Tag as string ?? page.GetType().Name.Replace("Page", "");
                PageTitle.Text = title;
                Title = "Учёт платежей — " + title;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите закрыть приложение?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) e.Cancel = true;
            else _timer?.Stop();
        }
    }
}