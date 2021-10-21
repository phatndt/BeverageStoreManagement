using BeverageStoreManagement.Views;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    class PaymentVoucherViewModel: BaseViewModel
    {
        public ICommand SeparateThousandsCommand { get; set; }
        public ICommand OpenAddPaymentVoucherWindowCommand { get; set; }
        public PaymentVoucherViewModel()
        {
            OpenAddPaymentVoucherWindowCommand = new RelayCommand<PaymentVoucherPage>((parameter) => true, (parameter) => OpenPaymentVoucherWindow(parameter)); 
            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));
        }

        private void OpenPaymentVoucherWindow(PaymentVoucherPage parameter)
        {
            AddPaymentVoucherWindow addPaymentVoucherWindow = new AddPaymentVoucherWindow();
            addPaymentVoucherWindow.ShowDialog();
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

        public long ConvertToNumber(string str)
        {
            string[] s = str.Split(',');
            string tmp = "";
            foreach (string a in s)
            {
                tmp += a;
            }
            return long.Parse(tmp);
        }

        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
