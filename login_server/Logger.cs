using System;

namespace login_server
{
    class Logger
    {
        // Class for logging some info in console

        private static int clientCount = 0;

        private readonly string localAddress;
        private readonly string localPort;

        private readonly string userAddress;

        public Logger(string localAddress, string localPort)
        {
            this.localAddress = localAddress;
            this.localPort = localPort;

            ServerStarting();
        }

        public Logger(string userAddress)
        {
            this.userAddress = userAddress;

            NewClient();
        }

        private string UserInfo()
        {
            return $"[user: {userAddress}]:";
        }

        public void ServerStarting()
        {
            Console.WriteLine($"[Starting server]: Address = {localAddress}   Port = {localPort}");
        }

        public void ServerShuttedDown()
        {
            Console.WriteLine($"[Shutting down server]: Address = {localAddress}   Port = {localPort}");
        }

        public static void Error(Exception e)
        {
            Console.WriteLine($"[Error]: {e}");
        }

        public void UserLoggedIn(string login)
        {
            Console.WriteLine(UserInfo() + $"Logged in as {login}");
        }

        public void UserLoggedOut(string login)
        {
            Console.WriteLine(UserInfo() + $"logged out {login}");
        }

        public void UserDeleted(string login)
        {
            Console.WriteLine(UserInfo() + $"Deleted user {login}");
        }

        public void UserAdded(string login)
        {
            Console.WriteLine(UserInfo() + $"Added user {login}");
        }

        public void NewClient()
        {
            clientCount++;
            Console.WriteLine($"[New client]: Client Address= {userAddress}");
            Console.WriteLine($"[State]: Current client count is {clientCount}");
        }

        public void ClientLeft()
        {
            clientCount--;
            Console.WriteLine($"[Client disconnected]: Client Address= {userAddress}");
            Console.WriteLine($"[State]: Current client count is {clientCount}");
        }
    }
}
