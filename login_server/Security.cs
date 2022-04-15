using System.Security.Cryptography;
using System.Text;

namespace login_server
{
    class Security
    {
        protected static readonly string pepper = "f8c1add3";
        private readonly SQLControl Sql;
        private readonly Logger log;
        private int securityGroup;
        private string userLogin;

        public Security(Logger log)
        {
            Sql = new SQLControl();
            this.log = log;
        }

        private static string GetSHA256(string input)
        {
            if (input == "") { return ""; }

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hashBytes = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public byte Login(string login, string password)
        {
            if (!Sql.RequestDataByLogin(login)) { return 0; }

            var userData = Sql.GetData();
            userLogin = userData.login;
            securityGroup = userData.securityGroup;

            if (userData.hashedPassword != GetSHA256(password + userData.salt + pepper)) { return 0; }
            log.UserLoggedIn(login);

            if (userData.securityGroup == 100) { return 2; }
            return 1;
        }

        public byte LogOut()
        {
            securityGroup = 0;
            log.UserLoggedOut(userLogin);
            return 0;
        }

        public byte AddUser(string login, string salt, string password, int privelegeGroup)
        {
            if (securityGroup != 100) { return 0; } //Check for enough privelege
            if (Sql.RequestDataByLogin(login)) { return 1; } //Check for user existing
            if (login == "" || salt == "" || password == "") { return 1; }

            SQLControl.AddUser(login, salt, GetSHA256(password + salt + pepper), privelegeGroup);
            log.UserAdded(login);
            return 2;
        }

        public byte DeleteUser(string login)
        {
            if (securityGroup != 100) { return 0; } //Check for enough privelege
            if (!Sql.RequestDataByLogin(login)) { return 1; } //Check for user existing

            SQLControl.DeleteUser(login);
            log.UserDeleted(login);
            return 2;
        }

        public void GenerateKeys()
        {

        }
    }
}
