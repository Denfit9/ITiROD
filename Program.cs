using System.Net;
using System.Net.Sockets;
using System.Text;

void ClearCurrentConsoleLine()
{
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
}

IPAddress localAddress = IPAddress.Parse("127.0.0.1");
Console.Write("Enter your name: ");
string? username = Console.ReadLine();
Console.Write("Enter a numebr of port to listen: ");
if (!int.TryParse(Console.ReadLine(), out var localPort)) return;
Console.Write("Enter a number of port to send: ");
if (!int.TryParse(Console.ReadLine(), out var remotePort)) return;
Console.WriteLine();

Task.Run(ReceiveMessageAsync);
await SendMessageAsync();

async Task SendMessageAsync()
{
    using Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    Console.WriteLine("Enter a message and then press enter button to send it");
    while (true)
    {
        var message = Console.ReadLine();
        var messageCopy = message;
        if (string.IsNullOrWhiteSpace(message)) break;
        message = $"{username}: {message}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        await sender.SendToAsync(data, new IPEndPoint(localAddress, remotePort));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        ClearCurrentConsoleLine();
        Console.WriteLine("You: " + messageCopy);
    }
}

async Task ReceiveMessageAsync()
{
    byte[] data = new byte[65535];
    using Socket receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    receiver.Bind(new IPEndPoint(localAddress, localPort));
    while (true)
    {
        var result = await receiver.ReceiveFromAsync(data, new IPEndPoint(IPAddress.Any, 0));
        var message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);
        Console.WriteLine(message);
    }
}
