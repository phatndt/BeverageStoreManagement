using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.Print;
using BeverageStoreManagement.Resources.Print.UserControls;
using BeverageStoreManagement.Resources.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    class InvoiceViewModel : BaseViewModel
    {
        private MainWindow mainWindow;
        private List<Employee> employees = new List<Employee>();
        //grd
        public ICommand LoadInvoiceCommand { get; set; }

        //InvoiceControl
        public ICommand SeeDetailInvoiceCommand { get; set; }
        public ICommand PrintInvoiceCommand { get; set; }
        public ICommand DeleteInvoiceCommand { get; set; }

        public InvoiceViewModel()
        {
            LoadInvoiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => LoadInvoice(parameter));

            SeeDetailInvoiceCommand = new RelayCommand<InvoiceControl>((parameter) => true, (parameter) => SeeDetailInvoice(parameter));
            PrintInvoiceCommand = new RelayCommand<InvoiceControl>((parameter) => true, (parameter) => PrintInvoice(parameter));
            DeleteInvoiceCommand = new RelayCommand<InvoiceControl>((parameter) => true, (parameter) => DeleteInvoice(parameter));
        }

        #region Grid
        private void LoadInvoice(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.stkInvoice.Children.Clear();
            employees.Clear();
            List<Invoice> invoices = InvoiceDAL.Instance.GetList();
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
                invoiceControl.txb_status.Text = invoice.Status ? "On Spot"  : "Take Away";
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
            } else
            {
                CustomMessageBox.Show("Can't open window. Try again!");
            }

        }

        private void PrintInvoice(InvoiceControl parameter)
        {
            throw new NotImplementedException();
        }

        private void DeleteInvoice(InvoiceControl parameter)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
