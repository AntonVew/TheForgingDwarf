using System.Windows;
using ForgingDwarf.Client.Services;

namespace ForgingDwarf.Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ApiService _apiService = new ApiService();

    private async void LoadOrders_Click(object sender, RoutedEventArgs e)
    {
        //var orders = await _apiService.GetOrdersAsync();
        //OrdersGrid.ItemsSource = orders;
    }
}