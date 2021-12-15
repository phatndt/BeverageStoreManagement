using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Account
    {
        private int idAccount;
        private int idEmployee;
        private string username;
        private string password;
        private bool isDelete;
        public int IdAccount { get => idAccount; set => idAccount = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public int IdEmployee { get => idEmployee; set => idEmployee = value; }

        public Account() { }

        public Account(int idAccount,int idEmployee, string username, string password, bool isDelete) {
            this.idAccount = idAccount;
            this.idEmployee = idEmployee;
            this.username = username;
            this.password = password;
            this.isDelete = isDelete;
        }
    }
}
