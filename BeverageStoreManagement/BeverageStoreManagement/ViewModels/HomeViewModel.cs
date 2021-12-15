using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
namespace BeverageStoreManagement.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        public ICommand GetUidCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        private string uid;
        public string Uid { get => uid; set => uid = value; }
        public HomeViewModel()
        {
            GetUidCommand = new RelayCommand<Button>((parameter) => true, (parameter) => Uid = parameter.Uid);
            SwitchTabCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => SwitchTab(parameter));

            LogoutCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => logout(parameter));
        }

        private void SwitchTab(MainWindow parameter)
        {
            int index = int.Parse(Uid);

            parameter.grdHome.Visibility = Visibility.Collapsed;
            parameter.grdSale.Visibility = Visibility.Collapsed;
            parameter.grdInvoice.Visibility = Visibility.Collapsed;
            parameter.grdProduct.Visibility = Visibility.Collapsed;
            parameter.grdMaterial.Visibility = Visibility.Collapsed;
            parameter.grdImportBill.Visibility = Visibility.Collapsed;

            parameter.grdPaymentVoucher.Visibility = Visibility.Collapsed;
            parameter.grdReceiptVoucher.Visibility = Visibility.Collapsed;
            parameter.grdIncident.Visibility = Visibility.Collapsed;
            parameter.grdEmployee.Visibility = Visibility.Collapsed;
            parameter.grdAccount.Visibility = Visibility.Collapsed;

            parameter.grdSupplier.Visibility = Visibility.Collapsed;

            string focusColor = "#FF1e88e5";
            string color = "#FF828282";


            parameter.rdHome.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdSale.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdInvoice.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdProduct.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdMaterial.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdImportBill.Foreground = (Brush)new BrushConverter().ConvertFrom(color);

            parameter.rdPaymentVoucher.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdReceiptVoucher.Foreground = (Brush)new BrushConverter().ConvertFrom(color);

            parameter.rdIncident.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdEmployee.Foreground = (Brush)new BrushConverter().ConvertFrom(color);
            parameter.rdAcount.Foreground = (Brush)new BrushConverter().ConvertFrom(color);

            parameter.rdSupplier.Foreground = (Brush)new BrushConverter().ConvertFrom(color);

            switch (index)
            {
                case 0:
                    parameter.grdHome.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdHome.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 1:
                    parameter.grdSale.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdSale.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 2:
                    parameter.grdInvoice.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdInvoice.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 3:
                    parameter.grdProduct.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdProduct.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 4:
                    parameter.grdMaterial.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdMaterial.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 5:
                    parameter.grdImportBill.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdImportBill.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 6:
                    parameter.grdPaymentVoucher.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdPaymentVoucher.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 7:
                    parameter.grdReceiptVoucher.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdReceiptVoucher.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 8:
                    parameter.grdIncident.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdIncident.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 9:
                    parameter.grdEmployee.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdEmployee.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 10:
                    parameter.grdAccount.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdAcount.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
                case 12:
                    parameter.grdSupplier.Visibility = Visibility.Visible;
                    CloseExpander(parameter, index);
                    parameter.rdSupplier.Foreground = (Brush)new BrushConverter().ConvertFrom(focusColor);
                    break;
            }
        }
        void CloseExpander(MainWindow parameter, int index)
        {
            switch (index)
            {
                case 0:
                    //parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 1:
                    //parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 2:
                    parameter.expanderHome.IsExpanded = false;
                    //parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 3:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    //parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 4:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    //parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 5:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    //parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 6:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    //parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 7:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    //parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 8:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    //parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 9:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    //parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 10:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    //parameter.expanderManage.IsExpanded = false;
                    parameter.expanderPartner.IsExpanded = false;
                    break;
                case 12:
                    parameter.expanderHome.IsExpanded = false;
                    parameter.expanderStore.IsExpanded = false;
                    parameter.expanderWarehouse.IsExpanded = false;
                    parameter.expanderVoucher.IsExpanded = false;
                    parameter.expanderManage.IsExpanded = false;
                    //parameter.expanderPartner.IsExpanded = false;
                    break;
            }
        }

        void logout(MainWindow parameter)
        {
            parameter.Close();
        }
    }
}
