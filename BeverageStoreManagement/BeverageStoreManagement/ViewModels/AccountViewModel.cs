using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Validations;
using BeverageStoreManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BeverageStoreManagement.ViewModels
{
    class AccountViewModel : BaseViewModel
    {
        private MainWindow mainWindow;

        public ICommand LoadAccountCommand { get; set; }

        public ICommand saveAccountCommand { get; set; }

        public ICommand deleteAccountCommand { get; set; }

        public ICommand saveUpdateInfoCommand { get; set; }

        public ICommand closeUpdateInfoCommand { get; set; }

        public ICommand openUpdateInfoCommand { get; set; }

        public AccountViewModel()
        {
            LoadAccountCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => loadAccount(parameter));
            saveAccountCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => saveAccount(parameter));
            deleteAccountCommand = new RelayCommand<AccountControl2>((parameter) => true, (parameter) => DeleteAccount(parameter));
            saveUpdateInfoCommand = new RelayCommand<ChangeAccountInfoWindow>((parameter) => true, (parameter) => saveUpdateInfo(parameter));
            closeUpdateInfoCommand = new RelayCommand<ChangeAccountInfoWindow>((parameter) => true, (parameter) => parameter.Close());
            openUpdateInfoCommand = new RelayCommand<AccountControl2>((parameter) => true, (parameter) => openUpdateInfoWindow(parameter));
        }

        #region Account control
        private void DeleteAccount(AccountControl2 parameter)
        {
            MessageBoxResult messageBoxResult = CustomMessageBox.ShowYesNo("You are going to delete this account!", "Are you sure?", "Yes", "No", MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    int idAccount = int.Parse(parameter.txtIdAccount.Text);
                    AccountDAL.Instance.deleteAccountById(idAccount);
                    Notification.Instance.Success("Delete Account Success");
                    loadAccount(mainWindow);
                } catch(Exception e)
                {
                    CustomMessageBox.Show("Unable to delete account, please come back later!");
                    //CustomMessageBox.Show(e.ToString());
                }
            }
        }
        #endregion

        #region grdAccount
        private void loadAccount(MainWindow parameter)
        {
            this.mainWindow = parameter;
            parameter.stkAccount.Children.Clear();
            List<Account> accounts = (List<Account>)AccountDAL.Instance.GetList();

            int id = 1;
            string shortenedName = "";

            List<int> idList = getUncreatedIdEmployeeList();
            parameter.cbbIDEmployee.ItemsSource = idList;

            Employee employee = new Employee();
            string position = "";

            foreach (Account account in accounts)
            {
                employee = EmployeeDAL.Instance.GetEmployeeById(account.IdEmployee);
                switch (employee.IdPosition)
                {
                    case 1:
                        position = "Manager";
                        break;
                    case 2:
                        position = "Bartender";
                        break;
                    case 3:
                        position = "Employee";
                        break;
                }

                AccountControl2 accountControl = new AccountControl2();

                shortenedName = account.Username[0].ToString().ToUpper();
                accountControl.txb_username.Text = account.Username;
                accountControl.txb_shorted_name.Content = shortenedName;
                accountControl.txtIdAccount.Text = account.IdAccount.ToString();
                accountControl.lblPosition.Content = position;
                parameter.stkAccount.Children.Add(accountControl);

                id++;
            }
        }

         private List<int> getUncreatedIdEmployeeList()
        {
            List<int> idAccountList = new List<int>();
            idAccountList = AccountDAL.Instance.getListIdAccount();

            List<int> idEmployeeList = new List<int>();
            idEmployeeList = EmployeeDAL.Instance.getIDEmployeeList();

            for (int i = 0; i < idAccountList.Count; i++)
            {
                for (int j = 0; j < idEmployeeList.Count; j++)
                {
                    if(idAccountList[i] == idEmployeeList[j])
                    {
                        idEmployeeList.RemoveAt(j);
                    }
                }
            }
            return idEmployeeList;
        }
        #endregion

        #region Add account function
        private bool checkValidAddAccount(MainWindow parameter)
        {
            if(parameter.cbbIDEmployee.Text == "")
            {
                CustomMessageBox.Show("Please choose ID of an employee!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.cbbIDEmployee.Focus();
                return false;
            }
            if (parameter.txtUsername.Text == "")
            {
                CustomMessageBox.Show("Please enter username!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtUsername.Focus();
                return false;
            }
            if (parameter.txtPassword.Text == "")
            {
                CustomMessageBox.Show("Please enter password!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPassword.Focus();
                return false;
            }
            return true;
        }

        private void saveAccount(MainWindow parameter)
        {
            if (checkValidAddAccount(parameter))
            {
                int idAccount = AccountDAL.Instance.GetMaxIdAccount() + 1;
                int idEmployee = int.Parse(parameter.cbbIDEmployee.Text);

                Account account = new Account(
                    idAccount,
                    idEmployee,
                    parameter.txtUsername.Text,
                    MD5Hash(parameter.txtPassword.Text),
                    false);
                if (AccountDAL.Instance.AddNewAccount(account) == 1)
                {
                    Notification.Instance.Success("Add Account Success!");
                    loadAccount(mainWindow);
                    parameter.txtFullName.Clear();
                    parameter.txtUsername.Clear();
                    parameter.txtPassword.Clear();
                    parameter.cbbIDEmployee.SelectedIndex = -1;
                }
                else
                {
                    Notification.Instance.Failed("Add Account Failed!");
                    parameter.txtPassword.Clear();
                }
            }
        }

        public void setFullNameToTXTField(object sender, SelectionChangedEventArgs e)
        {
            if (mainWindow.cbbIDEmployee.SelectedItem == null)
            {
                mainWindow.txtFullName.Text = "Unknown user";
            }
            else
            {
                Employee employee = EmployeeDAL.Instance.GetEmployeeById(int.Parse(mainWindow.cbbIDEmployee.SelectedItem.ToString()));
                mainWindow.txtFullName.Text = employee.Name;
            }
        }
        #endregion

        #region update account info
        private void openUpdateInfoWindow(AccountControl2 accountControl)
        {
            int idAccount = int.Parse(accountControl.txtIdAccount.Text);
            Account account = AccountDAL.Instance.getAccountByID(idAccount);

            ChangeAccountInfoWindow changeAccountInfoWindow = new ChangeAccountInfoWindow();
            changeAccountInfoWindow.txtUpdateIDEmployee.Text = account.IdEmployee.ToString();
            changeAccountInfoWindow.txtUpdateUsername.Text = account.Username;
            changeAccountInfoWindow.txtIdAccount.Text = account.IdAccount.ToString();
            changeAccountInfoWindow.txtUpdatePassword.Clear();

            changeAccountInfoWindow.ShowDialog();
        }

        private bool checkValidUpdateAccount(ChangeAccountInfoWindow parameter)
        {
            if(parameter.txtUpdateIDEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter ID of employee!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtUpdateIDEmployee.Focus();
                return false;
            }
            if (parameter.txtUpdateUsername.Text == "")
            {
                CustomMessageBox.Show("Please enter user name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtUpdateUsername.Focus();
                return false;
            }
            if (parameter.txtUpdatePassword.Text == "")
            {
                CustomMessageBox.Show("Please enter password!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtUpdatePassword.Focus();
                return false;
            }
            return true;
        }

        private void saveUpdateInfo(ChangeAccountInfoWindow parameter)
        {
            if (checkValidUpdateAccount(parameter))
            {
                int idAccount = int.Parse(parameter.txtIdAccount.Text);
                int idEmployee = int.Parse(parameter.txtUpdateIDEmployee.Text);
                string name = parameter.txtUpdateUsername.Text;
                string password = MD5Hash(parameter.txtUpdatePassword.Text);

                Account account = new Account(
                    idAccount, idEmployee, name, password, false
                    );

                if(AccountDAL.Instance.UpdateAccount(account) == 1)
                {
                    Notification.Instance.Success("Update Account Success!");
                    parameter.Close();
                    loadAccount(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Update Account Failed!");
                    parameter.txtUpdateIDEmployee.Clear();
                    parameter.txtUpdateUsername.Clear();
                    parameter.txtUpdatePassword.Clear();
                }
            }
        }
        #endregion
    }
}
