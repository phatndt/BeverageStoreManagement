using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    public class PaymentVoucherViewModel : BaseViewModel
    {
        private MainWindow mainWindow;


        private string idImportBill;
        public string IdImportBill { get => idImportBill; set => idImportBill = value; }

        private string totalMoney;
        public string TotalMoney { get => totalMoney; set => totalMoney = value; }

        //grdPaymentVoucherLoadInvoiceCommand
        public ICommand LoadPaymentVoucherCommand { get; set; }
        public ICommand OpenAddPaymentVoucherWindowCommand { get; set; }

        //AddPaymentVoucherWindow
        public ICommand GetListEmployeeCommand { get; set; }
        public ICommand SavePaymentVoucherWindowCommand { get; set; }
        public ICommand ExitAddPaymentVoucherCommand { get; set; }

        private ObservableCollection<string> itemSourceEmployee = new ObservableCollection<string>();
        public ObservableCollection<string> ItemSourceEmployee { get => itemSourceEmployee; set { itemSourceEmployee = value; OnPropertyChanged(); } }

        private ObservableCollection<string> itemSourceImportBill = new ObservableCollection<string>();
        public ObservableCollection<string> ItemSourceImportBill { get => itemSourceImportBill; set { itemSourceImportBill = value; OnPropertyChanged(); } }

        public ICommand SeparateThousandsCommand { get; set; }
        public PaymentVoucherViewModel()
        {
            LoadPaymentVoucherCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => LoadPaymentVoucher(parameter));
            OpenAddPaymentVoucherWindowCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => OpenPaymentVoucherWindow(parameter));

            GetListEmployeeCommand = new RelayCommand<AddPaymentVoucherWindow>(parameter => true, parameter => GetListEmployee(parameter));
            SavePaymentVoucherWindowCommand = new RelayCommand<AddPaymentVoucherWindow>((parameter) => true, (parameter) => SavePaymentVoucherWindow(parameter));
            ExitAddPaymentVoucherCommand = new RelayCommand<AddPaymentVoucherWindow>(parameter => true, parameter => parameter.Close());

            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));
        }
        #region grdPaymentVoucher

        private void LoadPaymentVoucher(MainWindow parameter)
        {
            this.mainWindow = parameter;
            parameter.stkPaymentVoucher.Children.Clear();
            List<PaymentVoucher> paymentVouchers = PaymentVoucherDAL.Instance.GetList();

            int id = 1;
            foreach (PaymentVoucher paymentVoucher in paymentVouchers)
            {
                Employee employee = EmployeeDAL.Instance.GetEmployeeById(paymentVoucher.IdEmployee);
                PaymentVoucherControl paymentVoucherControl = new PaymentVoucherControl();

                paymentVoucherControl.id.Text = paymentVoucher.IdEmployee.ToString();
                paymentVoucherControl.no.Text = id.ToString();
                paymentVoucherControl.txb_paymentVoucher_name.Text = employee.Name;
                paymentVoucherControl.txb_paymentVoucher_time.Text = paymentVoucher.Date.ToString("dd/MM/yyyy");
                paymentVoucherControl.txb_paymentVoucher_money.Text = Converter.Instance.SeparateThousands(paymentVoucher.TotalMoney.ToString());
                paymentVoucherControl.txb_paymentVoucher_note.Text = paymentVoucher.Note;
                paymentVoucherControl.txb_paymentVoucher_import_bill.Text = paymentVoucher.IdImportBill.ToString();
                parameter.stkPaymentVoucher.Children.Add(paymentVoucherControl);

                id++;
            }
        }

        private void OpenPaymentVoucherWindow(MainWindow parameter)
        {
            int idPaymentVoucher = PaymentVoucherDAL.Instance.GetMaxIdPaymentVoucher() + 1;
            if (idPaymentVoucher != 1)
            {
                AddPaymentVoucherWindow addPaymentVoucherWindow = new AddPaymentVoucherWindow();
                addPaymentVoucherWindow.txtId.Text = idPaymentVoucher.ToString();
                addPaymentVoucherWindow.txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                addPaymentVoucherWindow.txtIdEmployee.Text = CurrentAccount.IdEmployee.ToString();
                addPaymentVoucherWindow.txtNameEmployee.Text = CurrentEmployee.Name;
                addPaymentVoucherWindow.ShowDialog();

            }
            else
            {
                Notification.Instance.Failed("Connected to database failed!!!");
            }
        }

        #endregion

        #region AddPaymentVoucherWindow
        public void GetListEmployee(AddPaymentVoucherWindow parameter)
        {
            itemSourceEmployee.Clear();
            List<Employee> employees = (List<Employee>)EmployeeDAL.Instance.GetList();
            foreach (Employee employee in employees)
            {
                itemSourceEmployee.Add(employee.Name);
            }
        }
        private bool CheckEmptyAddPaymentVoucher(AddPaymentVoucherWindow parameter)
        {
            if (parameter.txtNameEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter name employee!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNameEmployee.Focus();
                return false;
            }
            if (parameter.txtImportBill.Text == "" || parameter.txtTotalMoney.Text == "0")
            {
                CustomMessageBox.Show("Please enter import Bill!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtImportBill.Focus();
                return false;
            }
            if (parameter.txtDate.Text == "")
            {
                CustomMessageBox.Show("Please enter date!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDate.Focus();
                return false;
            }
            if (parameter.txtNote.Text == "")
            {
                CustomMessageBox.Show("Please enter note!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNote.Focus();
                return false;
            }
            if (parameter.txtTotalMoney.Text == "" || parameter.txtTotalMoney.Text == "0")
            {
                CustomMessageBox.Show("Please enter total money!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtTotalMoney.Focus();
                return false;
            }
            return true;
        }

        public void SavePaymentVoucherWindow(AddPaymentVoucherWindow parameter)
        {
            if (CheckEmptyAddPaymentVoucher(parameter))
            {
                int idPaymentVoucher = int.Parse(parameter.txtId.Text); 
                int idEmployee = int.Parse(parameter.txtIdEmployee.Text);
                int idImportBill = int.Parse(parameter.txtImportBill.Text);
                PaymentVoucher paymentVoucher = new PaymentVoucher(
                       idPaymentVoucher,
                       idEmployee,
                       idImportBill,
                       DateTime.Parse(parameter.txtDate.Text),
                       double.Parse(parameter.txtTotalMoney.Text),
                       parameter.txtNote.Text,
                       false);
                if (PaymentVoucherDAL.Instance.AddNewPaymentVoucher(paymentVoucher) == 1)
                {
                    Notification.Instance.Success("Add Payment Voucher Success!");
                    parameter.Close();
                    LoadPaymentVoucher(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Add Payment Voucher Failed!");
                    parameter.Close();
                }
            }
        }
        #endregion

        public void SeparateThousands(TextBox txt)
        {
            if (!string.IsNullOrEmpty(txt.Text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(ConvertToNumber(txt.Text).ToString(), System.Globalization.NumberStyles.AllowThousands);
                txt.Text = String.Format(culture, "{0:N0}", valueBefore);
                txt.Select(txt.Text.Length, 0);
            }
        }
    }
}
