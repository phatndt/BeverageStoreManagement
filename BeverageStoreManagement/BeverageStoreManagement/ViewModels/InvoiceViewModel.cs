using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.Print;
using BeverageStoreManagement.Resources.Print.UserControls;
using BeverageStoreManagement.Resources.UserControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace BeverageStoreManagement.ViewModels
{
    class InvoiceViewModel : BaseViewModel
    {
        private MainWindow mainWindow;
        private List<Employee> employees = new List<Employee>();
        private List<Invoice> invoices = new List<Invoice>();
        //grd
        public ICommand LoadInvoiceCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        //InvoiceControl
        public ICommand SeeDetailInvoiceCommand { get; set; }
        public ICommand PrintInvoiceCommand { get; set; }
        public ICommand PrintInvoiceControlCommand { get; set; }
        public ICommand DeleteInvoiceCommand { get; set; }

        public InvoiceViewModel()
        {
            LoadInvoiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => LoadInvoice(parameter));
            ExportCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => Export(parameter));

            SeeDetailInvoiceCommand = new RelayCommand<InvoiceControl>((parameter) => true, (parameter) => SeeDetailInvoice(parameter));
            PrintInvoiceCommand = new RelayCommand<InvoicePrint>((parameter) => true, (parameter) => PrintInvoice(parameter));
            DeleteInvoiceCommand = new RelayCommand<InvoiceControl>((parameter) => true, (parameter) => DeleteInvoice(parameter));
            PrintInvoiceControlCommand = new RelayCommand<InvoiceControl>((parameter) => true, (parameter) => PrintInvoiceControl(parameter));
        }

        #region Grid
        public void LoadInvoice(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.stkInvoice.Children.Clear();
            employees.Clear();
            List<Invoice> invoices = InvoiceDAL.Instance.GetList();
            this.invoices = invoices;
            employees = (List<Employee>)EmployeeDAL.Instance.GetList();
            int id = 1;
            foreach (Invoice invoice in invoices)
            {
                InvoiceControl invoiceControl = new InvoiceControl();

                invoiceControl.id.Text = invoice.IdInvoice.ToString();
                invoiceControl.no.Text = id.ToString();
                invoiceControl.txb_eployee_name.Text = getNameEmployee(invoice.IdEmployee);
                invoiceControl.txb_time.Text = invoice.Date.ToString("dd/MM/yyyy");
                invoiceControl.txb_money.Text = SeparateThousands(invoice.TotalMoney.ToString());
                invoiceControl.txb_money_give_by_customer.Text = SeparateThousands(invoice.MoneyCustomer.ToString());
                invoiceControl.txb_status.Text = invoice.Status ? "On Spot" : "Take Away";
                parameter.stkInvoice.Children.Add(invoiceControl);

                id++;
            }
        }

        private string getNameEmployee(int idEmployee)
        {
            foreach (Employee employee in employees)
            {
                if (employee.IdEmployee == idEmployee)
                {
                    return employee.Name;
                }
            }
            return "ERROR";
        }

        private void Export(MainWindow parameter)
        {
            SaveFileDialog x = new SaveFileDialog();

            if (x.ShowDialog() == true)
            {
                GenerateExcel(ConvertToDataTable(invoices), x.FileName);
            }
        }

        private DataTable ConvertToDataTable<T>(List<T> models)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static void GenerateExcel(DataTable dataTable, string path)
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            Excel._Worksheet xlWorksheet = excelWorkBook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            foreach (DataTable table in dataSet.Tables)
            {
                Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = table.TableName;

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
            }
            // excelWorkBook.Save(); -> this will save to its default location
            excelWorkBook.SaveAs(path); // -> this will do the custom
            excelWorkBook.Close();
            excelApp.Quit();
        }
        #endregion

        #region InvoiceControl
        private void SeeDetailInvoice(InvoiceControl parameter)
        {
            int idInvoice = int.Parse(parameter.id.Text);
            List<InvoiceInfo> invoiceInfos = InvoiceInfoDAL.Instance.GetList(idInvoice);
            if (invoiceInfos.Count != 0)
            {
                InvoicePrint invoicePrint = new InvoicePrint();
                invoicePrint.txbIdBill.Text = parameter.id.Text;
                invoicePrint.txbInvoiceDate.Text = parameter.txb_time.Text;
                invoicePrint.txbEmployeeName.Text = parameter.txb_eployee_name.Text;
                invoicePrint.txbEmployeeName.Text = parameter.txb_money.Text;

                int id = 1;
                foreach (InvoiceInfo invoiceInfo in invoiceInfos)
                {
                    InvoicePrintControl invoicePrintControl = new InvoicePrintControl();
                    invoicePrintControl.txbOrderNum.Text = id.ToString();
                    invoicePrintControl.txbName.Text = ProductDAL.Instance.GetNameProductById(invoiceInfo.IdGood);
                    invoicePrintControl.txbUnitPrice.Text = invoiceInfo.Price.ToString();
                    invoicePrintControl.txbQuantity.Text = invoiceInfo.Quantity.ToString();
                    invoicePrintControl.txbUnit.Text = "None";
                    invoicePrintControl.txbTotal.Text = SeparateThousands(invoiceInfo.IntoMoney.ToString());
                    invoicePrint.stkBillInfo.Children.Add(invoicePrintControl);

                    id++;
                }
                invoicePrint.ShowDialog();
            }
            else
            {
                CustomMessageBox.Show("Can't open window. Try again!");
            }
        }

        private void PrintInvoice(InvoicePrint parameter)
        {
            PrintDialog printDiaLog = new PrintDialog();

            if (printDiaLog.ShowDialog() == true)
            {
                printDiaLog.PrintVisual(parameter, "Invoice");
            }
        }

        private void PrintInvoiceControl(InvoiceControl parameter)
        {
            int idInvoice = int.Parse(parameter.id.Text);
            List<InvoiceInfo> invoiceInfos = InvoiceInfoDAL.Instance.GetList(idInvoice);
            if (invoiceInfos.Count != 0)
            {
                InvoicePrint invoicePrint = new InvoicePrint();
                invoicePrint.txbIdBill.Text = parameter.id.Text;
                invoicePrint.txbInvoiceDate.Text = parameter.txb_time.Text;
                invoicePrint.txbEmployeeName.Text = parameter.txb_eployee_name.Text;
                invoicePrint.txbEmployeeName.Text = parameter.txb_money.Text;

                int id = 1;
                foreach (InvoiceInfo invoiceInfo in invoiceInfos)
                {
                    InvoicePrintControl invoicePrintControl = new InvoicePrintControl();
                    invoicePrintControl.txbOrderNum.Text = id.ToString();
                    invoicePrintControl.txbName.Text = ProductDAL.Instance.GetNameProductById(invoiceInfo.IdGood);
                    invoicePrintControl.txbUnitPrice.Text = invoiceInfo.Price.ToString();
                    invoicePrintControl.txbQuantity.Text = invoiceInfo.Quantity.ToString();
                    invoicePrintControl.txbUnit.Text = "None";
                    invoicePrintControl.txbTotal.Text = SeparateThousands(invoiceInfo.IntoMoney.ToString());
                    invoicePrint.stkBillInfo.Children.Add(invoicePrintControl);

                    id++;
                }
                PrintDialog printDiaLog = new PrintDialog();

                if (printDiaLog.ShowDialog() == true)
                {
                    printDiaLog.PrintVisual(invoicePrint, "Invoice");
                }
            }
            else
            {
                CustomMessageBox.Show("Can't print invoice. Try again!");
            }
        }

        private void DeleteInvoice(InvoiceControl parameter)
        {
            MessageBoxResult messageBoxResult = CustomMessageBox.ShowYesNo("Confirm delelte invoice!", "Information", "Yes", "No", MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int idInvoice = int.Parse(parameter.id.Text);
                InvoiceDAL.Instance.DeleteInvoiceById(idInvoice);
                Notification.Instance.Success("Delete Invoice Success!");
                LoadInvoice(mainWindow);
            }
        }
        #endregion
    }
}
