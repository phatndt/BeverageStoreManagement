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
    class AccountDAL : Connection, BaseDAL
    {
        private static AccountDAL instance;

        public static AccountDAL Instance
        {
            get { 
                if (instance == null) instance = new AccountDAL(); 
                return AccountDAL.instance;
            }
            private set { AccountDAL.instance = value; }
        }

        public object GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Account WHERE idAccount != 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<Account> accounts = new List<Account>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool isDelete = (bool)dt.Rows[i].ItemArray[4];

                    Account account = new Account(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        dt.Rows[i].ItemArray[2].ToString(),
                        dt.Rows[i].ItemArray[3].ToString(),
                        isDelete);
                    accounts.Add(account);
                }
                return accounts;
            } catch(Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<Account>();
            } finally
            {
                CloseConnection();
            }
        }

        public List<int> getListIdAccount() 
        { 
            
            try
            {
                OpenConnection();
                string queryStr = "SELECT idAccount FROM Account where idAccount != 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<int> ids = new List<int>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int id = int.Parse(dt.Rows[i].ItemArray[0].ToString());
                    ids.Add(id);
                }
                return ids;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<int>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public Account getAccountByID(int idAccount)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Account WHERE idAccount = @idAccount";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idAccount", idAccount);
                SqlDataReader dataReader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                bool isDelete = (bool)dt.Rows[0].ItemArray[4];

                return new Account(
                    int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[0].ItemArray[1].ToString()),
                        dt.Rows[0].ItemArray[2].ToString(),
                        dt.Rows[0].ItemArray[3].ToString(),
                        isDelete
                    );
            } catch
            {
                return new Account();
            } finally
            {
                CloseConnection();
            }
        }

        public void deleteAccountById(int idAccount)
        {
            try
            {
                OpenConnection();
                string queryStr = "DELETE FROM Account WHERE idAccount = @idAccount";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idAccount", idAccount);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
            } catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetMaxIdAccount()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idAccount) FROM Account";
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

        public int AddNewAccount(Account account)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO Account VALUES (@idAccount, @idEmployee, @username, @password, '0')";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idAccount", account.IdAccount);
                command.Parameters.AddWithValue("@idEmployee", account.IdEmployee);
                command.Parameters.AddWithValue("@username", account.Username);
                command.Parameters.AddWithValue("@password", account.Password);

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

        public int UpdateAccount(Account account)
        {
            try
            {
                OpenConnection();

                string queryStr = "UPDATE Account SET idEmployee=@idEmployee,username=@username,password=@password WHERE idAccount=@idAccount";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idAccount", account.IdAccount);
                command.Parameters.AddWithValue("@idEmployee", account.IdEmployee);
                command.Parameters.AddWithValue("@username", account.Username);
                command.Parameters.AddWithValue("@password", account.Password);

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

        public Account GetAccount(string username, string password)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Account WHERE username=@username AND password=@password AND isDelete=0 AND idAccount != 0";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                SqlDataReader dataReader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                bool isDelete = (bool)dt.Rows[0].ItemArray[4];

                return new Account(
                       int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                       int.Parse(dt.Rows[0].ItemArray[1].ToString()),
                       dt.Rows[0].ItemArray[2].ToString(),
                       dt.Rows[0].ItemArray[3].ToString(),
                       isDelete);
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString() + "Man");
                return new Account();
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
