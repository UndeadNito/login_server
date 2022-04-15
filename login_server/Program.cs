using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace login_server
{
    class Program
    {
        static readonly int port = 2020;
        static readonly IPAddress address = Dns.GetHostEntry( IPAddress.Parse("127.0.0.1")).AddressList.Last();
        static readonly TcpListener server;
        static private bool isServerActive = false;
        static readonly Logger log;

        static Program()
        {
            // Program entry point

            try
            {
                server = new TcpListener(address, port);
                isServerActive = true;
                log = new Logger(address.ToString(), port.ToString());
                server.Start();
                SQLControl.SQLOpenConnection();
            }
            catch(Exception e)
            {
                Logger.Error(e);
            }
        }

        static void NewClientThread()
        {
            // Thread for new client

            new ClientProcess(server.AcceptTcpClient());
        }

        static int Main()
        {
            // Method wating for new connections

            if (!isServerActive) { return 1; }

            while (isServerActive)
            {
                if (server.Pending())
                {
                    new Thread(new ThreadStart(NewClientThread)).Start();
                }
            }

            SQLControl.SQLCloseConnection();
            log.ServerShuttedDown();
            return 0;
        }

        static void Main1()
        {

            Subscribe s = Subscribe.CreateSubscribe(1609); //1609

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            System.Numerics.BigInteger result = s.EncryptHash(new System.Numerics.BigInteger(123456));
            Console.WriteLine(s.DecryptHash(result));
            
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();
        }
    }
}
