using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class Server
{

    const int MAX_CONNECTION = 10;
    const int PORT_NUMBER = 9999;

    static TcpListener listener;

    static Dictionary<string, string> _data =
        new Dictionary<string, string>
    {
        {"0","Zero"},
        {"1","One"},
        {"2","Two"},
        {"3","Three"},
        {"4","Four"},
        {"5","Five"},
        {"6","Six"},
        {"7","Seven"},
        {"8","Eight"},
        {"9","Nine"},
        {"10","Ten"},
    };

    public static void Main()
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");

        listener = new TcpListener(address, PORT_NUMBER);
        Console.WriteLine("SERVER MULTI CLIENT CONNECTION - CODING BY NTK - 74458");
        Console.WriteLine("Waiting for connection...");
        listener.Start();

        for (int i = 0; i < MAX_CONNECTION; i++)
        {
            new Thread(DoWork).Start();
        }
    }

    static void DoWork()
    {
        while (true)
        {
            Socket soc = listener.AcceptSocket();

            Console.WriteLine("Connection received from: {0}",
                              soc.RemoteEndPoint);
            try
            {   
                var stream = new NetworkStream(soc);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;

                //writer.WriteLine("Please enter the number (0-10) : ");

                while (true)
                {
                    string id = reader.ReadLine();

                    if (id.ToUpper() == "EXIT")
                        writer.WriteLine("bye");
                        break; // disconnect

                    if (_data.ContainsKey(id))
                        writer.WriteLine("Number you've entered: '{0}'", _data[id]);
                    else
                        writer.WriteLine("Number is not valid !");
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            Console.WriteLine("Client disconnected: {0}",
                              soc.RemoteEndPoint);
            soc.Close();
        }
    }
}