using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace login_server //admin pass; user password
{
    class ClientProcess
    {
        private readonly TcpClient client;
        private readonly NetworkStream clientStream;
        private readonly Security sec;
        private readonly Logger log;

        public ClientProcess(TcpClient getClient)
        {
            client = getClient;
            clientStream = getClient.GetStream();

            log = new Logger(client.Client.RemoteEndPoint.ToString());
            sec = new Security(log);

            Start();
        }

        void Start()
        {
            byte[] bytes = new byte[129];
            byte[] code = new byte[1];

            while (client.Connected && clientStream.Read(code, 0, 1) != 0)
            {
                clientStream.Read(bytes, 0, bytes.Length - 1);
                string[] data = Encoding.ASCII.GetString(bytes.ToArray()).Split('!');

                switch (code[0]) {

                    case (0): Send(sec.Login(data[0], data[1]));
                              break; //Login    {(login)!(password)!}


                    case (1): Send(sec.AddUser(data[0], data[1], data[2], Convert.ToInt32(data[3])));
                              break; //Add user    {(login)!(salt)!(password)!(securityGroup)!}     !admin only!


                    case (2): Send(sec.DeleteUser(data[0]));
                              break; //Delete user      {(login)!}      !admin only!


                    case (3): break; //Generate keys      {}  !isn't suported this version!

                    case (4): break; //Validate hash
                }
            }

            log.ClientLeft();

            clientStream.Close();
            client.Close();
        }

        private void Send(byte data)
        {
            clientStream.Write(new byte[1] { data }, 0, 1);
        }
    }
}
