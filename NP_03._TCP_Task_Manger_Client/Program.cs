using NP_03._TCP_Task_Manger_Client;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;
var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);
Command command = null;
string responce = null;
string str = null;

while (true)
{
    Console.WriteLine("Write command or HELP");
    str = Console.ReadLine().ToUpper();
    if (str == "HELP")
    {
        Console.WriteLine();
        Console.WriteLine("Command list:");
        Console.WriteLine(Command.ProccessList);
        Console.WriteLine($"{Command.Run} <process_name>");
        Console.WriteLine($"{Command.Kill} <process_name>");
        Console.WriteLine("HELP");
        Console.ReadLine();
        Console.Clear();
    }
    var input = str.Split(' ');

    switch (input[0])
    {
        case Command.ProccessList:
            command = new Command { Text = input[0] };
            bw.Write(JsonSerializer.Serialize(command));
            responce = br.ReadString();
            var processList = JsonSerializer.Deserialize<string[]>(responce);
            foreach (var item in processList)
            {
                Console.WriteLine($"        {item}");
            }
            break;
        case Command.Run:
            if (input.Length > 1)
            {
                command = new Command { Text = input[0], Param = input[1] };
                bw.Write(JsonSerializer.Serialize(command));
                Console.WriteLine($"Sent command to run: {input[1]}");
            }
            else
            {
                Console.WriteLine("Please provide the name of the process to run.");
            }
            break;
        case Command.Kill:
            if (input.Length > 1)
            {
                command = new Command { Text = input[0], Param = input[1] };
                bw.Write(JsonSerializer.Serialize(command));
                Console.WriteLine($"Sent command to kill: {input[1]}");
            }
            else
            {
                Console.WriteLine("Please provide the name of the process to kill.");
            }

            break;
    }
    Console.WriteLine("Press any key to continue");
    Console.ReadLine();
    Console.Clear();

}