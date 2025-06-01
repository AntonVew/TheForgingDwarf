using ForgingDwarf.Client.Services;
using ForgingDwarf.Common.Models;
using System.Windows;

namespace ForgingDwarf.Client
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        private readonly ApiService _apiService;

        public CreateOrderWindow(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += CreateOrderWindow_Loaded;
        }

        private async void CreateOrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var clients = await _apiService.GetClientsAsync();
            ClientCombo.ItemsSource = clients;

            ItemTypeCombo.ItemsSource = Enum.GetValues(typeof(ItemType));
            SteelTypeCombo.ItemsSource = Enum.GetValues(typeof(SteelType));
            StyleCombo.ItemsSource = Enum.GetValues(typeof(Common.Models.Style));
            StatusCombo.ItemsSource = Enum.GetValues(typeof(OrderStatus));

            // по умолчанию
            ItemTypeCombo.SelectedIndex = 0;
            SteelTypeCombo.SelectedIndex = 0;
            StyleCombo.SelectedIndex = 0;
            StatusCombo.SelectedIndex = 0;
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(PriceTextBox.Text, out var price))
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }

            var order = new Order
            {
                ClientId = ((Common.Models.Client)ClientCombo.SelectedItem).Id,
                ItemType = (ItemType)ItemTypeCombo.SelectedItem,
                SteelType = (SteelType)SteelTypeCombo.SelectedItem,
                Style = (Common.Models.Style)StyleCombo.SelectedItem,
                Status = (OrderStatus)StatusCombo.SelectedItem,
                IsCustom = IsCustomCheck.IsChecked ?? false,
                Price = (double)price,
                OrderDate = DateTime.Now
            };

            var success = await _apiService.CreateOrderAsync(order);

            if (success)
            {
                MessageBox.Show("Заказ успешно создан!");
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка при создании заказа");
            }
        }
    }
}
