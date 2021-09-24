using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeverageStoreManagement.DAL
{
    public class Connection
    {
        private string strConn;
        public SqlConnection conn;
        public Connection()
        {
            try
            {
                strConn = ConfigurationManager.ConnectionStrings["BSM"].ConnectionString;
            }
            catch
            {
                MessageBox.Show("Mất kết nối đến cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            conn = new SqlConnection(strConn);
        }
        public void OpenConnection()
        {
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["BSM"].ConnectionString;
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mất kết nối đến cơ sở dữ liệu!!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }
        public void CloseConnection()
        {
            conn.Close();
        }
    }
}
