﻿using System.Net.Sockets;
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

namespace ClientTelegram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoginWindow loginWindow = new LoginWindow(this);
            loginWindow.Show();

            Hide();
        }

        private void AddContact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Blacklist_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}