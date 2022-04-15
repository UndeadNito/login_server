using MySql.Data.MySqlClient;

namespace login_server
{
    class SQLControl
    {

        protected readonly static string connectionString = "Database=cssoft;Uid=requester;Pwd=pass;";
        private readonly static MySqlConnection connection = new MySqlConnection(connectionString);
        UserDataStruct userData;
        

        public readonly struct UserDataStruct
        {
            // Structure storing user data

            public readonly int id;
            public readonly string login;
            public readonly string salt;
            public readonly string hashedPassword;
            public readonly int securityGroup;

            public UserDataStruct(int id, string login, string salt, string hashedPassword, int securityGroup){
                this.id = id;
                this.login = login;
                this.salt = salt;
                this.hashedPassword = hashedPassword;
                this.securityGroup = securityGroup;
            }

        }

        public static void SQLOpenConnection()
        {
            connection.Open();
        }
        public static void SQLCloseConnection()
        {
            connection.Close();
        }

        private static MySqlDataReader ExecuteCommand(string command)
        {
            // The function executes SQL command snd returns a reader with data

            MySqlCommand SQLcommand = new MySqlCommand(command, connection);

            MySqlDataReader reader = SQLcommand.ExecuteReader();
            return reader;
        }

        public bool RequestDataByLogin(string login)
        {
            // The function gets all user data by login and stores it in /userData/

            MySqlDataReader reader = ExecuteCommand($"select * from user where login = \'{login}\'");
            reader.Read();
            if (!reader.HasRows) { reader.Close(); return false; }

            userData = new UserDataStruct(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4));

            reader.Close();
            return true;
        }

        public ref UserDataStruct GetData()
        {
            return ref userData;
        }

        public static void AddUser(string login, string salt, string password, int privelegeGroup)
        {
            // The function adds new user

            MySqlDataReader reader = ExecuteCommand("INSERT INTO user (`login`, `salt`, `password`, `securityGroup`) " +
                                                    $"VALUES (\'{login}\', \'{salt}\', \'{password}\', \'{privelegeGroup}\')");
            reader.Close();
        }

        public static void DeleteUser(string login)
        {
            // The function deletes a user

            MySqlDataReader reader = ExecuteCommand($"delete from user where login = \'{login}\'");
            reader.Close();
        }
    }
}
