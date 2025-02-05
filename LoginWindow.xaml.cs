using ClientTelegram.Data;
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

namespace ClientTelegram
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly MainWindow _mainWindow;
        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
            RegisterWindow registerWindow = new RegisterWindow(_mainWindow);
            registerWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            registerWindow.Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using (var telegramContext = new TelegramContext())
            {
                var user = telegramContext.Users!.ToList().FirstOrDefault(u => u.Name == NameTextBox.Text && BCrypt.Net.BCrypt.Verify(PasswordTextBox.Password, BCrypt.Net.BCrypt.HashPassword(PasswordTextBox.Password)));

                if (user != null)
                {
                    _mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Such user is not found","Found error",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
