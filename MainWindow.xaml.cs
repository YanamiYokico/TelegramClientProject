using System.Net.Http;
using System.Net.Sockets;
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
        private TcpClient _client;
        private LoginWindow _loginWindow;
        public string UserNickname { get; set; } = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            _loginWindow = new LoginWindow(this);
            _loginWindow.Show();
            this.Hide();
        }

        public void InitializeClient()
        {
            _client = new TcpClient("127.0.0.1", 8080, ReceiveMessage, UserNickname);
            _client.Start();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (_client == null)
            {
                MessageBox.Show("Please log in first.");
                return;
            }

            string message = MessageInput.Text;
            _client.SendMessage(message);
            MessageInput.Clear();
        }

        private void AddContact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Blacklist_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ReceiveMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                string[] messageParts = message.Split(':', 2);
                if (messageParts.Length == 2)
                {
                    string nickname = messageParts[0];
                    string content = messageParts[1];

                    var chatMessage = new MessageItem(nickname, content);

                    MessageList.Items.Add(chatMessage);
                }
            });
        }


        public void SetUserNickname(string nickname)
        {
            UserNickname = nickname;
        }
    }
}