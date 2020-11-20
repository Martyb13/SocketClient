using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SoketClient_Biagioni
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            string strIPAddress = "";
            string strPort = "";
            IPAddress ipAddress = null;
            int nPort;

            try
            {
                Console.WriteLine("IP del server: ");
                strIPAddress = Console.ReadLine();
                Console.WriteLine("Porta del server: ");
                strPort = Console.ReadLine();
                
                if(!IPAddress.TryParse(strIPAddress.Trim(), out ipAddress))
                {
                    Console.WriteLine("IP non valido.");
                    return;
                }
                if (!int.TryParse(strPort.Trim(), out nPort))
                {
                    Console.WriteLine("Porta non valida.");
                    return;
                }
                if (nPort <= 0 || nPort >= 65535) 
                {
                    Console.WriteLine("Porta non valida.");
                    return;
                }
                Console.WriteLine("End Point del Server " + ipAddress.ToString() + " " + nPort);
                client.Connect(ipAddress, nPort);

                byte[] buff = new byte[128];
                string sendString = "";
                string receivedString = "";
                int receivedBytes = 0;

                while(true)
                {
                    Console.WriteLine("Manda un messaggio: ");
                    sendString = Console.ReadLine();
                    Encoding.ASCII.GetBytes(sendString).CopyTo(buff, 0);
                    client.Send(buff);

                    if(sendString.ToUpper().Trim()=="QUI")
                    {
                        break;
                    }

                    Array.Clear(buff, 0, buff.Length);
                    receivedBytes = client.Receive(buff);
                    receivedString = Encoding.ASCII.GetString(buff, 0, receivedBytes);
                    Console.WriteLine("S: " + receivedString);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
