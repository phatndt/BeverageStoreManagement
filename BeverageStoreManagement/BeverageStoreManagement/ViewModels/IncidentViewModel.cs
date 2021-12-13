using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views.Incident;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    class IncidentViewModel : BaseViewModel
    {
        private MainWindow mainWindow;
        //addSupplierWindow
        public ICommand SaveAddIncidentCommand { get; set; }
        public ICommand ExitAddIncidentCommand { get; set; }
        //gridIncident
        public ICommand OpenWindowAddIncidentCommand { get; set; }
        public ICommand LoadIncidentCommand { get; set; }

        public ICommand CloseWindowAddIncidentCommand { get; set; }

        public IncidentViewModel()
        {
            //Add new incident 
            OpenWindowAddIncidentCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => AddNewIncident(parameter));
            LoadIncidentCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadIncident(parameter));
            CloseWindowAddIncidentCommand = new RelayCommand<AddIncidentWindow>((parameter) => true, (parameter) => parameter.Close());

            SaveAddIncidentCommand = new RelayCommand<AddIncidentWindow>(parameter => true, parameter => SaveAddIncident(parameter));
            ExitAddIncidentCommand = new RelayCommand<AddIncidentWindow>(parameter => true, parameter => parameter.Close());
        }
        #region gridSupplier
        private void LoadIncident(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.incidentItem.Children.Clear();
            List<Incident> incidents = IncidentDAL.Instance.GetList();

            int id = 1;
            foreach (Incident incident in incidents)
            {
                IncidentControls incidentControls = new IncidentControls();

                incidentControls.id.Text = incident.IdIncident.ToString();
                incidentControls.name.Text = incident.Description;
                incidentControls.date.Text = incident.Date.ToString("dd/MM/yyyy");
                incidentControls.employee.Text = EmployeeDAL.Instance.GetEmployeeById(incident.IdEmployee).Name;
                incidentControls.money.Text = incident.TotalMoney.ToString();
                incidentControls.status.IsChecked = incident.Status;
                incidentControls.pay.IsChecked = incident.Pay;
                parameter.incidentItem.Children.Add(incidentControls);

                id++;
            }
        }
        private void AddNewIncident(MainWindow parameter)
        {
            int idIncident = IncidentDAL.Instance.GetMaxIdIncident() + 1;
            if (idIncident != 0)
            {
                AddIncidentWindow addIncidentWindow = new AddIncidentWindow();
                addIncidentWindow.txtIdIncident.Text = idIncident.ToString();
                addIncidentWindow.txtIdEmployee.Text = CurrentAccount.IdEmployee.ToString();
                addIncidentWindow.txtEmployee.Text = EmployeeDAL.Instance.GetEmployeeById(CurrentAccount.IdEmployee).Name;
                addIncidentWindow.ShowDialog();
            }
            else
            {
                Notification.Instance.Failed("Connected to database failed!!!");
            }
        }
        #endregion

        #region addSupplierWindow
        private bool CheckEmptyAddSupplier(AddIncidentWindow parameter)
        {
            if (parameter.txtDateIncident.Text == "")
            {
                CustomMessageBox.Show("Please enter date!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDateIncident.Focus();
                return false;
            }
            if (parameter.rdoDone.IsChecked == false && parameter.rdoUndone.IsChecked == false)
            {
                CustomMessageBox.Show("Please check status!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.rdoUndone.Focus();
                return false;
            }
            if (parameter.rdoPay.IsChecked == false && parameter.rdoNotPay.IsChecked == false)
            {
                CustomMessageBox.Show("Please check pay!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.rdoNotPay.Focus();
                return false;
            }
            if (parameter.txtDescriptionIncident.Text == "")
            {
                CustomMessageBox.Show("Please  enter description!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDescriptionIncident.Focus();
                return false;
            }
            if (parameter.txtMoneyIncident.Text == "" || parameter.txtMoneyIncident.Text == "0")
            {
                CustomMessageBox.Show("Please  enter money!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtDescriptionIncident.Focus();
                return false;
            }
            return true;
        }

        public void SaveAddIncident(AddIncidentWindow parameter)
        {
            if (CheckEmptyAddSupplier(parameter))
            {
                int idSupplier = int.Parse(parameter.txtIdIncident.Text);
                int idEmployee = int.Parse(parameter.txtIdEmployee.Text);

                bool status = parameter.rdoDone.IsChecked == true ? true : false;
                bool pay = parameter.rdoPay.IsChecked == true ? true : false;

                Incident incident = new Incident(
                       idSupplier,
                       idEmployee,
                       DateTime.Parse(parameter.txtDateIncident.Text),
                       parameter.txtDescriptionIncident.Text,
                       status,
                       pay,
                       double.Parse(parameter.txtMoneyIncident.Text),
                       false);

                if (IncidentDAL.Instance.AddNewIncident(incident) == 1)
                {
                    Notification.Instance.Success("Add Incident Success!");
                    parameter.Close();
                }
                else
                {
                    Notification.Instance.Failed("Add Incident Failed!");
                    parameter.Close();
                }
            }
        }
        #endregion
    }
}
