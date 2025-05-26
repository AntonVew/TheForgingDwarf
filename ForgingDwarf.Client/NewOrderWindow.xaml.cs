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
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ForgingDwarf.Client
{
    /// <summary>
    /// Логика взаимодействия для NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Common.Models.Client>? _clients;

        public NewOrderWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadClients();
        }

        private async Task LoadClients()
        {
            _clients = await _apiService.GetClientsAsync();
            ClientIdComboBox.ItemsSource = _clients;
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var newOrder = new Order
            {
                Id = IdTextBox.Text.ToString(),
                ClientId = ((ComboBoxItem)ClientIdComboBox.SelectedItem).Content.ToString(),
                ItemType = ((ComboBoxItem)ItemTypeComboBox.SelectedItem).Content.ToString(),
                SteelType = ((ComboBoxItem)SteelTypeComboBox.SelectedItem).Content.ToString(),
                Style = ((ComboBoxItem)StyleComboBox.SelectedItem).Content.ToString(),
                IsCustom = IsCustomChekBox.IsChecked ?? false,
                OrderDate = DateTime.Now,
                Status = "В очереди"
            };

            try
            {
                await _apiService.CreateOrderAsync(newOrder);
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
