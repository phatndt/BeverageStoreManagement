using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement
{
    public static class CurrentAccount
    {
        private static int idAccount;
        private static int idEmployee;
        private static string username;
        private static string password;
        private static bool isDelete;

        public static int IdAccount { get => idAccount; set => idAccount = value; }
        public static int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public static string Username { get => username; set => username = value; }
        public static string Password { get => password; set => password = value; }
        public static bool IsDelete { get => isDelete; set => isDelete = value; }
    }
}
