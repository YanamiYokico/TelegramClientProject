using ClientTelegram;
using System.Net.Sockets;
using System.Text;
using System.Windows;

public class TcpClient
{
    private System.Net.Sockets.TcpClient _client;
    private NetworkStream _stream;
    private Action<string> _onMessageReceived;
    private string _currentChat = string.Empty;
    public string UserNickname { get; set; }

    public TcpClient(string serverIp, int port, Action<string> onMessageReceived, string userNickname)
    {
        _client = new System.Net.Sockets.TcpClient(serverIp, port);  // Используем встроенный TcpClient
        _stream = _client.GetStream();  // Получаем поток данных
        _onMessageReceived = onMessageReceived;
        UserNickname = userNickname;
    }

    // Запускаем поток для чтения сообщений
    public void Start()
    {
        Thread readThread = new Thread(ReadMessages);
        readThread.IsBackground = true;  // Поток завершится при закрытии приложения
        readThread.Start();
    }

    // Чтение сообщений от сервера
    private void ReadMessages()
    {
        byte[] buffer = new byte[1024];

        while (true)
        {
            try
            {
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                _onMessageReceived?.Invoke(message);  // Вызываем обработчик сообщений
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading message: {ex.Message}");
                break;
            }
        }
    }

    // Отправка сообщения на сервер
    public void SendMessage(string message)
    {
        if (_client == null)
        {
            MessageBox.Show("Please log in first.");
            return;
        }

        string formattedMessage = $"{UserNickname}: {message}";

        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(formattedMessage);
            _stream.Write(buffer, 0, buffer.Length);  // Отправка сообщения через поток
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while sending message: {ex.Message}");
        }
    }
}
