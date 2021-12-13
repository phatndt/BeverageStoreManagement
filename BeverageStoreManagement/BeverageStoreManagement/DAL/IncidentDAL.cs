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
    class IncidentDAL:Connection
    {
        private static IncidentDAL instance;

        public static IncidentDAL Instance
        {
            get { if (instance == null) instance = new IncidentDAL(); return IncidentDAL.instance; }
            private set { IncidentDAL.instance = value; }
        }

        public List<Incident> GetList()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Incident WHERE idIncident != 0 AND isDelete = 0";
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                List<Incident> Incidents = new List<Incident>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool isDelete = (bool)dt.Rows[i].ItemArray[7];
                    bool status = (bool)dt.Rows[i].ItemArray[4];
                    bool pay = (bool)dt.Rows[i].ItemArray[5];

                    Incident Incident = new Incident(
                        int.Parse(dt.Rows[i].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[i].ItemArray[1].ToString()),
                        DateTime.Parse(dt.Rows[i].ItemArray[2].ToString()),
                        dt.Rows[i].ItemArray[3].ToString(),
                        status,
                        pay,
                        double.Parse(dt.Rows[i].ItemArray[6].ToString()),
                        isDelete);

                    Incidents.Add(Incident);
                }

                return Incidents;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new List<Incident>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public Incident GetIncidentById(int idIncident)
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT * FROM Incident WHERE idIncident = @idIncident";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idIncident", idIncident);
                SqlDataReader dataReader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                bool isDelete = (bool)dt.Rows[0].ItemArray[7];
                bool status = (bool)dt.Rows[0].ItemArray[4];
                bool pay = (bool)dt.Rows[0].ItemArray[5];

                return new Incident(
                     int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[0].ItemArray[1].ToString()),
                        DateTime.Parse(dt.Rows[0].ItemArray[2].ToString()),
                        dt.Rows[0].ItemArray[3].ToString(),
                        status,
                        pay,
                        double.Parse(dt.Rows[0].ItemArray[6].ToString()),
                        isDelete);

            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return new Incident();
            }
            finally
            {
                CloseConnection();
            }
        }

        public int AddNewIncident(Incident Incident)
        {
            try
            {
                OpenConnection();
                string queryStr = "INSERT INTO Incident VALUES (@idIncident,@idEmployee,@date,@description,@status,@pay,@total,'0')";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@idIncident", Incident.IdIncident);
                command.Parameters.AddWithValue("@idEmployee", Incident.IdEmployee);
                command.Parameters.AddWithValue("@date", Incident.Date);
                command.Parameters.AddWithValue("@description", Incident.Description);
                command.Parameters.AddWithValue("@status", Incident.Status);
                command.Parameters.AddWithValue("@pay", Incident.Pay);
                command.Parameters.AddWithValue("@total", Incident.TotalMoney);

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

        public int GetMaxIdIncident()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT MAX(idIncident) FROM Incident";
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

        public int GetTotalIncident()
        {
            try
            {
                OpenConnection();
                string queryStr = "SELECT COUNT(*) FROM Incident";
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
