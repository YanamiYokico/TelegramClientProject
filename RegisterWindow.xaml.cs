using ClientTelegram.Data;
using ClientTelegram.Models;
using Microsoft.SqlServer.Server;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BCrypt;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BCrypt.Net;

namespace ClientTelegram
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly MainWindow _mainWindow;
        public RegisterWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            PhoneTextBox.Text = "+380 12 345 67 89";
            PhoneTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            DataContext = this;
            _mainWindow = mainWindow;
        }

        private void PhoneTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PhoneTextBox.Text == "+380 12 345 67 89")
            {
                PhoneTextBox.Text = string.Empty;
                PhoneTextBox.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void PhoneTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PhoneTextBox.Text))
            {
                PhoneTextBox.Text = "+380 12 345 67 89";
                PhoneTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidatePhone(PhoneTextBox.Text))
                {
                    MessageBox.Show("Invalid phone number format.Example: +380123456789", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!ValidatePassword(PasswordTextBox.Password))
                {
                    MessageBox.Show("Password must be at least 8 characters long and include uppercase letters, lowercase letters, and digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (var telegramContext = new TelegramContext())
                {
                    var registeredUser = new Users
                    {
                        Name = NameTextBox.Text,
                        Password = BCrypt.Net.BCrypt.HashPassword(PasswordTextBox.Password),
                        Phone = PhoneTextBox.Text,
                    };

                    telegramContext.Users!.Add(registeredUser);
                    telegramContext.SaveChanges();
                }

                MessageBox.Show("User has been successfully registered");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            NameTextBox.Text = string.Empty;
            PasswordTextBox.Password = string.Empty;
            PhoneTextBox.Text = string.Empty;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Close();

                var loginWindow = new LoginWindow(_mainWindow)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна: {ex.Message}");
            }
        }

        private bool ValidatePhone(string phone)
        {
            string phonePattern = @"^\+380\d{9}$";
            return Regex.IsMatch(phone, phonePattern);
        }

        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
            bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");

            return hasUpperCase && hasLowerCase && hasDigit;
        }
    }
}
