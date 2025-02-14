using System.Net.Sockets;
using System.Text;
using System.Windows;

public class TcpClient
{
    private System.Net.Sockets.TcpClient _client;
    private NetworkStream _stream;
    private Action<string> _onMessageReceived;

    public string UserNickname { get; set; }

    public TcpClient(string serverIp, int port, Action<string> onMessageReceived, string userNickname)
    {
        _client = new System.Net.Sockets.TcpClient(serverIp, port);
        _stream = _client.GetStream();
        _onMessageReceived = onMessageReceived;
        UserNickname = userNickname;  
    }

    public void Start()
    {
        Thread readThread = new Thread(ReadMessages);
        readThread.Start();
    }

    private void ReadMessages()
    {
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0) break;

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {message}");

            _onMessageReceived?.Invoke(message);
        }
    }

    public void SendMessage(string message)
    {
        if (_client == null)
        {
            MessageBox.Show("Please log in first.");
            return;
        }

        string formattedMessage = $"{UserNickname}: {message}"; 

        byte[] buffer = Encoding.UTF8.GetBytes(formattedMessage);
        _stream.Write(buffer, 0, buffer.Length);
    }
}
