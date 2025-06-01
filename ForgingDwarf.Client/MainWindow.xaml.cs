using System.Windows;
using ForgingDwarf.Client.Services;
using ForgingDwarf.Common.Models;

namespace ForgingDwarf.Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ApiService _apiService = new ApiService();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadOrders();
        ComboStatus.ItemsSource = Enum.GetValues(typeof(OrderStatus));
        ComboStatus.SelectedIndex = 0;
        Loaded += async (s, e) => await LoadOrders();
    }

    private async Task LoadOrders()
    {
        var orders = await _apiService.GetOrdersWithClientsAsync();
        OrdersGrid.ItemsSource = orders;
    }

    private async void RefreshOrders_Click(object sender, RoutedEventArgs e)
    {
        await LoadOrders();
    }

    private void CreateOrder_Click(object sender, RoutedEventArgs e)
    {
        var createOrderWindow = new CreateOrderWindow(_apiService);
        createOrderWindow.Closed += async (s, args) => await LoadOrders();
        createOrderWindow.ShowDialog();
    }

    private async void DeleteOrder_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(SelectedID.Text, out int orderId))
        {
            MessageBox.Show("Введите корректный ID заказа");
            return;
        }

        var result = MessageBox.Show(
            $"Вы уверены, что хотите удалить заказ #{orderId}?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo);

        if (result != MessageBoxResult.Yes) return;

        bool isSuccess = await _apiService.DeleteOrderAsync(orderId);
        if (isSuccess)
        {
            MessageBox.Show("Заказ успешно удален");
            await LoadOrders();
        }
        else
        {
            MessageBox.Show("Ошибка при удалении заказа");
        }
    }

    private async void UpdateStatus_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(SelectedID.Text, out int orderId))
        {
            MessageBox.Show("Введите корректный ID заказа");
            return;
        }

        if (ComboStatus.SelectedItem == null)
        {
            MessageBox.Show("Выберите новый статус");
            return;
        }

        var newStatus = (OrderStatus)ComboStatus.SelectedItem;
        bool isSuccess = await _apiService.UpdateOrderStatusAsync(orderId, newStatus);

        if (isSuccess)
        {
            MessageBox.Show("Статус успешно изменен");
            await LoadOrders();
        }
        else
        {
            MessageBox.Show("Ошибка при изменении статуса");
        }
    }
}