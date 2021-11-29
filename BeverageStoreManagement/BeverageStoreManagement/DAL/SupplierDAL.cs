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
    class SupplierDAL : Connection
    {
        private static SupplierDAL instance;

        public static SupplierDAL Instance
        {
            get { if (instance == null) instance = new SupplierDAL(); return SupplierDAL.instance; }
            private set { SupplierDAL.instance = value; }
        }

        public List<Supplier> GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Supplier WHERE idSupplier != 0 AND isDelete = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<Supplier> suppliers = new List<Supplier>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool isDelete = (bool)dt.Rows[i].ItemArray[4];

                    Supplier supplier = new Supplier(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        dt.Rows[i].ItemArray[1].ToString(),
                        dt.Rows[i].ItemArray[2].ToString(),
                        dt.Rows[i].ItemArray[3].ToString(),
                        isDelete);

                    suppliers.Add(supplier);
                }

                return suppliers;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<Supplier>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public Supplier GetSupplierById(int idSupplier)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Supplier WHERE idSupplier = @idSupplier";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idSupplier", idSupplier);
                SqlDataReader dataReader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                bool isDelete = (bool)dt.Rows[0].ItemArray[4];

                return new Supplier(
                    int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                    dt.Rows[0].ItemArray[1].ToString(),
                    dt.Rows[0].ItemArray[2].ToString(),
                    dt.Rows[0].ItemArray[3].ToString(),
                    isDelete);
                
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new Supplier();
            }
            finally
            {
                CloseConnection();
            }
        }

        public int AddNewSupplier(Supplier supplier)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO Supplier VALUES (@idSupplier,@name,@address,@phone,'0')";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idSupplier", supplier.IdSupplier);
                command.Parameters.AddWithValue("@name", supplier.NameSupplier);
                command.Parameters.AddWithValue("@address", supplier.AddressSupplier);
                command.Parameters.AddWithValue("@phone", supplier.PhoneSupplier);

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

        public int GetMaxIdSupplier()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idSupplier) FROM Supplier";
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

        public int UpdateSupplier(Supplier supplier)
        {
            try
            {
                OpenConnection();

                string queryStr = "UPDATE Supplier SET nameSupplier=@nameSupplier,addressSupplier=@addressSupplier,phoneSupplier=@phoneSupplier WHERE idSupplier=@idSupplier";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@nameSupplier", supplier.NameSupplier);
                command.Parameters.AddWithValue("@addressSupplier", supplier.AddressSupplier);
                command.Parameters.AddWithValue("@phoneSupplier", supplier.PhoneSupplier);
                command.Parameters.AddWithValue("@idSupplier", supplier.IdSupplier);

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

        public int GetTotalSupplier()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT COUNT(*) FROM Supplier";
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
