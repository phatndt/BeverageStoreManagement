using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.DAL
{
    class PositionDAL : Connection
    {
        private static PositionDAL instance;

        public static PositionDAL Instance
        {
            get { if (instance == null) instance = new PositionDAL(); return PositionDAL.instance; }
            private set { PositionDAL.instance = value; }
        }

        public string GetNamePositionById(int idPosition)
        {
            try
            {
                OpenConnection();
                string query = "SELECT namePosition FROM Position Where idPosition = @idPosition";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@idPosition", idPosition);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable.Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "No Position";
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
