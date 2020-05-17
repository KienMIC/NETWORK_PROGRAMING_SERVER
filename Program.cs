using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    const int PORT_NUMBER = 9999;

    static TcpListener listener;
    static List<EndPoint> ConnectedClients = new List<EndPoint>();

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

    public static void Main(string []args)
    {
        int MAX_CONNECTION = Convert.ToInt32(args[0]);
        IPAddress address = IPAddress.Parse("127.0.0.1");

        listener = new TcpListener(address, PORT_NUMBER);
        Console.WriteLine("SERVER MULTI CLIENT CONNECTION - CODING BY NTK - 74458");
        Console.WriteLine("IP:Port of Server : " + address + ":" + PORT_NUMBER);
        Console.WriteLine("Waiting for connection...");
        listener.Start();

        for (int i = 1; i <= MAX_CONNECTION+1; i++)
        {
            if (i <= MAX_CONNECTION)
            {
                new Thread(DoWork).Start();
            }
            else
            {
                new Thread(DoWork_Deny).Start();
            }
        }
    }
    static void DoWork_Deny()
    {
        while (true)
        {
            Socket soc = listener.AcceptSocket();
            var stream = new NetworkStream(soc);
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            Console.WriteLine("Connection received from(DENIED): {0}",
                                  soc.RemoteEndPoint);
            writer.WriteLine("503 Service Unavalible, Close Connection !");
            stream.Close();
            //soc.Close();
        }
    }
    static void DoWork()
    {
        while (true)
        {
            Socket soc = listener.AcceptSocket();


            Console.WriteLine("Connection received from: {0}",
                              soc.RemoteEndPoint);
            StringBuilder sb = new StringBuilder();
            sb.Append("IP:Port of Client: " + soc.RemoteEndPoint + ";" + "Connect At: " + DateTime.Now);
            foreach (EndPoint EP in ConnectedClients)
            {
                if (soc.RemoteEndPoint == EP)
                {
                    var stream = new NetworkStream(soc);
                    var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;
                    writer.WriteLine("429 Many Request !");
                    sb.Append(" Disconect At : " + DateTime.Now + ";" + "Reason : 429 Many Request");
                    File.AppendAllText("Access.log", sb.ToString());
                    stream.Close();
                    soc.Close();
                }
                else
                {
                    ConnectedClients.Add(soc.RemoteEndPoint);
                }
            }
            try
            {   
                var stream = new NetworkStream(soc);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;
               

                while (true)
                {
                    string id = reader.ReadLine();

                    if (id.ToUpper() == "EXIT")
                    {
                        writer.WriteLine("BYE");
                        sb.Append(";Disconnect At: " + DateTime.Now+";"+"Reason: Closed By Client\n");
                        File.AppendAllText("D://access.log", sb.ToString());
                        sb.Clear();
                        break; // disconnect
                    }
                        

                    if (_data.ContainsKey(id))
                        writer.WriteLine("Number you've entered: '{0}'", _data[id]);
                    else if (id.Contains("dig"))
                    {
                        string[] idcut = id.Split(' ');
                        var ipaddress = Dns.GetHostAddresses(idcut[1])[0];
                        writer.WriteLine("IP of Domain is : " + ipaddress);
                    }
                    else if (id.Contains("curl"))
                    {
                        string[] idcut = id.Split(' ');
                        WebClient Client = new WebClient();
                        Client.DownloadFile(idcut[1],"myfile.txt");
                        writer.WriteLine("Content of Website has been downloaded !");
                    }
                    else
                    {
                        writer.WriteLine("Wrong Syntax ! ");
                    }
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            Console.WriteLine("Client disconnected: {0}",
                              soc.RemoteEndPoint);
            ConnectedClients.Remove(soc.RemoteEndPoint);
            soc.Close();
        }
    }
}