using NP_03._TCP_Task_Manger_Listener;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;
var listener = new TcpListener(ip, port);

listener.Start();

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var bw = new BinaryWriter(stream);
    while (true)
    {
        var input = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(input);
        Console.WriteLine(command.Text);
        Console.WriteLine(command.Param);
        switch (command.Text)
        {
            case Command.ProccessList:
                var processes = Process.GetProcesses();
                var processesNames = JsonSerializer
                    .Serialize(processes.Select(p => p.ProcessName));
                bw.Write(processesNames);
                break;
            case Command.Run:
                try
                {
                    Process.Start(command.Param);
                    Console.WriteLine($"Started process: {command.Param}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting process: {ex.Message}");
                }
                break;
            case Command.Kill:
                try
                {
                    var processToKill = Process.GetProcessesByName(command.Param).FirstOrDefault();
                    if (processToKill != null)
                    {
                        processToKill.Kill();
                        Console.WriteLine($"Killed process: {command.Param}");
                    }
                    else
                    {
                        Console.WriteLine($"Process not found: {command.Param}. Make sure the process name is correct.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error killing process: {ex.Message}. Ensure you have the necessary permissions.");
                }

                break;
            default:
                Console.WriteLine("Unknown command");
                break;
        }
    }
}