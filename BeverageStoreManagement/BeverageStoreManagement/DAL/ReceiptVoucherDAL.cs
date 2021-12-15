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
    class ReceiptVoucherDAL : Connection
    {
        private static ReceiptVoucherDAL instance;

        public static ReceiptVoucherDAL Instance
        {
            get { if (instance == null) instance = new ReceiptVoucherDAL(); return ReceiptVoucherDAL.instance; }
            private set { ReceiptVoucherDAL.instance = value; }
        }

        public List<ReceiptVoucher> GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM ReceiptVoucher WHERE idReceiptVoucher != 0 AND isDelete = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<ReceiptVoucher> receiptVouchers = new List<ReceiptVoucher>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool isDelete = (bool)dt.Rows[i].ItemArray[5];

                    ReceiptVoucher receiptVoucher = new ReceiptVoucher(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        DateTime.Parse(dt.Rows[i].ItemArray[2].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[3].ToString()),
                        dt.Rows[i].ItemArray[4].ToString(),
                        isDelete);

                    receiptVouchers.Add(receiptVoucher);
                }

                return receiptVouchers;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<ReceiptVoucher>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public int AddNewReceiptVoucher(ReceiptVoucher receiptVoucher)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO ReceiptVoucher VALUES (@idReceiptVoucher,@idEmployee,@date,@totalMoney,@note,'0')";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idReceiptVoucher", receiptVoucher.IdReceiptVoucher);
                command.Parameters.AddWithValue("@idEmployee", receiptVoucher.IdEmployee);
                command.Parameters.AddWithValue("@date", receiptVoucher.Date);
                command.Parameters.AddWithValue("@totalMoney", receiptVoucher.TotalMoney);
                command.Parameters.AddWithValue("@note", receiptVoucher.Note);

                int result = command.ExecuteNonQuery();

                return result;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetMaxIdReceiptVoucher()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idReceiptVoucher) FROM ReceiptVoucher";
                SqlCommand command = new SqlCommand(queryStr, conn);
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
        public void DeleteReceiptVoucherById(int idReceiptVoucher)
        {
            try
            {
                OpenConnection();
                string queryStr = "UPDATE ReceiptVoucher SET isDelete = '1' WHERE idReceiptVoucher = @idReceiptVoucher";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idReceiptVoucher", idReceiptVoucher);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                //return int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                //return 0;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
