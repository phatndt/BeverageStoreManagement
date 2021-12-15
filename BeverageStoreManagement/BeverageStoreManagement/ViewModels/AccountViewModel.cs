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

        public AccountViewModel()
        {
            OnClickLoginCommand = new RelayCommand<LoginWindow>((parameter) => true, (parameter) => onClickLogin(parameter));
        }

        private void onClickLogin(LoginWindow parameter)
        {
            string username = parameter.txtUsername.Text;
            string password = Converter.Instance.MD5Hash(parameter.txtPassword.Password.Trim());

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
                    CurrentEmployee.IdEmployee = employee.IdEmployee;
                    CurrentEmployee.IdPosition = employee.IdPosition;
                    CurrentEmployee.DateStartWorking = employee.DateStartWorking;
                    CurrentEmployee.PhoneNumber = employee.PhoneNumber;
                    CurrentEmployee.IsDelete = employee.IsDelete;
                    CurrentEmployee.Name = employee.Name;
                    CurrentEmployee.DateOfBirth = employee.DateOfBirth;
                    CurrentEmployee.Address = employee.Address;
                    CurrentEmployee.Gender = employee.Gender;
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
