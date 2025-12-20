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
        public partial class Registration : Window
        {
            public Registration()
            {
                InitializeComponent();
            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                MainWindow mainwindow = new MainWindow();
                mainwindow.Show();
                this.Close();
            }

            private void Register(object sender, RoutedEventArgs e)
            {
                UserManager _userManager = new UserManager();
                string username = UsernameTextBox.Text;
                string password = PasswordTextBox.Text;
                string confirmPassword = confirmPasswordTextBox.Text;

                if (password != confirmPassword)
                {
                    MessageBox.Show("Паролі не співпадають!", "Помилка реєстрації", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string errorMessage;
                bool success = _userManager.RegisterUser(username, password, out errorMessage);

                if (success)
                {
                    MessageBox.Show("Реєстрація успішна! Тепер ви можете увійти.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    OpenLoginWindow();
                }
                else
                {
                    MessageBox.Show(errorMessage, "Помилка реєстрації", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            private void SignInButton_Click(object sender, RoutedEventArgs e)
            {
                OpenLoginWindow();
            }

            private void OpenLoginWindow()
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
            

        }
    }

