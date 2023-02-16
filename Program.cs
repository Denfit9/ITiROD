using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
string path = localPort + "," + remotePort + "=" + username + ".txt";
using (StreamWriter fileStream = File.Exists(path) ? File.AppendText(path) : File.CreateText(path))
{
   
}
using (StreamReader reader = new StreamReader(path))
{
    string text = await reader.ReadToEndAsync();
    Console.WriteLine(text);
}

Task.Run(ReceiveMessageAsync);
await SendMessageAsync();

async Task SendMessageAsync()
{
    using Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    Console.WriteLine("Enter a message and then press enter button to send it or type quit to leave ");
    while (true)
    {
        var message = Console.ReadLine();
        if(message == "quit")
        {
            return;
        }
        var messageCopy = message;
        if (string.IsNullOrWhiteSpace(message)) break;
        message = $"{username}: {message}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        await sender.SendToAsync(data, new IPEndPoint(localAddress, remotePort));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        ClearCurrentConsoleLine();
        Console.WriteLine("You: " + messageCopy);
        using (StreamWriter writer = new StreamWriter(path, true, System.Text.Encoding.Default))
        {
           writer.WriteLine("You: " + messageCopy);
        }
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
        using (StreamWriter writer = new StreamWriter(path, true, System.Text.Encoding.Default))
        {
            writer.WriteLine(message);
        }
    }
}
