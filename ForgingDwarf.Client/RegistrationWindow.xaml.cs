using System.Windows;
using System.Windows.Controls;
using ForgingDwarf.Client.Services;

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
            // Валидация паролей
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            var client = new Common.Models.Client
            {
                Name = NameBox.Text,
                Password = PasswordBox.Password, // Сервер хеширует пароль
                Email = string.IsNullOrWhiteSpace(EmailBox.Text) ? null : EmailBox.Text,
                Phone = string.IsNullOrWhiteSpace(PhoneBox.Text) ? null : PhoneBox.Text
            };

            // Отправка на сервер
            var success = await _apiService.RegisterAsync(client);
            if (success)
            {
                MessageBox.Show("Регистрация успешна!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка регистрации. Возможно, имя уже занято.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
