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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var regWindow = new RegistrationWindow();
            regWindow.Owner = this; // Связываем окна
            regWindow.ShowDialog(); // Модальное окно
        }
    }
}
