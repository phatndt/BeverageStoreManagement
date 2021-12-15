using BeverageStoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.DAL
{
    class AccountDAL : Connection
    {
        private static AccountDAL instance;

        public static AccountDAL Instance
        {
            get { if (instance == null) instance = new AccountDAL(); return AccountDAL.instance; }
            private set { AccountDAL.instance = value; }
        }

        public Account GetAccount(string username, string password)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Account WHERE username=@username AND password=@password AND isDelete=0 AND idAccount != 0";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                SqlDataReader dataReader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                bool isDelete = (bool)dt.Rows[0].ItemArray[4];

                return new Account(
                       int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                       int.Parse(dt.Rows[0].ItemArray[1].ToString()),
                       dt.Rows[0].ItemArray[2].ToString(),
                       dt.Rows[0].ItemArray[3].ToString(),
                       isDelete);
            }
            catch (Exception e)
            {
                //CustomMessageBox.Show(e.ToString());
                return new Account();
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
