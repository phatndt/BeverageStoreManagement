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
    class InvoiceInfoDAL : Connection
    {
        private static InvoiceInfoDAL instance;

        public static InvoiceInfoDAL Instance
        {
            get { if (instance == null) instance = new InvoiceInfoDAL(); return InvoiceInfoDAL.instance; }
            private set { InvoiceInfoDAL.instance = value; }
        }

        public int GetMaxIdInvoiceInfo()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idInvoiceInfo) FROM InvoiceInfo";
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

        public int AddNewInvoiceInvoiceInfo(InvoiceInfo invoiceInfo)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO InvoiceInfo VALUES (@idInvoiceInfo,@idInvoice,@idProduct,@quantity,@price,@intoMoney)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idInvoiceInfo", invoiceInfo.IdInvoiceInfo);
                command.Parameters.AddWithValue("@idInvoice", invoiceInfo.IdInvoice);
                command.Parameters.AddWithValue("@idProduct", invoiceInfo.IdGood);
                command.Parameters.AddWithValue("@quantity", invoiceInfo.Quantity);
                command.Parameters.AddWithValue("@price", invoiceInfo.Price);
                command.Parameters.AddWithValue("@intoMoney", invoiceInfo.IntoMoney);

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

        public List<InvoiceInfo> GetList(int idInvoice)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM InvoiceInfo WHERE idInvoice=@idInvoice";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@idInvoice", idInvoice);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<InvoiceInfo> invoiceInfos = new List<InvoiceInfo>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    InvoiceInfo invoiceInfo = new InvoiceInfo(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[3].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[5].ToString()));

                    invoiceInfos.Add(invoiceInfo);
                }

                return invoiceInfos;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<InvoiceInfo>();
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
