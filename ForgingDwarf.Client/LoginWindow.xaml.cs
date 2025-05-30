using System.Windows;
using System.Windows.Controls;
using ForgingDwarf.Client.Services;
using ForgingDwarf.Common.Models;

namespace ForgingDwarf.Client
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private readonly ApiService _apiService = new();

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (await _apiService.LoginAsync(NameBox.Text, PasswordBox.Password))
            {
                new MainWindow().Show(); // Основное окно приложения
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка входа");
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var client = new Common.Models.Client { Name = NameBox.Text, Password = PasswordBox.Password };
            if (await _apiService.RegisterAsync(client))
                MessageBox.Show("Регистрация успешна");
        }
    }
}
