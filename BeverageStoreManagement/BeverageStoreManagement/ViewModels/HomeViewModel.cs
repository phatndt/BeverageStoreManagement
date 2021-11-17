using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
namespace BeverageStoreManagement.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        public ICommand GetUidCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }

        private string uid;
        public string Uid { get => uid; set => uid = value; }
        public HomeViewModel()
        {
            GetUidCommand = new RelayCommand<RadioButton>((parameter) => true, (parameter) => Uid = parameter.Uid);
            SwitchTabCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => SwitchTab(parameter));
        }

        private void SwitchTab(MainWindow parameter)
        {
            int index = int.Parse(Uid);

            parameter.grdSale.Visibility = Visibility.Collapsed;
            parameter.grdInvoice.Visibility = Visibility.Collapsed;
            parameter.grdProduct.Visibility = Visibility.Collapsed;
            parameter.grdMaterial.Visibility = Visibility.Collapsed;
            //parameter.grdImportBill.Visibility = Visibility.Collapsed;

            parameter.grdPaymentVoucher.Visibility = Visibility.Collapsed;
            parameter.grdReceiptVoucher.Visibility = Visibility.Collapsed;
            parameter.grdIncident.Visibility = Visibility.Collapsed;
            parameter.grdEmployee.Visibility = Visibility.Collapsed;
            parameter.grdAccount.Visibility = Visibility.Collapsed;

            switch (index)
            {
                case 0:
                    break;
                case 1:
                    parameter.grdSale.Visibility = Visibility.Visible;
                    break;
                case 2:
                    parameter.grdInvoice.Visibility = Visibility.Visible;
                    break;
                case 3:
                    parameter.grdProduct.Visibility = Visibility.Visible;
                    break;
                case 4:
                    parameter.grdMaterial.Visibility = Visibility.Visible;
                    break;
                case 5:
                    break;
                case 6:
                    parameter.grdPaymentVoucher.Visibility = Visibility.Visible;
                    break;
                case 7:
                    parameter.grdReceiptVoucher.Visibility = Visibility.Visible;
                    break;
                case 8:
                    parameter.grdIncident.Visibility = Visibility.Visible;
                    break;
                case 9:
                    parameter.grdEmployee.Visibility = Visibility.Visible;
                    break;
                case 10:
                    parameter.grdAccount.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
