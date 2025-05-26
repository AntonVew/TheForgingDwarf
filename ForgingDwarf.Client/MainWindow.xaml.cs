using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        Loaded += async (s, e) => await RefreshOrders();
    }

    private async Task RefreshOrders()
    {
        try
        {
            var orders = await _apiService.GetOrdersAsync();
            OrdersGrid.ItemsSource = orders;
            StatusText.Text = $"Загружено заказов: {orders.Count}";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Ошибка: {ex.Message}";
        }
    }

    private async void NewOrder_Click(object sender, RoutedEventArgs e)
    {
        var window = new NewOrderWindow();
        if (window.ShowDialog() == true)
        {
            await RefreshOrders();
        }
    }

    private async void NewClient_Click(object sender, RoutedEventArgs e)
    {
        var window = new NewClientWindow();
        if (window.ShowDialog() == true)
        {
            await RefreshOrders();
        }
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e)
    {
        await RefreshOrders();
    }
}