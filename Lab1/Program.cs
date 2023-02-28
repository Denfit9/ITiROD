using System.Net;
using System.Net.Sockets;
using System.Reflection;
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
while (string.IsNullOrEmpty(username))
{
    Console.WriteLine("Empty names are not allowed\nEnter your name:  ");
    username = Console.ReadLine();
}
var localPort=0;
Console.Write("Enter a number of port to listen: ");
while (!int.TryParse(Console.ReadLine(), out  localPort))
{
    Console.Write("Such port is not allowed\nEnter a number of port to listen: ");
    //int.TryParse(Console.ReadLine(), out localPort);
}
    
Console.Write("Enter a number of port to send: ");
var remotePort = 0;
while (!int.TryParse(Console.ReadLine(), out remotePort))
{
    Console.Write("Such port is not allowed\nEnter a number of port to send: ");
    //int.TryParse(Console.ReadLine(), out remotePort);
}
Console.Clear();

string path = localPort + "," + remotePort + "=" + username + ".txt";
using (StreamWriter fileStream = File.Exists(path) ? File.AppendText(path) : File.CreateText(path))
{

}
using (StreamReader reader = new StreamReader(path))
{

    string text = await reader.ReadToEndAsync();
    if (string.IsNullOrEmpty(text))
    {

    }
    else
    {
        Console.WriteLine("Previous session messages: \n");
    }
    Console.WriteLine(text);
}

Task.Run(ReceiveMessageAsync);
await SendMessageAsync();

async Task SendMessageAsync()
{
    using Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    byte[] greet = Encoding.UTF8.GetBytes("Your interlocutor just connected");
    await sender.SendToAsync(greet, new IPEndPoint(localAddress, remotePort));
    Console.WriteLine("Enter a message and then press enter button to send it or type quit to leave ");
    while (true)
    {
        var message = Console.ReadLine();
        if(message == "quit")
        {
            byte[] left = Encoding.UTF8.GetBytes("Your interlocutor left");
            await sender.SendToAsync(left, new IPEndPoint(localAddress, remotePort));
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
            if(message== "Your interlocutor left" || message == "Your interlocutor just connected") { }
            else
            {
                writer.WriteLine(message);
            }
        }
    }
}
