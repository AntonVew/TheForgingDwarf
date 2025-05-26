using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ForgingDwarf.Client.Services;
using ForgingDwarf.Common.Models;

namespace ForgingDwarf.Client
{
    /// <summary>
    /// Логика взаимодействия для NewClientWindow.xaml
    /// </summary>
    public partial class NewClientWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();

        public NewClientWindow()
        {
            InitializeComponent();
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var newClient = new Common.Models.Client
            {
                Id = IdTextBox.Text.ToString(),
                Name = NameTextBox.Text.ToString(),
                Phone = PhoneTextBox.Text.ToString(),
                Email = EmailTextBox.Text.ToString(),
                IsSuperuser = false,
            };

            try
            {
                await _apiService.CreateClientAsync(newClient);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
