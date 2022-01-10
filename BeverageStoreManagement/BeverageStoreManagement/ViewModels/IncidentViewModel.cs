using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views.Incident;
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
using System.Windows.Media;

namespace BeverageStoreManagement.ViewModels
{
    class IncidentViewModel : BaseViewModel
    {
        private MainWindow mainWindow;
        private string totalMoney;
        private List<Incident> incidents = new List<Incident>();
        private List<Incident> newIncidents = new List<Incident>();
        public string TotalMoney { get => totalMoney; set => totalMoney = value; }
        //addSupplierWindow
        public ICommand SaveAddIncidentCommand { get; set; }
        public ICommand ExitAddIncidentCommand { get; set; }
        //gridIncident
        public ICommand OpenWindowAddIncidentCommand { get; set; }
        public ICommand LoadIncidentCommand { get; set; }

        public ICommand CloseWindowAddIncidentCommand { get; set; }

        public ICommand SeparateThousandsCommand { get; set; }

        public ICommand ReloadIncidentCommand { get; set; }

        public ICommand ResetIncidentWindowCommand { get; set; }

        public ICommand UpdateIncidentCommand { get; set; }

        private List<IncidentControls> listIncidentToView = new List<IncidentControls>();
        public List<IncidentControls> ListIncidentToView { get => listIncidentToView; set => listIncidentToView = value; }

        public IncidentViewModel()
        {
            //Add new incident 
            OpenWindowAddIncidentCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => AddNewIncident(parameter));
            LoadIncidentCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadIncident(parameter));
            CloseWindowAddIncidentCommand = new RelayCommand<AddIncidentWindow>((parameter) => true, (parameter) => parameter.Close());

            SaveAddIncidentCommand = new RelayCommand<AddIncidentWindow>(parameter => true, parameter => SaveAddIncident(parameter));
            ExitAddIncidentCommand = new RelayCommand<AddIncidentWindow>(parameter => true, parameter => parameter.Close());

            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));
            ReloadIncidentCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => reloadIncident(parameter));
            ResetIncidentWindowCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => resetWindow(parameter));
            UpdateIncidentCommand = new RelayCommand<IncidentControls>((parameter) => true, (parameter) => saveUpdateInfo(parameter));
        }
        #region gridSupplier
        private void LoadIncident(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.incidentItem.Children.Clear();
            incidents = IncidentDAL.Instance.GetList();
            ListIncidentToView.Clear();

            int id = 1;
            bool flag = true;
                
            foreach (Incident incident in incidents)
            {
                IncidentControls incidentControls = new IncidentControls(); 
                flag = !flag;
                if (flag)
                {
                    incidentControls.grdMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFffffff");
                }

                incidentControls.id.Text = incident.IdIncident.ToString();
                incidentControls.name.Text = incident.Description;
                incidentControls.date.Text = incident.Date.ToString("dd/MM/yyyy");
                incidentControls.employee.Text = EmployeeDAL.Instance.GetEmployeeById(incident.IdEmployee).Name;
                incidentControls.money.Text = SeparateThousands(incident.TotalMoney.ToString());
                incidentControls.checkBoxStatus.IsChecked = incident.Status;
                incidentControls.checkBoxPay.IsChecked = incident.Pay; 

                ListIncidentToView.Add(incidentControls);

                id++;
            }
            LoadLoad(mainWindow);
        }

        public void reloadIncident(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.incidentItem.Children.Clear();
            ListIncidentToView.Clear();
            List<Incident> incidents2 = new List<Incident>(incidents);
            bool flag = true;
            int id = 1;

            if (parameter.rdbUnder20.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.TotalMoney > 20000)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdb20to50.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.TotalMoney <= 20000 || incident.TotalMoney >= 50000)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdb50to100.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.TotalMoney < 50000 || incident.TotalMoney > 100000)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdbOver100.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.TotalMoney <= 100000)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdbNo.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.Pay)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdbYes.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.Pay == false)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdbDone.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.Status == false)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdbUndone.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.Status)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdbMonth.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.Date.Month != DateTime.Now.Month)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }
            if (mainWindow.rdbYear.IsChecked == true)
            {
                foreach (Incident incident in incidents)
                {
                    if (incident.Date.Year != DateTime.Now.Year)
                    {
                        incidents2.Remove(incident);
                    }
                }
            }

            foreach (Incident incident in incidents2)
            {
                IncidentControls incidentControls = new IncidentControls();
                flag = !flag;
                if (flag)
                {
                    incidentControls.grdMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFffffff");
                }

                incidentControls.id.Text = incident.IdIncident.ToString();
                incidentControls.name.Text = incident.Description;
                incidentControls.date.Text = incident.Date.ToString("dd/MM/yyyy");
                incidentControls.employee.Text = EmployeeDAL.Instance.GetEmployeeById(incident.IdEmployee).Name;
                incidentControls.money.Text = SeparateThousands(incident.TotalMoney.ToString());
                incidentControls.checkBoxStatus.IsChecked = incident.Status;
                incidentControls.checkBoxPay.IsChecked = incident.Pay;

                ListIncidentToView.Add(incidentControls);

                id++;
            }
            LoadLoad(mainWindow);
        }

        private void saveUpdateInfo(IncidentControls parameter)
        {
            int idIncident = int.Parse(parameter.id.Text);
            bool status = parameter.checkBoxStatus.IsChecked ?? false;
            bool pay = parameter.checkBoxPay.IsChecked ?? false;

            if(IncidentDAL.Instance.UpdateIncident(idIncident,status, pay) == 1)
            {
                Notification.Instance.Success("Update incident successfully!");
                LoadIncident(mainWindow);
            }
            else
            {
                Notification.Instance.Failed("Update incident failed!");
            }
        }

        public void LoadLoad(MainWindow parameter)
        {
            for(int i = 0; i < ListIncidentToView.Count; i++)
            {
                parameter.incidentItem.Children.Add(ListIncidentToView[i]);
            }
        }

        private void AddNewIncident(MainWindow parameter)
        {
            int idIncident = IncidentDAL.Instance.GetMaxIdIncident() + 1;
            if (idIncident != 0)
            {
                AddIncidentWindow addIncidentWindow = new AddIncidentWindow();
                addIncidentWindow.txtIdIncident.Text = idIncident.ToString();
                addIncidentWindow.txtDateIncident.Text = DateTime.Now.ToString("MM/dd/yyyy");
                addIncidentWindow.txtIdEmployee.Text = CurrentAccount.IdEmployee.ToString();
                addIncidentWindow.txtEmployee.Text = CurrentEmployee.Name;
                addIncidentWindow.ShowDialog();
            }
            else
            {
                Notification.Instance.Failed("Connected to database failed!!!");
            }
        }

        private void resetWindow(MainWindow mainWindow)
        {
            mainWindow.rdbUnder20.IsChecked = false;
            mainWindow.rdb20to50.IsChecked = false;
            mainWindow.rdb50to100.IsChecked = false;
            mainWindow.rdbOver100.IsChecked = false;

            mainWindow.rdbYes.IsChecked = false;
            mainWindow.rdbNo.IsChecked = false;

            mainWindow.rdbDone.IsChecked = false;
            mainWindow.rdbUndone.IsChecked = false;

            mainWindow.rdbMonth.IsChecked = false;
            mainWindow.rdbYear.IsChecked = false;

            reloadIncident(mainWindow);
        }
        #endregion

        #region addSupplierWindow
        private bool CheckEmptyAddIncident(AddIncidentWindow parameter)
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
            if (parameter.txtDescriptionIncident.Text == "")//14
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
            if (CheckEmptyAddIncident(parameter))
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
