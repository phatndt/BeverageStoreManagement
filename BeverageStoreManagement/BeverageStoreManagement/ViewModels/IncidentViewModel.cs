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
        public string TotalMoney { get => totalMoney; set => totalMoney = value; }
        //addSupplierWindow
        public ICommand SaveAddIncidentCommand { get; set; }
        public ICommand ExitAddIncidentCommand { get; set; }
        //gridIncident
        public ICommand OpenWindowAddIncidentCommand { get; set; }
        public ICommand LoadIncidentCommand { get; set; }

        public ICommand CloseWindowAddIncidentCommand { get; set; }

        public ICommand SeparateThousandsCommand { get; set; }


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
        }
        #region gridSupplier
        private void LoadIncident(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.incidentItem.Children.Clear();
            List<Incident> incidents = IncidentDAL.Instance.GetList();
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
                incidentControls.status.IsChecked = incident.Status;
                incidentControls.pay.IsChecked = incident.Pay; 

                ListIncidentToView.Add(incidentControls);

                id++;
            }
            LoadLoad(mainWindow);
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
