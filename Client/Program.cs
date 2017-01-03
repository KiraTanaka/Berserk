using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            const int port = 8888;

            var remoteEndPoint = new IPEndPoint(ipAddress, port);

            var socketClient = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            socketClient.Connect(remoteEndPoint);
            Console.WriteLine($"Connected: {socketClient.RemoteEndPoint}");

            byte[] data = Encoding.ASCII.GetBytes("Hello World!");
            socketClient.Send(data);

            Console.ReadLine();
        }

        public static void StartClient()
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try
            {
                var sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                
                try
                {
                    const string host = "localhost";
                    const int port = 8888;

                    sender.Connect(host, port);
                    Console.WriteLine($"Socket connected to {sender.RemoteEndPoint}");


                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine($"ArgumentNullException : {e}");
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"SocketException : {e}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected exception : {e}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
