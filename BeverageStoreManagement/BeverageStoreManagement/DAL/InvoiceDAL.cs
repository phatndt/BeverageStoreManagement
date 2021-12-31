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
    class InvoiceDAL : Connection
    {
        private static InvoiceDAL instance;

        public static InvoiceDAL Instance
        {
            get { if (instance == null) instance = new InvoiceDAL(); return InvoiceDAL.instance; }
            private set { InvoiceDAL.instance = value; }
        }

        public int GetMaxIdInvoice()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idInvoice) FROM Invoice";
                SqlCommand command = new SqlCommand(queryStr, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                CustomMessageBox.Show(dataTable.Rows[0].ItemArray[0].ToString());
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

        public int AddNewInvoice(Invoice invoice)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO Invoice VALUES (@idInvoice,@idEmployee,@date,@totalMoney,@moneyCustomer,@tableNumber,@status,'0')";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idInvoice", invoice.IdInvoice);
                command.Parameters.AddWithValue("@idEmployee", invoice.IdEmployee);
                command.Parameters.AddWithValue("@date", invoice.Date);
                command.Parameters.AddWithValue("@totalMoney", invoice.TotalMoney);
                command.Parameters.AddWithValue("@moneyCustomer", invoice.MoneyCustomer);
                command.Parameters.AddWithValue("@tableNumber", invoice.TableNumber);
                command.Parameters.AddWithValue("@status", invoice.Status);

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

        public List<Invoice> GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Invoice WHERE idInvoice != 0 AND isDelete = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<Invoice> invoices = new List<Invoice>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool status = (bool)dt.Rows[i].ItemArray[6];
                    bool isDelete = (bool)dt.Rows[i].ItemArray[7];

                    Invoice invoice = new Invoice(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        DateTime.Parse(dt.Rows[i].ItemArray[2].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[3].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[5].ToString()),
                        status,
                        isDelete);

                    invoices.Add(invoice);
                }

                return invoices;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<Invoice>();
            }
            finally
            {
                CloseConnection();
            }
        }
    }

}
