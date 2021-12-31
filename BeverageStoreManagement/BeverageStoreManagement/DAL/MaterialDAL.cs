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
    class MaterialDAL : Connection, BaseDAL
    {
        private static MaterialDAL instance;

        public static MaterialDAL Instance
        {
            get { if (instance == null) instance = new MaterialDAL(); return MaterialDAL.instance; }
            private set { MaterialDAL.instance = value; }
        }
        public object GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Material WHERE idMaterial != 0 AND isDelelte = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<Material> materials = new List<Material>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool status = (bool)dt.Rows[i].ItemArray[7];
                    bool isDelete = (bool)dt.Rows[i].ItemArray[8];

                    Material material = new Material(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        dt.Rows[i].ItemArray[1].ToString(),
                        dt.Rows[i].ItemArray[2].ToString(),
                        dt.Rows[i].ItemArray[3].ToString(),
                        double.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        double.Parse(dt.Rows[i].ItemArray[5].ToString()),
                        Convert.FromBase64String(dt.Rows[i].ItemArray[6].ToString()),
                        status,
                        isDelete);

                    materials.Add(material);
                }

                return materials;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<Material>();
            }
            finally
            {
                CloseConnection();
            }
        }
        public int GetMaxId()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select max(idMaterial) as id from Material";
                SqlCommand command = new SqlCommand(queryString, conn);

                SqlDataReader rdr = command.ExecuteReader();
                rdr.Read();
                res = int.Parse(rdr["id"].ToString());
                return res;
            }
            catch
            {
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public Material GetMaterial(string idMaterial) 
        {
            try
            {
                OpenConnection();
                string queryString = "select * from Material where Material.idMaterial = " + "'" + idMaterial + "'";

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                Material res = new Material(int.Parse(idMaterial), dataTable.Rows[0].ItemArray[1].ToString(),
                    dataTable.Rows[0].ItemArray[2].ToString(), 
                    dataTable.Rows[0].ItemArray[3].ToString(), 
                    double.Parse(dataTable.Rows[0].ItemArray[4].ToString()),
                     double.Parse(dataTable.Rows[0].ItemArray[5].ToString()),
                    Convert.FromBase64String(dataTable.Rows[0].ItemArray[6].ToString()), 
                    (bool)dataTable.Rows[0].ItemArray[7],
                     (bool)dataTable.Rows[0].ItemArray[8]);
                return res;
            }
            catch
            {
                return new Material();
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool AddIntoDB(Material material)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into Material values (@idMaterial, @nameMaterial, @type, @countUnit, @quantity, @purchasePrice, @image, @status, 0)";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idMaterial", material.IdMaterial.ToString());
                command.Parameters.AddWithValue("@nameMaterial", material.NameMaterial);
                command.Parameters.AddWithValue("@type", material.Type);
                command.Parameters.AddWithValue("@countUnit", material.CountUnit);
                command.Parameters.AddWithValue("@quantity", material.Quantity.ToString());
                command.Parameters.AddWithValue("@purchasePrice", material.PurchasePrice.ToString());
                command.Parameters.AddWithValue("@status", material.Status.ToString());
                command.Parameters.AddWithValue("@image", Convert.ToBase64String(material.Image));
                int rs = command.ExecuteNonQuery();
                return true;
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
        public bool DeleteMaterial(string idMaterial)
        {
            try
            {
                OpenConnection();
                string queryString = "Update Material Set IsDelelte = '1' Where idMaterial = " + idMaterial;
                SqlCommand command = new SqlCommand(queryString, conn);
                int rs = command.ExecuteNonQuery();
                return true;
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
