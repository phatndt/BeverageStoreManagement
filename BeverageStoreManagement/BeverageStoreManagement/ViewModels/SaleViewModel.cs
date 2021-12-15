using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views;
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
    public class SaleViewModel : BaseViewModel
    {

        private string total = "150000";

        public string Total { get => total; set => total = value; }

        private string datez = "150000";

        public string Datez { get => datez; set => datez = value; }

        public ICommand SeparateThousandsCommand { get; set; }
        public ICommand LoadInvoiceCommand { get; set; }

        //AddInvoiceWindow
        public ICommand OpenAddInvoiceWindowCommand { get; set; }

        public SaleViewModel()
        {
            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));
            LoadInvoiceCommand = new RelayCommand<InvoicePage>((parameter) => true, (parameter) => LoadInvoice(parameter));
            OpenAddInvoiceWindowCommand = new RelayCommand<InvoicePage>((parameter) => true, (parameter) => OpenAddInvoiceWindow(parameter));
            Loading();
        }

        //AddInvoiceWindow
        private void OpenAddInvoiceWindow(InvoicePage parameter)
        {
            AddInvoiceWindow addInvoiceWindow = new AddInvoiceWindow();
            addInvoiceWindow.lbDate.Content = DateTime.Now.ToString("dd/MM/yyyy");
            addInvoiceWindow.ShowDialog();
        }

        private void LoadInvoice(InvoicePage parameter)
        {
            bool flag = false;
            for (int i = 0; i<= 20; i++)
            {
                InvoiceControl invoiceControl = new InvoiceControl();
                flag = !flag;
                if (flag)
                {
                    invoiceControl.grdMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFffffff");
                }
                parameter.stkInvoice.Children.Add(invoiceControl);
            }
        }

        public void Loading()
        {
            Datez = DateTime.Now.ToString("dd/MM/yyyy");
        }
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
