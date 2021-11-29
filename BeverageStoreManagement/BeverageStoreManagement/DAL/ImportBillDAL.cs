using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.DAL
{
    class ImportBillDAL : Connection
    {
        private static ImportBillDAL instance;

        public static ImportBillDAL Instance
        {
            get { if (instance == null) instance = new ImportBillDAL(); return ImportBillDAL.instance; }
            private set { ImportBillDAL.instance = value; }
        }

        public int GetBillQuantityBySupplier(int idSupplier)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT COUNT(*) FROM ImportBill Where idSupplier=@idSupplier";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idSupplier", idSupplier);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                return int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch
            {
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetTotalMoneyBySupplier(int idSupplier)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM ImportBill Where idSupplier=@idSupplier";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idSupplier", idSupplier);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows[0].ItemArray[0].ToString() == "NULL")
                {
                    return -9999;
                }
                return int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch
            {
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetTotalMoney()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM ImportBill";
                SqlCommand command = new SqlCommand(queryStr, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows[0].ItemArray[0].ToString() == "NULL")
                {
                    return -9999;
                }
                return int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch
            {
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
