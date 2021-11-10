using BeverageStoreManagement.Views.Employee;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    class EmployeeViewModel:BaseViewModel
    {
        //AddEmployeeWindow
        private string name = null;
        public ICommand openAddEmployeeWindowCommand { get; set; }
        public ICommand saveAddEmployeeCommand { get; set; }
        public string Name { get => name; set => name = value; }

        public EmployeeViewModel()
        {
            openAddEmployeeWindowCommand = new RelayCommand<EmployeePage>((parameter) => true, (parameter) => openAddEmployeeWindow(parameter));
            saveAddEmployeeCommand = new RelayCommand<AddEmployeeWindow>((parameter) => true, (parameter) => saveAddEmployee(parameter));
        }

        private void saveAddEmployee(AddEmployeeWindow parameter)
        {
            if(parameter.txtNameEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNameEmployee.Focus();
                return;
            }
            if (parameter.txtDateEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter date!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateEmployee.Focus();
                return;
            }
            if(parameter.txtDateStartWorkEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter date start work!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateStartWorkEmployee.Focus();
                return;
            }
            if (parameter.txtGenderEmployee.Text == "")
            {
                CustomMessageBox.Show("TPlease enter gender!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtGenderEmployee.Focus();
                return;
            }
            if (parameter.txtPositionEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter position!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPositionEmployee.Focus();
                return;
            }
        }

        private void openAddEmployeeWindow(EmployeePage parameter)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.txtIdEmployee.Text = "1";
            addEmployeeWindow.ShowDialog();
        }
    }
}
