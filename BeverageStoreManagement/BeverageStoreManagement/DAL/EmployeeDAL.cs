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
    class EmployeeDAL : Connection, BaseDAL
    {
        private static EmployeeDAL instance;

        public static EmployeeDAL Instance
        {
            get { if (instance == null) instance = new EmployeeDAL(); return EmployeeDAL.instance; }
            private set { EmployeeDAL.instance = value; }
        }

        public object GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Employee WHERE idEmployee != 0 AND isDelete = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<Employee> employees = new List<Employee>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool gender = (bool)dt.Rows[i].ItemArray[7];
                    bool isDelete = (bool)dt.Rows[i].ItemArray[8];

                    Employee employee = new Employee(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        dt.Rows[i].ItemArray[2].ToString(),
                        DateTime.Parse(dt.Rows[i].ItemArray[3].ToString()),
                        DateTime.Parse(dt.Rows[i].ItemArray[4].ToString()),
                        dt.Rows[i].ItemArray[5].ToString(),
                        dt.Rows[i].ItemArray[6].ToString(),
                        gender,
                        isDelete);

                    employees.Add(employee);
                }

                return employees;
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

        public Employee GetEmployeeById(int idEmployee)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Employee WHERE idEmployee = @idEmployee";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idEmployee", idEmployee);
                SqlDataReader dataReader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                bool gender = (bool)dt.Rows[0].ItemArray[7];
                bool isDelete = (bool)dt.Rows[0].ItemArray[8];

                return new Employee(
                       int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                       int.Parse(dt.Rows[0].ItemArray[1].ToString()),
                       dt.Rows[0].ItemArray[2].ToString(),
                       DateTime.Parse(dt.Rows[0].ItemArray[3].ToString()),
                       DateTime.Parse(dt.Rows[0].ItemArray[4].ToString()),
                       dt.Rows[0].ItemArray[5].ToString(),
                       dt.Rows[0].ItemArray[6].ToString(),
                       gender,
                       isDelete);
            }
            catch
            {
                //CustomMessageBox.Show(e.ToString());
                return new Employee();
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DeleteEmployeeById(int idEmployee)
        {
            try
            {
                OpenConnection();
                string queryStr = "UPDATE Employee SET isDelete = '1' WHERE idEmployee = @idEmployee";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idEmployee", idEmployee);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                //return int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch(Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                //return 0;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int GetMaxIdEmployee()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idEmployee) FROM Employee";
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

        public int AddNewEmployee(Employee employee)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO Employee VALUES (@idEmployee,@idPosition,@name,@date,@dateStartWork,@address,@phoneNumber,@gender,'0')";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idEmployee", employee.IdEmployee);
                command.Parameters.AddWithValue("@idPosition", employee.IdPosition);
                command.Parameters.AddWithValue("@name", employee.Name);
                command.Parameters.AddWithValue("@date", employee.DateOfBirth);
                command.Parameters.AddWithValue("@dateStartWork", employee.DateStartWorking);
                command.Parameters.AddWithValue("@address", employee.Address);
                command.Parameters.AddWithValue("@phoneNumber", employee.PhoneNumber);
                command.Parameters.AddWithValue("@gender", employee.Gender);

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

        public int UpdateEmployee(Employee employee)
        {
            try
            {
                OpenConnection();

                int gender = employee.Gender ? 1 : 0;

                string queryStr = "UPDATE Employee SET idPosition=@idPosition,name=@name,dateOfBirth=@date,dateStartWorking=@dateStartWork,address=@address,phoneNumber=@phoneNumber,gender=@gender WHERE idEmployee=@idEmployee";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idPosition", employee.IdPosition);
                command.Parameters.AddWithValue("@name", employee.Name);
                command.Parameters.AddWithValue("@date", employee.DateOfBirth);
                command.Parameters.AddWithValue("@dateStartWork", employee.DateStartWorking);
                command.Parameters.AddWithValue("@address", employee.Address);
                command.Parameters.AddWithValue("@phoneNumber", employee.PhoneNumber);
                command.Parameters.AddWithValue("@gender", gender);
                command.Parameters.AddWithValue("@idEmployee", employee.IdEmployee);

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
    }
}
