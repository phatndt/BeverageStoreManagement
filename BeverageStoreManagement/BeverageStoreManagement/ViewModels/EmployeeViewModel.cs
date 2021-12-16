using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Validations;
using BeverageStoreManagement.Views.Employee;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BeverageStoreManagement.ViewModels
{
    class EmployeeViewModel : BaseViewModel
    {
        //AddEmployeeWindow
        private string name = null;
        public ICommand openAddEmployeeWindowCommand { get; set; }
        public ICommand saveAddEmployeeCommand { get; set; }
        public ICommand ExitAddEmployeeCommand { get; set; }
        public string Name { get => name; set => name = value; }

        //ChangeInformationEmployee
        public ICommand SaveChangeInformationEmployeeCommand { get; set; }
        public ICommand ExitUpdateEmployeeCommand { get; set; }

        //grd 
        public ICommand LoadEmployeeCommand { get; set; }

        //EmployeeControl
        public ICommand OpenChangeInformationEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }

        private MainWindow mainWindow;

        private EmployeeDALValidation employeeDALValidation = new EmployeeDALValidation();
        public EmployeeViewModel()
        {
            openAddEmployeeWindowCommand = new RelayCommand<EmployeePage>((parameter) => true, (parameter) => openAddEmployeeWindow(parameter));
            saveAddEmployeeCommand = new RelayCommand<AddEmployeeWindow>((parameter) => true, (parameter) => SaveAddEmployee(parameter));
            ExitAddEmployeeCommand = new RelayCommand<AddEmployeeWindow>(parameter => true, parameter => parameter.Close());

            SaveChangeInformationEmployeeCommand = new RelayCommand<ChangeEmployeeWindow>((parameter) => true, (parameter) => SaveChangeInformationEmployee(parameter));
            ExitUpdateEmployeeCommand = new RelayCommand<AddEmployeeWindow>(parameter => true, parameter => parameter.Close());

            LoadEmployeeCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => LoadEmployee(parameter));

            OpenChangeInformationEmployeeCommand = new RelayCommand<EmployeeControl>((parameter) => true, (parameter) => OpenChangeInformationEmployee(parameter));
            DeleteEmployeeCommand = new RelayCommand<EmployeeControl>((parameter) => true, (parameter) => DeleteEmployee(parameter));
        }

        #region EmployeeControl
        private void DeleteEmployee(EmployeeControl parameter)
        {
            MessageBoxResult messageBoxResult = CustomMessageBox.ShowYesNo("Confirm delelte employee!", "Information", "Yes", "No", MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                int idEmployee = int.Parse(parameter.id.Text);
                EmployeeDAL.Instance.DeleteEmployeeById(idEmployee); Notification.Instance.Success("Delete Employee Success");
                LoadEmployee(mainWindow);
            }
        }

        private void OpenChangeInformationEmployee(EmployeeControl parameter)
        {
            int idEmployee = int.Parse(parameter.id.Text);

            Employee employee = EmployeeDAL.Instance.GetEmployeeById(idEmployee);
            ChangeEmployeeWindow changeEmployeeWindow = new ChangeEmployeeWindow();

            changeEmployeeWindow.txtIdEmployee.Text = employee.IdEmployee.ToString();
            changeEmployeeWindow.txtNameEmployee.Text = employee.Name;
            changeEmployeeWindow.txtDateEmployee.Text = employee.DateOfBirth.ToString("dd/MM/yyyy");
            changeEmployeeWindow.txtDateStartWorkEmployee.Text = employee.DateStartWorking.ToString("dd/MM/yyyy");
            changeEmployeeWindow.txtAddressEmployee.Text = employee.Address;
            changeEmployeeWindow.txtphoneNumberEmployee.Text = employee.PhoneNumber;
            changeEmployeeWindow.txtGenderEmployee.Text = employee.Gender ? "Male" : "Female";
            changeEmployeeWindow.txtPositionEmployee.Text = PositionDAL.Instance.GetNamePositionById(employee.IdPosition);

            changeEmployeeWindow.ShowDialog();
        }
        #endregion

        #region grdEmployee
        private void LoadEmployee(MainWindow parameter)
        {
            this.mainWindow = parameter;
            parameter.stkEmployee.Children.Clear();
            List<Employee> employees = (List<Employee>)EmployeeDAL.Instance.GetList();

            bool flag = false;
            int id = 1;
            foreach (Employee employee in employees)
            {
                EmployeeControl employeeControl = new EmployeeControl();
                flag = !flag;
                if (flag)
                {
                    employeeControl.grdMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFbbdefb");
                }

                employeeControl.id.Text = employee.IdEmployee.ToString();
                employeeControl.no.Text = id.ToString();
                employeeControl.txb_employee_name.Text = employee.Name;
                employeeControl.txb_position.Text = PositionDAL.Instance.GetNamePositionById(employee.IdEmployee);
                employeeControl.txb_date.Text = employee.DateOfBirth.ToString("dd/MM/yyyy");
                employeeControl.txb_date_start_work.Text = employee.DateStartWorking.ToString("dd/MM/yyyy");
                employeeControl.txb_gender.Text = employee.Gender ? "Male" : "Female";
                parameter.stkEmployee.Children.Add(employeeControl);

                id++;
            }
        }

        private void openAddEmployeeWindow(EmployeePage parameter)
        {
            int idEmployee = EmployeeDAL.Instance.GetMaxIdEmployee() + 1;
            this.name = "";
            if (idEmployee != 0)
            {
                AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
                addEmployeeWindow.txtIdEmployee.Text = idEmployee.ToString();
                addEmployeeWindow.ShowDialog();
            }
            else
            {
                Notification.Instance.Failed("Connected to database failed!!!");
            }
        }
        #endregion

        #region SaveAddEmployee
        private bool CheckEmptyAddEmployee(AddEmployeeWindow parameter)
        {
            if (parameter.txtNameEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNameEmployee.Focus();
                return false;
            }
            if (parameter.txtDateEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter date!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateEmployee.Focus();
                return false;
            }
            if (parameter.txtDateStartWorkEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter date start work!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateStartWorkEmployee.Focus();
                return false;
            }
            if (parameter.txtGenderEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter gender!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtGenderEmployee.Focus();
                return false;
            }
            if (parameter.txtAddressEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter address!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtAddressEmployee.Focus();
                return false;
            }
            if (parameter.txtphoneNumberEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter phone number!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtphoneNumberEmployee.Focus();
                return false;
            }
            if (parameter.txtPositionEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter position!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPositionEmployee.Focus();
                return false;
            }
            if (!employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse(parameter.txtDateEmployee.Text), DateTime.Parse(parameter.txtDateStartWorkEmployee.Text)))
            {
                CustomMessageBox.Show("Conflict Date of Birth and Date Start Working", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateEmployee.Focus();
                return false;
            }
            if (!employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse(parameter.txtDateStartWorkEmployee.Text)))
            {
                CustomMessageBox.Show("Conflict Date Start Working and Today", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateStartWorkEmployee.Focus();
                return false;
            }
            return true;
        }

        private void SaveAddEmployee(AddEmployeeWindow parameter)
        {
            if (CheckEmptyAddEmployee(parameter))
            {
                int idEmployee = int.Parse(parameter.txtIdEmployee.Text);
                int idPosition = parameter.txtPositionEmployee.SelectedIndex + 1;
                bool gender = parameter.txtGenderEmployee.Text == "Male" ? true : false;

                Employee employee = new Employee(
                    idEmployee,
                    idPosition,
                    parameter.txtNameEmployee.Text,
                    DateTime.Parse(parameter.txtDateEmployee.Text),
                    DateTime.Parse(parameter.txtDateStartWorkEmployee.Text),
                    parameter.txtAddressEmployee.Text,
                    parameter.txtphoneNumberEmployee.Text,
                    gender,
                    false);
                if (EmployeeDAL.Instance.AddNewEmployee(employee) == 1)
                {
                    Notification.Instance.Success("Add Employee Success!");
                    parameter.Close();
                    name = "";
                    LoadEmployee(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Add Employee Failed!");
                    parameter.Close();
                    name = "";
                }
            }
        }
        #endregion

        #region SaveChangeInfoEmployee
        private bool CheckEmptyUpdateEmployee(ChangeEmployeeWindow parameter)
        {
            if (parameter.txtNameEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNameEmployee.Focus();
                return false;
            }
            if (parameter.txtDateEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter date!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateEmployee.Focus();
                return false;
            }
            if (parameter.txtDateStartWorkEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter date start work!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateStartWorkEmployee.Focus();
                return false;
            }
            if (parameter.txtGenderEmployee.Text == "")
            {
                CustomMessageBox.Show("TPlease enter gender!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtGenderEmployee.Focus();
                return false;
            }
            if (parameter.txtPositionEmployee.Text == "")
            {
                CustomMessageBox.Show("Please enter position!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPositionEmployee.Focus();
                return false;
            }
            if (!employeeDALValidation.CompareDateAnDateStartWork(DateTime.Parse(parameter.txtDateEmployee.Text), DateTime.Parse(parameter.txtDateStartWorkEmployee.Text)))
            {
                CustomMessageBox.Show("Conflict Date of Birth and Date Start Working", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateEmployee.Focus();
                return false;
            }
            if (!employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.Parse(parameter.txtDateStartWorkEmployee.Text)))
            {
                CustomMessageBox.Show("Conflict Date Start Working and Today", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateStartWorkEmployee.Focus();
                return false;
            }
            return true;
        }
        private void SaveChangeInformationEmployee(ChangeEmployeeWindow parameter)
        {
            if (CheckEmptyUpdateEmployee(parameter))
            {
                int idEmployee = int.Parse(parameter.txtIdEmployee.Text);
                int idPosition = parameter.txtPositionEmployee.SelectedIndex + 1;
                bool gender = parameter.txtGenderEmployee.Text == "Male" ? true : false;

                Employee employee = new Employee(
                    idEmployee,
                    idPosition,
                    parameter.txtNameEmployee.Text,
                    DateTime.Parse(parameter.txtDateEmployee.Text),
                    DateTime.Parse(parameter.txtDateStartWorkEmployee.Text),
                    parameter.txtAddressEmployee.Text,
                    parameter.txtphoneNumberEmployee.Text,
                    gender,
                    false);
                if (EmployeeDAL.Instance.UpdateEmployee(employee) == 1)
                {
                    Notification.Instance.Success("Update Employee Success!");
                    parameter.Close();
                    name = "";
                    LoadEmployee(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Update Employee Failed!");
                    parameter.Close();
                    name = "";
                }
            }
        }
        #endregion


        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
