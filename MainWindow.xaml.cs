using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClientTelegram
{
    public partial class MainWindow : Window
    {
        private TcpClient _client;
        private LoginWindow _loginWindow;
        public string UserNickname { get; set; } = string.Empty;
        private string _currentChat = string.Empty; // Активный чат

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
            if (string.IsNullOrWhiteSpace(message)) return;

       
            _client.SendMessage($"/msg {_currentChat} {message}");

            MessageInput.Clear();
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

                    if (!string.IsNullOrEmpty(_currentChat) && message.Contains(_currentChat))
                    {
                        var chatMessage = new MessageItem(nickname, content);
                        MessageList.Items.Add(chatMessage);
                        MessageList.ScrollIntoView(chatMessage); 
                    }
                }
            });
        }

        public void SetUserNickname(string nickname)
        {
            UserNickname = nickname;
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            string chatName = "New Chat " + (ChatList.Items.Count + 1);
            ListBoxItem newChat = new ListBoxItem { Content = chatName };
            ChatList.Items.Add(newChat);
        }

        private void JoinChat_Click(object sender, RoutedEventArgs e)
        {
            if (ChatList.SelectedItem is ListBoxItem selectedChat)
            {
                string chatName = selectedChat.Content.ToString();
                _client.SendMessage($"/join {chatName}");
                _currentChat = chatName; 
                MessageBox.Show($"Joined chat: {chatName}");


                MessageList.Items.Clear();
            }
            else
            {
                MessageBox.Show("Please select a chat to join.");
            }
        }
    }
}
