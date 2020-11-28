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

            //Dichiarazione EndPoint del Server
            IPAddress ipAddress = null;
            string strIPAddress = "";
            string strPort = "";
            int nPort;

            //Dichiarazione Variabili per comunicare con il server
            string sendString = "";
            string receivedString = "";
            byte[] sendBuff = new byte[128];
            byte[] recvBuff = new byte[128];
            int nReceivedBytes = 0;

            try
            {
                // Settagio da Console dell'EndPoint
                Console.WriteLine("Benvenuto nel Client Socket");
                Console.Write("IP del Server: ");
                strIPAddress = Console.ReadLine();
                Console.Write("Porta del Server: ");
                strPort = Console.ReadLine();

                if (!IPAddress.TryParse(strIPAddress.Trim(), out ipAddress))
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

                //Connessione al Server
                client.Connect(ipAddress, nPort);

                //Inizio chat con il server
                Console.WriteLine("Chatta con il server. ");

                while (true)
                {
                    // Prendo il messaggio & condizione di uscita
                    sendString = Console.ReadLine();

                    //Dico al Server di interrompersi
                    sendBuff = Encoding.ASCII.GetBytes(sendString);
                    client.Send(sendBuff);

                    if (sendString.ToUpper().Trim()=="QUI")
                    {
                        break;
                    }

                    //Pulisco il buffer e ricevo il messaggio
                    Array.Clear(recvBuff, 0, recvBuff.Length);
                    nReceivedBytes = client.Receive(recvBuff);
                    receivedString = Encoding.ASCII.GetString(recvBuff);
                    Console.WriteLine("S: " + receivedString);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                /* In ogni occasione chiudo la connessione per sicurezza */
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);//disabilita la send e receive
                    }
                    client.Close();
                    client.Dispose();
                }
            }
            Console.WriteLine("Premi Enter per chiudere...");
            Console.ReadLine();
        }
    }
}
