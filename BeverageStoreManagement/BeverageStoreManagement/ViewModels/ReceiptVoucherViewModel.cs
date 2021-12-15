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
    class ReceiptVoucherViewModel : BaseViewModel
    {
        private MainWindow mainWindow;

        private string totalMoney;
        public string TotalMoney { get => totalMoney; set => totalMoney = value; }

        public ICommand SeparateThousandsCommand { get; set; }

        //grdReceiptVoucher
        public ICommand OpenAddReceiptVoucherWindowCommand { get; set; }

        public ICommand LoadReceiptVoucherCommand { get; set; }

        //addNewReceiptVoucher
        public ICommand GetListEmployeeCommand { get; set; }
        public ICommand SaveAddReceiptVoucherCommand { get; set; }
        public ICommand ExitAddReceiptVoucherCommand { get; set; }

        private ObservableCollection<string> itemSourceEmployee = new ObservableCollection<string>();
        public ObservableCollection<string> ItemSourceEmployee { get => itemSourceEmployee; set { itemSourceEmployee = value; OnPropertyChanged(); } }

        private ObservableCollection<string> itemSourceImportBill = new ObservableCollection<string>();
        public ObservableCollection<string> ItemSourceImportBill { get => itemSourceImportBill; set { itemSourceImportBill = value; OnPropertyChanged(); } }

        //ReceiptVoucherControl
        public ICommand DeleteReceiptVoucherCommand { get; set; }
        public ReceiptVoucherViewModel()
        {
            LoadReceiptVoucherCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadReceiptVoucher(parameter));
            OpenAddReceiptVoucherWindowCommand = new RelayCommand<ReceiptVoucherPage>((parameter) => true, (parameter) => OpenReceiptVoucherWindow(parameter));

            GetListEmployeeCommand = new RelayCommand<AddReceiptVoucherWindow>(parameter => true, parameter => GetListEmployee(parameter));
            SaveAddReceiptVoucherCommand = new RelayCommand<AddReceiptVoucherWindow>(parameter => true, parameter => SaveAddReceiptVoucher(parameter));
            ExitAddReceiptVoucherCommand = new RelayCommand<AddReceiptVoucherWindow>(parameter => true, parameter => parameter.Close());

            DeleteReceiptVoucherCommand = new RelayCommand<ReceiptVoucherControl>(parameter => true, parameter => DeleteReceiptVoucher(parameter));

            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));
        }

        #region grdReceiptVoucher
        private void LoadReceiptVoucher(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.stkReceiptVoucher.Children.Clear();
            List<ReceiptVoucher> receiptVouchers = ReceiptVoucherDAL.Instance.GetList();

            int id = 1;
            foreach (ReceiptVoucher receiptVoucher in receiptVouchers)
            {
                Employee employee = EmployeeDAL.Instance.GetEmployeeById(receiptVoucher.IdEmployee);
                ReceiptVoucherControl receiptVoucherControl = new ReceiptVoucherControl();

                receiptVoucherControl.id.Text = receiptVoucher.IdReceiptVoucher.ToString();
                receiptVoucherControl.no.Text = id.ToString();
                receiptVoucherControl.txb_receiptVoucher_name.Text = employee.Name;
                receiptVoucherControl.txb_receiptVoucher_time.Text = receiptVoucher.Date.ToString("dd/MM/yyyy");
                receiptVoucherControl.txb_receiptVoucher_money.Text = Converter.Instance.SeparateThousands(receiptVoucher.TotalMoney.ToString());
                receiptVoucherControl.txb_receiptVoucher_note.Text = receiptVoucher.Note;
                parameter.stkReceiptVoucher.Children.Add(receiptVoucherControl);

                id++;
            }
        }
        private void OpenReceiptVoucherWindow(ReceiptVoucherPage parameter)
        {
            int idReceiptVoucher = ReceiptVoucherDAL.Instance.GetMaxIdReceiptVoucher() + 1;
            if (idReceiptVoucher != 0)
            {
                AddReceiptVoucherWindow addReceiptVoucherWindow = new AddReceiptVoucherWindow();
                addReceiptVoucherWindow.txtId.Text = idReceiptVoucher.ToString();
                addReceiptVoucherWindow.txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                addReceiptVoucherWindow.txtIdEmployee.Text = CurrentAccount.IdEmployee.ToString();
                addReceiptVoucherWindow.txtNameEmployee.Text = CurrentEmployee.Name;
                addReceiptVoucherWindow.ShowDialog();
            }
            else
            {
                Notification.Instance.Failed("Connected to database failed!!!");
            }
        }
        #endregion

        #region addNewReceiptVoucher
        public void GetListEmployee(AddReceiptVoucherWindow parameter)
        {
            itemSourceEmployee.Clear();
            List<Employee> employees = (List<Employee>)EmployeeDAL.Instance.GetList();
            foreach (Employee employee in employees)
            {
                itemSourceEmployee.Add(employee.Name);
            }
        }

        private bool CheckEmptyAddReceiptVoucher(AddReceiptVoucherWindow parameter)
        {
            if (parameter.txtNameEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter name employee!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNameEmployee.Focus();
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

        public void SaveAddReceiptVoucher(AddReceiptVoucherWindow parameter)
        {
            if (CheckEmptyAddReceiptVoucher(parameter))
            {
                int idReceiptVoucher = int.Parse(parameter.txtId.Text);
                int idEmployee = int.Parse(parameter.txtIdEmployee.Text);

                ReceiptVoucher receiptVoucher = new ReceiptVoucher(
                       idReceiptVoucher,
                       idEmployee,
                       DateTime.Parse(parameter.txtDate.Text),
                       double.Parse(parameter.txtTotalMoney.Text),
                       parameter.txtNote.Text,
                       false);
                if (ReceiptVoucherDAL.Instance.AddNewReceiptVoucher(receiptVoucher) == 1)
                {
                    Notification.Instance.Success("Add Receipt Voucher Success!");
                    parameter.Close();
                    LoadReceiptVoucher(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Add Receipt Voucher Failed!");
                    parameter.Close();
                }
            }
        }
        #endregion

        #region ReceiptVoucherControl
        public void DeleteReceiptVoucher(ReceiptVoucherControl parameter)
        {
            MessageBoxResult messageBoxResult = CustomMessageBox.ShowYesNo("Confirm delelte receipt voucher!", "Information", "Yes", "No", MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                int idReceiptVoucher = int.Parse(parameter.id.Text);
                ReceiptVoucherDAL.Instance.DeleteReceiptVoucherById(idReceiptVoucher);
                Notification.Instance.Success("Delete Receipt Voucher Success");
                LoadReceiptVoucher(mainWindow);
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
