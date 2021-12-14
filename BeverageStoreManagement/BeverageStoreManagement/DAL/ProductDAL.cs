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
    class ProductDAL : Connection, BaseDAL
    {

        private static ProductDAL instance;

        public static ProductDAL Instance
        {
            get { if (instance == null) instance = new ProductDAL(); return ProductDAL.instance; }
            private set { ProductDAL.instance = value; }
        }
        public object GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Product WHERE idProduct != 0 AND isDelelte = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<Product> products = new List<Product>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool status = (bool)dt.Rows[i].ItemArray[5];
                    bool isDelete = (bool)dt.Rows[i].ItemArray[7];

                    Product product = new Product(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        dt.Rows[i].ItemArray[2].ToString(),
                        dt.Rows[i].ItemArray[3].ToString(),
                        int.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        status,
                        Convert.FromBase64String(dt.Rows[i].ItemArray[6].ToString()),
                        isDelete);

                    products.Add(product);
                }

                return products;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<Employee>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<Product> ConvertDBToList()
        {
            List<Product> productList = new List<Product>();
            try
            {
                OpenConnection();
                string queryString = "select * from Product where Product.isDelelte = 0";

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Product acc = new Product(int.Parse(dataTable.Rows[i].ItemArray[0].ToString()), int.Parse(dataTable.Rows[i].ItemArray[1].ToString()),
                        dataTable.Rows[i].ItemArray[2].ToString(), dataTable.Rows[i].ItemArray[3].ToString(),
                        int.Parse(dataTable.Rows[i].ItemArray[4].ToString()), true, Convert.FromBase64String(dataTable.Rows[i].ItemArray[5].ToString()), false);

                    productList.Add(acc);
                }
                return productList;
            }
            catch
            {
                return productList;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Product GetProduct(string idProduct) // lấy thông tin hàng hóa khi biết id 
        {
            try
            {
                OpenConnection();
                string queryString = "select * from SANPHAM where Product.idProduct = " + "'" + idProduct + "'";

                SqlCommand command = new SqlCommand(queryString, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                Product res = new Product(int.Parse(idProduct), int.Parse( dataTable.Rows[0].ItemArray[1].ToString()),
                    dataTable.Rows[2].ItemArray[1].ToString(),"", int.Parse(dataTable.Rows[0].ItemArray[4].ToString()), (bool)dataTable.Rows[0].ItemArray[5],
                    Convert.FromBase64String(dataTable.Rows[0].ItemArray[6].ToString()), (bool)dataTable.Rows[0].ItemArray[7]);
                return res;
            }
            catch
            {
                return new Product();
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool IsExistProductName(string productName)
        {
            try
            {
                OpenConnection();
                string query = "select * from Product where isDelelte = 0 and nameProduct = " + "'" + productName + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count == 0)
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
                return true;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AddIntoDB(Product product)
        {
            try
            {
                OpenConnection();
                string queryString = "insert into Product (idProduct, idProductType, nameProduct, description, price, status, image, isDelelte) " +
                    "values(@idProduct, @idProductType, @nameProduct, @description, @price, @status, @image, @isDelelte)";
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@idProduct", product.IdProduct.ToString());
                command.Parameters.AddWithValue("@idProductType", product.IdProductType.ToString());
                command.Parameters.AddWithValue("@nameProduct", product.NameProduct);
                command.Parameters.AddWithValue("@description", product.Description);
                command.Parameters.AddWithValue("@price", product.Price.ToString());
                command.Parameters.AddWithValue("@status", product.Status.ToString());
                command.Parameters.AddWithValue("@image", Convert.ToBase64String(product.Image));
                command.Parameters.AddWithValue("@isDelelte", product.IsDelete.ToString());
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
        public bool UpdateOnDB(Product product)
        {
            try
            {
                OpenConnection();
                string queryString = "update Product set nameProduct=@nameProduct,price=@price, image=@image, status=@status " +
                    "where MaSP =" + product.IdProduct.ToString();
                SqlCommand command = new SqlCommand(queryString, conn);
                command.Parameters.AddWithValue("@nameProduct", product.NameProduct);
                command.Parameters.AddWithValue("@price", product.Price.ToString());
                command.Parameters.AddWithValue("@image", Convert.ToBase64String(product.Image));
                command.Parameters.AddWithValue("@status", product.Status.ToString());

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
        public int GetMaxId()
        {
            int res = 0;
            try
            {
                OpenConnection();
                string queryString = "select max(idProduct) as id from Product";
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
        public bool DeleteProduct(string idProduct)
        {
            try
            {
                OpenConnection();
                string queryString = "Update Product Set IsDelelte = '1' Where idProduct = " + idProduct;
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
