using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class PaymentVoucher : Voucher
    {
        private int idPaymentVoucher;
        private int idImportBill;
        public int IdPaymentVoucher { get => idPaymentVoucher; set => idPaymentVoucher = value; }
        public int IdImportBill { get => idImportBill; set => idImportBill = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public DateTime Date { get => date; set => date = value; }
        public double TotalMoney { get => totalMoney; set => totalMoney = value; }
        public string Note { get => note; set => note = value; }
        public PaymentVoucher() { }
        public PaymentVoucher(int idPaymentVoucher, int idEmployee, int idImportBill, DateTime date, double totalMoney, string note, bool isDelete) 
        {
            this.idPaymentVoucher = idPaymentVoucher;
            this.idEmployee = idEmployee;
            this.idImportBill = idImportBill;
            this.date = date;
            this.totalMoney = totalMoney;
            this.note = note;
            this.isDelete = isDelete;
        }
    }
}
