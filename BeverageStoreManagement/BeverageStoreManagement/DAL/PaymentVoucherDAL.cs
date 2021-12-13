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
    class PaymentVoucherDAL : Connection
    {
        private static PaymentVoucherDAL instance;

        public static PaymentVoucherDAL Instance
        {
            get { if (instance == null) instance = new PaymentVoucherDAL(); return PaymentVoucherDAL.instance; }
            private set { PaymentVoucherDAL.instance = value; }
        }
        public List<PaymentVoucher> GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM PaymentVoucher WHERE idPaymentVoucher != 0 AND isDelete = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<PaymentVoucher> paymentVouchers = new List<PaymentVoucher>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool isDelete = (bool)dt.Rows[i].ItemArray[6];

                    PaymentVoucher paymentVoucher = new PaymentVoucher(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()),
                        DateTime.Parse(dt.Rows[i].ItemArray[3].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        dt.Rows[i].ItemArray[5].ToString(),
                        isDelete);

                    paymentVouchers.Add(paymentVoucher);
                }

                return paymentVouchers;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<PaymentVoucher>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public int AddNewPaymentVoucher(PaymentVoucher paymentVoucher)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO PaymentVoucher VALUES (@idPaymentVoucher,@idEmployee,@idImportBill,@date,@totalMoney,@note,'0')";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idPaymentVoucher", paymentVoucher.IdPaymentVoucher);
                command.Parameters.AddWithValue("@idEmployee", paymentVoucher.IdEmployee);
                command.Parameters.AddWithValue("@idImportBill", paymentVoucher.IdImportBill);
                command.Parameters.AddWithValue("@date", paymentVoucher.Date);
                command.Parameters.AddWithValue("@totalMoney", paymentVoucher.TotalMoney);
                command.Parameters.AddWithValue("@note", paymentVoucher.Note);

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
        public bool AddPaymentVoucher(int a, int b, int c, DateTime d, double e, string f)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into PaymentVoucher(idPaymentVoucher, idEmployee, idImportBill, datePaymentVoucher, totalMoney, note, isDelete) values(@idPaymentVoucher, @idEmployee, @idImportBill, @datePaymentVoucher, @totalMoney, @note, @isDelete)";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idPaymentVoucher", a);
                command.Parameters.AddWithValue("@idEmployee", b);
                command.Parameters.AddWithValue("@idImportBill", c);
                command.Parameters.AddWithValue("@datePaymentVoucher", d);
                command.Parameters.AddWithValue("@totalMoney", e);
                command.Parameters.AddWithValue("@note", f);
                command.Parameters.AddWithValue("@isDelete", 0);

                int rs = command.ExecuteNonQuery();
                if (rs != 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetMaxIdPaymentVoucher()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idPaymentVoucher) FROM PaymentVoucher";
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
    }
}
