using System;
using System.Collections.Generic;
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
    }
}
