using System.Windows;
using ForgingDwarf.Client.Services;

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
}