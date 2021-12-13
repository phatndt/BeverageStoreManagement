using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    class AccountViewModel : BaseViewModel
    {
        public ICommand OnClickLoginCommand { get; set; }

        private Converter converter = new Converter();
        public AccountViewModel()
        {
            OnClickLoginCommand = new RelayCommand<LoginWindow>((parameter) => true, (parameter) => onClickLogin(parameter));
        }

        private void onClickLogin(LoginWindow parameter)
        {
            string username = parameter.txtUsername.Text;
            string password = converter.MD5Hash(parameter.txtPassword.Password.Trim());

            Account account = AccountDAL.Instance.GetAccount(username, password);

            if (account.IdAccount >= 1)
            {
                Employee employee = EmployeeDAL.Instance.GetEmployeeById(account.IdEmployee);
                MainWindow mainWindow = new MainWindow();
                CurrentAccount.IdAccount = account.IdAccount;
                CurrentAccount.IdEmployee = account.IdEmployee;
                CurrentAccount.Username = account.Username;
                CurrentAccount.Password = account.Password;
                CurrentAccount.IsDelete = account.IsDelete;
                if (employee.IdEmployee >= 1)
                {
                    if (employee.IdPosition == 1)
                    {
                        mainWindow.ShowDialog();
                        parameter.txtUsername.Clear();
                        parameter.txtPassword.Clear();
                    }
                }
            }
        }
    }
}
