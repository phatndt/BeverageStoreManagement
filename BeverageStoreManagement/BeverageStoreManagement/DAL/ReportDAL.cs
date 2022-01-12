using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.DAL
{
    class ReportDAL : Connection
    {
        private static ReportDAL instance;

        public static ReportDAL Instance
        {
            get { if (instance == null) instance = new ReportDAL(); return ReportDAL.instance; }
            private set { ReportDAL.instance = value; }
        }

        public long GetTotalMoneyOfSaleInMonth(string month, string year)
        {
            long res = 0;
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM Invoice WHERE MONTH(date)=@month AND YEAR(date)=@year";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception e)
            {
                //CustomMessageBox.Show(e.Message);
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }

        public long GetNumberInMonth(string month, string year)
        {
            long res = 0;
            try
            {
                OpenConnection();
                string queryStr = "SELECT COUNT(*) FROM Invoice WHERE MONTH(date)=@month AND YEAR(date)=@year";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return int.Parse(dataTable.Rows[0].ItemArray[0].ToString());
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

        #region PieChart
        public ChartValues<long> GetTotalMoneyOfInvoiceInDayPieChart(string day, string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM Invoice WHERE MONTH(date)=@month AND YEAR(date)=@year AND DAY(date)=@day";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@day", day);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(long.Parse(rdr[0].ToString()));
                }
                return res;
            }
            catch
            {
                res.Add(0);
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> GetTotalMoneyOfPaymentVoucherInDayPieChart(string day, string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM PaymentVoucher WHERE MONTH(date)=@month AND YEAR(date)=@year AND DAY(date)=@day";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@day", day);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(long.Parse(rdr[0].ToString()));
                }
                return res;
            }
            catch
            {
                res.Add(0);
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> GetTotalMoneyOfReceiptVoucherInDayPieChart(string day, string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM ReceiptVoucher WHERE MONTH(date)=@month AND YEAR(date)=@year AND DAY(date)=@day";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@day", day);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(long.Parse(rdr[0].ToString()));
                }
                return res;
            }
            catch
            {
                res.Add(0);
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> GetTotalMoneyOfInvoiceInMonthPieChart(string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM Invoice WHERE MONTH(date)=@month AND YEAR(date)=@year";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(long.Parse(rdr[0].ToString()));
                }
                return res;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                res.Add(0);
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> GetTotalMoneyOfPaymentVoucherInMonthPieChart(string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM PaymentVoucher WHERE MONTH(date)=@month AND YEAR(date)=@year";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(long.Parse(rdr[0].ToString()));
                }
                return res;
            }
            catch
            {
                res.Add(0);
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> GetTotalMoneyOfReceiptVoucherInMonthPieChart(string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                OpenConnection();
                string queryStr = "SELECT SUM(totalMoney) FROM ReceiptVoucher WHERE MONTH(date)=@month AND YEAR(date)=@year";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(long.Parse(rdr[0].ToString()));
                }
                return res;
            }
            catch
            {
                res.Add(0);
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion

        #region Chart
        public string[] QueryDayInMonth(string month, string year)
        {
            List<string> res = new List<string>();
            try
            {
                OpenConnection();
                string queryStr = "select day(date) as day from Invoice where month(date)=@month and year(date)=@year group by day(date) union select day(date) as day from PaymentVoucher where month(date)=@month and year(date)=@year group by day(date) union select day(date) as day from ReceiptVoucher where month(date)=@month and year(date)=@year group by day(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(rdr["day"].ToString());
                }
                return res.ToArray();
            }
            catch
            {
                return res.ToArray();
            }
            finally
            {
                CloseConnection();
            }
        }
        public string[] QueryQuarterInYear(string year)
        {
            List<string> res = new List<string>();
            try
            {
                OpenConnection();
                string queryStr = "select datepart(quarter, date) as quarter from Invoice where year(date)=@year group by datepart(quarter, date) union select datepart(quarter, date) as quarter from PaymentVoucher where year(date)=@year group by datepart(quarter, date) union select datepart(quarter, date) as quarter from ReceiptVoucher where year(date)=@year group by datepart(quarter, date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(rdr["quarter"].ToString());
                }
                return res.ToArray();
            }
            catch
            {
                return res.ToArray();
            }
            finally
            {
                CloseConnection();
            }
        }
        public string[] QueryMonthInYear(string year)
        {
            List<string> res = new List<string>();
            try
            {
                OpenConnection();
                string queryStr = "select month(date) as month from Invoice where year(date)=@year group by month(date) union select month(date) as month from PaymentVoucher where year(date)=@year group by month(date) union select month(date) as month from ReceiptVoucher where year(date)=@year group by month(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    res.Add(rdr["month"].ToString());
                }
                return res.ToArray();
            }
            catch
            {
                return res.ToArray();
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion

        public ChartValues<long> QueryMoneyInvoiceByMonth(string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] daysOfMonth = ReportDAL.Instance.QueryDayInMonth(month, year);

                OpenConnection();
                string queryStr = "select day(date), sum(totalMoney) from Invoice where month(date)=@month and year(date)=@year group by day(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[daysOfMonth.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;
                for (int i = 0; i < daysOfMonth.Length && j < numOfRows; i++)
                {
                    if (daysOfMonth[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
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
        public ChartValues<long> QueryMoneyPaymentByMonth(string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] daysOfMonth = ReportDAL.Instance.QueryDayInMonth(month, year);

                OpenConnection();
                string queryStr = "select day(date), sum(totalMoney) from PaymentVoucher where month(date)=@month and year(date)=@year group by day(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[daysOfMonth.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;
                for (int i = 0; i < daysOfMonth.Length && j < numOfRows; i++)
                {
                    if (daysOfMonth[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
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
        public ChartValues<long> QueryMoneyReceiptInByMonth(string month, string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] daysOfMonth = ReportDAL.Instance.QueryDayInMonth(month, year);

                OpenConnection();
                string queryStr = "select day(date), sum(totalMoney) from ReceiptVoucher where month(date)=@month and year(date)=@year group by day(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[daysOfMonth.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;
                for (int i = 0; i < daysOfMonth.Length && j < numOfRows; i++)
                {
                    if (daysOfMonth[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
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

        #region Quarter
        public ChartValues<long> QueryMoneyInvoiceByQuarter(string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] quartersOfYear = ReportDAL.Instance.QueryQuarterInYear(year);

                OpenConnection();
                string queryStr = "select datepart(quarter, date), sum(totalMoney) from Invoice where year(date)=@year group by datepart(quarter, date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[quartersOfYear.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;

                for (int i = 0; i < quartersOfYear.Length && j < numOfRows; i++)
                {
                    if (quartersOfYear[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
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
        public ChartValues<long> QueryMoneyPaymentByQuarter(string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] quartersOfYear = ReportDAL.Instance.QueryQuarterInYear(year);

                OpenConnection();
                string queryStr = "select datepart(quarter, date), sum(totalMoney) from PaymentVoucher where year(date)=@year group by datepart(quarter, date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[quartersOfYear.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;

                for (int i = 0; i < quartersOfYear.Length && j < numOfRows; i++)
                {
                    if (quartersOfYear[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
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
        public ChartValues<long> QueryMoneyReceiptByQuarter(string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] quartersOfYear = ReportDAL.Instance.QueryQuarterInYear(year);

                OpenConnection();
                string queryStr = "select datepart(quarter, date), sum(totalMoney) from ReceiptVoucher where year(date)=@year group by datepart(quarter, date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[quartersOfYear.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;

                for (int i = 0; i < quartersOfYear.Length && j < numOfRows; i++)
                {
                    if (quartersOfYear[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
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
        #endregion

        #region Year
        public ChartValues<long> QueryMoneyInvoiceByYear(string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] monthsOfYear = ReportDAL.Instance.QueryMonthInYear(year);

                OpenConnection();
                string queryStr = "select month(date), sum(totalMoney) from Invoice where year(date)=@year group by month(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[monthsOfYear.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;

                for (int i = 0; i < monthsOfYear.Length && j < numOfRows; i++)
                {
                    if (monthsOfYear[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
                return res;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> QueryMoneyPaymentByYear(string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] monthsOfYear = ReportDAL.Instance.QueryMonthInYear(year);

                OpenConnection();
                string queryStr = "select month(date), sum(totalMoney) from PaymentVoucher where year(date)=@year group by month(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[monthsOfYear.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;

                for (int i = 0; i < monthsOfYear.Length && j < numOfRows; i++)
                {
                    if (monthsOfYear[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
                return res;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        public ChartValues<long> QueryMoneyReceiptByYear(string year)
        {
            ChartValues<long> res = new ChartValues<long>();
            try
            {
                string[] monthsOfYear = ReportDAL.Instance.QueryMonthInYear(year);

                OpenConnection();
                string queryStr = "select month(date), sum(totalMoney) from ReceiptVoucher where year(date)=@year group by month(date)";
                SqlCommand command = new SqlCommand(queryStr, conn);
                command.Parameters.AddWithValue("@year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                long[] revenue = new long[monthsOfYear.Length];
                int j = 0;
                int numOfRows = dataTable.Rows.Count;

                for (int i = 0; i < monthsOfYear.Length && j < numOfRows; i++)
                {
                    if (monthsOfYear[i] == dataTable.Rows[j].ItemArray[0].ToString())
                    {
                        revenue[i] = long.Parse(dataTable.Rows[j].ItemArray[1].ToString());
                        j++;
                    }
                }
                res = new ChartValues<long>(revenue);
                return res;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show(e.ToString());
                return res;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion
    }
}
