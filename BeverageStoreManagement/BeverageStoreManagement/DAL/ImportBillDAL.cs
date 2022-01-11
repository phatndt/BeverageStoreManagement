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
    class ImportBillDAL : Connection
    {
        private static ImportBillDAL instance;

        public static ImportBillDAL Instance
        {
            get { if (instance == null) instance = new ImportBillDAL(); return ImportBillDAL.instance; }
            private set { ImportBillDAL.instance = value; }
        }

        public object GetListPending()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM ImportBill WHERE idImportBill != 0 AND status = 'true'";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<ImportBill> importBills = new List<ImportBill>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool status = (bool)dt.Rows[i].ItemArray[6];
                    bool isDelete = (bool)dt.Rows[i].ItemArray[7];

                    ImportBill importBill = new ImportBill(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()),
                        DateTime.Parse( dt.Rows[i].ItemArray[3].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        dt.Rows[i].ItemArray[5].ToString(),
                        status,
                        isDelete);

                    importBills.Add(importBill);
                }

                return importBills;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<ImportBill>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public object GetListImported()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM ImportBill WHERE idImportBill != 0 AND status = 'false'";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<ImportBill> importBills = new List<ImportBill>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool status = (bool)dt.Rows[i].ItemArray[6];
                    bool isDelete = (bool)dt.Rows[i].ItemArray[7];

                    ImportBill importBill = new ImportBill(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[2].ToString()),
                        DateTime.Parse(dt.Rows[i].ItemArray[3].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        dt.Rows[i].ItemArray[5].ToString(),
                        status,
                        isDelete);

                    importBills.Add(importBill);
                }

                return importBills;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<ImportBill>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetMaxIdImportBill()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idImportBill) FROM ImportBill";
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

        public bool AddImportBillInfoIntoDB(ImportBillInfo importBillInfo)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into ImportBillInfo values (@idImportBillInfo, @idImportBill, @idMaterial, @quantity, @unit, @price, @intoMoney)";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idImportBillInfo", importBillInfo.IdImportBillInfo);
                command.Parameters.AddWithValue("@idImportBill", importBillInfo.IdImportBill);
                command.Parameters.AddWithValue("@idMaterial", importBillInfo.IdMaterial);
                command.Parameters.AddWithValue("@quantity", importBillInfo.Quantity);
                command.Parameters.AddWithValue("@unit", importBillInfo.Unit);
                command.Parameters.AddWithValue("@price", importBillInfo.Price);
                command.Parameters.AddWithValue("@intoMoney", importBillInfo.IntoMoney);
                int rs = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AddImportBillIntoDB(ImportBill importBill)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into ImportBill values (@idImportBill, @idEmployee, @idSupplier, @date, @totalMoney, @note, @status, @isDelete)";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idImportBill", importBill.IdImportBill);
                command.Parameters.AddWithValue("@idEmployee", importBill.IdEmployee);
                command.Parameters.AddWithValue("@idSupplier", importBill.IdSupplier);
                command.Parameters.AddWithValue("@date", importBill.Date);
                command.Parameters.AddWithValue("@totalMoney", importBill.TotalMoney);
                command.Parameters.AddWithValue("@note", importBill.Note);
                command.Parameters.AddWithValue("@status", importBill.Status);
                command.Parameters.AddWithValue("@isDelete", importBill.IsDelete);
                int rs = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public ImportBill GetImportBill(string idImportBill)
        {
            try
            {
                OpenConnection();
                string queryString = "select * from ImportBill where ImportBill.idImportBill = " + "'" + idImportBill + "'";

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                ImportBill res = new ImportBill(int.Parse(idImportBill), 
                    int.Parse(dataTable.Rows[0].ItemArray[1].ToString()),
                    int.Parse(dataTable.Rows[0].ItemArray[2].ToString()),
                    DateTime.Parse( dataTable.Rows[0].ItemArray[3].ToString()),
                    double.Parse(dataTable.Rows[0].ItemArray[4].ToString()),
                     dataTable.Rows[0].ItemArray[5].ToString(),
                    (bool)dataTable.Rows[0].ItemArray[6],
                     (bool)dataTable.Rows[0].ItemArray[7]);
                return res;
            }
            catch
            {
                return new ImportBill();
            }
            finally
            {
                CloseConnection();
            }
        }

        public object GetImportInfoBill(string idImportBill)
        {
            try
            {
                OpenConnection();
                string queryStr = "select * from ImportBillInfo where ImportBillInfo.idImportBill = " + "'" + idImportBill + "'";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                List<ImportBillInfo> importBillInfos = new List<ImportBillInfo>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ImportBillInfo res = new ImportBillInfo(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()),
                    int.Parse(idImportBill),
                    int.Parse(dataTable.Rows[i].ItemArray[2].ToString()),
                    int.Parse(dataTable.Rows[i].ItemArray[3].ToString()),
                    dataTable.Rows[i].ItemArray[4].ToString(),
                     double.Parse(dataTable.Rows[i].ItemArray[5].ToString()),
                    double.Parse(dataTable.Rows[i].ItemArray[6].ToString()));

                    importBillInfos.Add(res);
                }

                return importBillInfos;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<ImportBillInfo>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool UpdateStatus(string idImportBill)
        {
            try
            {
                OpenConnection();
                string queryString = "update ImportBill set status= 'false' where idImportBill= '" + idImportBill + "'";
                SqlCommand command = new SqlCommand(queryString, conn);
                int rs = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
