using BeverageStoreManagement.DAL;
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

namespace BeverageStoreManagement.ViewModels
{
    public class PaymentVoucherViewModel: BaseViewModel
    {
        public ICommand SeparateThousandsCommand { get; set; }
        public ICommand OpenAddPaymentVoucherWindowCommand { get; set; }
        public ICommand SavePaymentVoucherWindowCommand { get; set; }
        
        public PaymentVoucherViewModel()
        {
            OpenAddPaymentVoucherWindowCommand = new RelayCommand<PaymentVoucherPage>((parameter) => true, (parameter) => OpenPaymentVoucherWindow(parameter)); 
            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));

            SavePaymentVoucherWindowCommand = new RelayCommand<AddPaymentVoucherWindow>((parameter) => true, (parameter) => SavePaymentVoucherWindow(parameter));
        }

        private void SavePaymentVoucherWindow(AddPaymentVoucherWindow parameter)
        {
            if (parameter.txtNameEmployee.Text != "" && 
                parameter.txtDate.Text != "" && 
                parameter.txtImportBill.Text != "" && 
                parameter.txtNote.Text != "" && 
                parameter.txtTotalMoney.Text != "")
            {
                int idPaymentVoucher = Int32.Parse(parameter.txtId.Text);
                int idEmployee = Int32.Parse(parameter.txtId.Text);
                int idImportBill = Int32.Parse(parameter.txtImportBill.Text);
                string note = parameter.txtNote.Text;
                double totalMoney = Convert.ToDouble(parameter.txtTotalMoney.Text);

                if(PaymentVoucherDAL.Instance.AddPaymentVoucher(idPaymentVoucher, idImportBill, idEmployee, DateTime.Now, totalMoney, note)) {
                    MessageBox.Show("Add Payment Voucher Successful");
                } else
                {
                    MessageBox.Show("Add Payment Voucher Failed");
                }
                
            } else
            {
                MessageBox.Show("Text Feild Empty");
            }
        }

        private void OpenPaymentVoucherWindow(PaymentVoucherPage parameter)
        {
            int idPaymentVoucher = PaymentVoucherDAL.Instance.GetMaxIdPaymentVoucher() + 1;
            if (idPaymentVoucher != 0)
            {
                AddPaymentVoucherWindow addPaymentVoucherWindow = new AddPaymentVoucherWindow();
                addPaymentVoucherWindow.txtId.Text = idPaymentVoucher.ToString();
                addPaymentVoucherWindow.ShowDialog();
                
            }
            else
            {
                Notification.Instance.Failed("Connected to database failed!!!");
            }
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
