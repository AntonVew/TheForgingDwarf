using System.Windows;
using System.Windows.Controls;
using ForgingDwarf.Client.Services;
using ForgingDwarf.Server.Controllers;

namespace ForgingDwarf.Client
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private readonly ApiService _apiService = new();
        public RegistrationWindow()
        {
            InitializeComponent();
        }
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new AuthController.ClientRegistrationDto
            {
                Name = NameBox.Text.Trim(),
                Password = PasswordBox.Password,
                Email = EmailBox.Text.Trim(),
                Phone = PhoneBox.Text.Trim()
            };

            var result = await _apiService.RegisterAsync(request);

            if (result.IsSuccess)
            {
                MessageBox.Show("Регистрация успешна!");
                Close();
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                if (result.ErrorMessage.Contains("имя")) NameBox.Focus();
                else if (result.ErrorMessage.Contains("пароль")) PasswordBox.Focus();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
