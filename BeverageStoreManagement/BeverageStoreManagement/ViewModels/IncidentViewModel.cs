using BeverageStoreManagement.Views.Incident;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    class IncidentViewModel : BaseViewModel
    {
        public ICommand OpenWindowAddIncidentCommand { get; set; }

        public ICommand CloseWindowAddIncidentCommand { get; set; }

        public IncidentViewModel()
        {
            //Add new incident 
            OpenWindowAddIncidentCommand = new RelayCommand<IncidentPage>((parameter) => true, (parameter) => AddNewIncident(parameter));
            CloseWindowAddIncidentCommand = new RelayCommand<AddIncidentWindow>((parameter) => true, (parameter) => parameter.Close());
        }

        private void AddNewIncident(IncidentPage parameter)
        {
            AddIncidentWindow addNewIncident = new AddIncidentWindow();
            addNewIncident.ShowDialog();
        }
    }
}
