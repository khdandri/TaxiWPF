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

namespace kyrsovarobota.View
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private OrderManager _orderManager = new OrderManager();
        private const decimal RatePerKm = 15.0m;
        private const decimal BasePrice = 40.0m;

        public Main()
        {
            InitializeComponent();

            // Заповнюємо ComboBox вулицями
            FromComboBox.ItemsSource = RouteService.Streets;
            ToComboBox.ItemsSource = RouteService.Streets;

            LoadOrders();
        }

        private void Route_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (FromComboBox.SelectedItem == null || ToComboBox.SelectedItem == null) return;

            string from = FromComboBox.SelectedItem.ToString();
            string to = ToComboBox.SelectedItem.ToString();

            if (from == to)
            {
                MessageBox.Show("Не можливо подорожувати по одній вулиці!!");
                ToComboBox.SelectedIndex = -1;
                return;
            }

            double dist = RouteService.GetDistance(from, to);
            DistanceInput.Text = dist.ToString("F1"); // Показуємо 1 знак після коми

            decimal total = BasePrice + (decimal)dist * RatePerKm;
            PriceInput.Text = total.ToString("F2");
        }

        private void LoadOrders()
        {
            OrdersGrid.ItemsSource = null;
            OrdersGrid.ItemsSource = _orderManager.GetAll(); 
        }



        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (FromComboBox.SelectedItem == null || ToComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(ClientNameInput.Text))
            {
                MessageBox.Show("Заповніть всі поля!");
                return;
            }

            var newOrder = new Orders
            {
                ClientName = ClientNameInput.Text,
                FromAddress = FromComboBox.SelectedItem.ToString(),
                ToAddress = ToComboBox.SelectedItem.ToString(),
                Distance = double.Parse(DistanceInput.Text),
                TotalPrice = decimal.Parse(PriceInput.Text),
                Status = "Нове"
            };

            _orderManager.AddOrder(newOrder);
            LoadOrders();
            ClearInputs();
            MessageBox.Show("Замовлення успішно додано!");
        }

        private void ClearInputs()
        {
            ClientNameInput.Clear();
            FromComboBox.SelectedIndex = -1;
            ToComboBox.SelectedIndex = -1;
            DistanceInput.Clear();
            PriceInput.Clear();
        }

        
        private void DeleteOrder_Click(object sender, RoutedEventArgs e) 
        {
            if (OrdersGrid.SelectedItem is Orders selected) 
            { _orderManager.GetAll().Remove(selected); _orderManager.Save(); LoadOrders(); }
            MessageBox.Show("Замовлення успішно видалено з бази данних!");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
             Registration loginWindow = new Registration(); 
             loginWindow.Show();
            this.Close();
        }
    }
}
