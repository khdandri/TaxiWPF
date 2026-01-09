using kyrsovarobota.Model;
using kyrsovarobota.Services;
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
using System.Windows.Threading;

namespace kyrsovarobota.View
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private OrderManager _orderManager = new OrderManager();
        private List<Orders> _sessionHistory = new List<Orders>(); // Тимчасова історія
        private DispatcherTimer _expirationTimer;
        private const decimal RatePerKm = 15.0m;
        private const decimal BasePrice = 40.0m;

        public Main()
        {
            InitializeComponent();
            FromComboBox.ItemsSource = RouteService.Streets;
            ToComboBox.ItemsSource = RouteService.Streets;
            CarClassComboBox.ItemsSource = DriverService.CarsClasses;

            // Таймер для перевірки 5-хвилинного ліміту
            _expirationTimer = new DispatcherTimer();
            _expirationTimer.Interval = TimeSpan.FromSeconds(30);
            _expirationTimer.Tick += (s, e) => CheckOrdersTime();
            _expirationTimer.Start();

            LoadOrders();
        }

        private void CheckOrdersTime()
        {
            var now = DateTime.Now;
            var orders = _orderManager.GetAll().ToList();
            bool changed = false;

            foreach (var order in orders.Where(o => o.Status == "New"))
            {
                if ((now - order.CreatedAt).TotalMinutes >= 1)
                {
                    order.Status = "Cancelled";
                    changed = true;
                    // Запускаємо видалення через 1 хв
                    ScheduleAutoDeletion(order);
                }
            }

            if (changed) { _orderManager.Save(); LoadOrders(); }
        }

        // ПУНКТ 1 ТА 2: Розрахунок ціни залежно від класу
        private void UpdatePrice()
        {
            if (FromComboBox.SelectedItem == null || ToComboBox.SelectedItem == null || CarClassComboBox.SelectedItem == null) return;

            double dist = RouteService.GetDistance(FromComboBox.SelectedItem.ToString(), ToComboBox.SelectedItem.ToString());
            DistanceInput.Text = dist.ToString("F1");

            decimal price = BasePrice + (decimal)dist * RatePerKm;
            string carClass = CarClassComboBox.SelectedItem.ToString();

            // Логіка коефіцієнтів
            switch (carClass)
            {
                case "Economy": price /= 2.0m; break;
                case "Standard": price /= 1.5m; break;
                case "Business": break; // Без змін
                case "Premium": price *= 1.5m; break;
            }

            PriceInput.Text = price.ToString("F2");
        }

        private void Route_Changed(object sender, SelectionChangedEventArgs e) => UpdatePrice();
        private void CarsClass_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdatePrice();
            if (CarClassComboBox.SelectedItem != null)
            {
                string cls = CarClassComboBox.SelectedItem.ToString();
                DriverComboBox.ItemsSource = DriverService.drivers.Where(d => d.CarClass == cls).Select(d => d.Name).ToList();
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClientNameInput.Text) || CarClassComboBox.SelectedItem == null)
            {
                MessageBox.Show( "Fill in all customers data and car classes!");
                return;
            }

            var order = new Orders
            {
                ClientName = ClientNameInput.Text,
                FromAddress = FromComboBox.SelectedItem.ToString(),
                ToAddress = ToComboBox.SelectedItem.ToString(),
                Distance = double.Parse(DistanceInput.Text),
                TotalPrice = decimal.Parse(PriceInput.Text),
                Status = "New",
                CreatedAt = DateTime.Now,
                DriverName = "Not assigned"
            };

            MessageBox.Show("Order add succesfuly, add driver");
            _orderManager.AddOrder(order);
            LoadOrders();
            ClearInputs();
        }

        private void AssignDriver_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is Orders selected && DriverComboBox.SelectedItem != null)
            {
                if (selected.Status != "New") return;

                selected.DriverName = DriverComboBox.SelectedItem.ToString();
                selected.Status = "Done";
                _orderManager.Save();
                LoadOrders();

                // ПУНКТ 3: Видалення через хвилину
                ScheduleAutoDeletion(selected);
            }
        }

        private async void ScheduleAutoDeletion(Orders order)
        {
            await Task.Delay(60000); // Чекаємо 1 хвилину

            // Переносимо в історію
            _sessionHistory.Add(order);
            HistoryListBox.ItemsSource = null;
            HistoryListBox.ItemsSource = _sessionHistory;

            // Видаляємо з основної бази
            _orderManager.GetAll().Remove(order);
            _orderManager.Save();

            Dispatcher.Invoke(() => LoadOrders());
        }

        // ЕЛЕМЕНТ ІНТЕРФЕЙСУ ДЛЯ ІСТОРІЇ
        private void ShowHistory_Click(object sender, RoutedEventArgs e) 
        {
            HistoryPanel.Visibility = Visibility.Visible; 
        }
        private void CloseHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryPanel.Visibility = Visibility.Collapsed;
        }

        private void LoadOrders()
        {
            OrdersGrid.ItemsSource = null; OrdersGrid.ItemsSource = _orderManager.GetAll(); 
        }
        private void ClearInputs()
        {
            ClientNameInput.Clear(); PriceInput.Clear(); DistanceInput.Clear(); 
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is Orders s) { _orderManager.GetAll().Remove(s); _orderManager.Save(); LoadOrders(); }
            MessageBox.Show("Order succesfuly deleted.");
        }

        private void Logout_Click(object sender, RoutedEventArgs e) 
        {
            new Registration().Show(); this.Close(); 
        }
    }
}
