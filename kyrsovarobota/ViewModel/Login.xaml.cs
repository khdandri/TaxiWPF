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
using kyrsovarobota.Model;


namespace kyrsovarobota.View
{

        public partial class Login : Window
        {
            public Login()
            {
                InitializeComponent();
            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                MainWindow registerWindow = new MainWindow();
                registerWindow.Show();
                this.Close();
            }

        private void BtnShowPass_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Password_visible.Text = Password_login.Password;
            Password_visible.Visibility = Visibility.Visible;
            Password_login.Visibility = Visibility.Collapsed;
        }

        private void BtnShowPass_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        private void BtnShowPass_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void HidePassword()
        {
            Password_visible.Visibility = Visibility.Collapsed;
            Password_login.Visibility = Visibility.Visible;
            Password_login.Focus();
        }
    

        private void Login_Click(object sender, RoutedEventArgs e)
            {
                UserManager _userManager = new UserManager();
                string username = Username_login.Text;
                string password = Password_login.Password;

                string errorMessage;
                User authenticatedUser = _userManager.AuthenticateUser(username, password, out errorMessage);

                if (authenticatedUser != null)
                {
                    MessageBox.Show($"Welcome, {authenticatedUser.Username}!", "Succesful authorization", MessageBoxButton.OK, MessageBoxImage.Information);
                    Main mainWindow = new Main();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(errorMessage, "Autorization Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }
}

