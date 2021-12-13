using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class ReceiptVoucher : Voucher
    {
        private int idReceiptVoucher;

        public int IdReceiptVoucher { get => idReceiptVoucher; set => idReceiptVoucher = value; }
        public int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public DateTime Date { get => date; set => date = value; }
        public double TotalMoney { get => totalMoney; set => totalMoney = value; }
        public string Note { get => note; set => note = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public ReceiptVoucher() { }
        public ReceiptVoucher(int idReceiptVoucher, int idEmployee, DateTime date, double totalMoney, string note, bool isDelete)
        {
            this.idReceiptVoucher = idReceiptVoucher;
            this.idEmployee = idEmployee;
            this.date = date;
            this.totalMoney = totalMoney;
            this.note = note;
            this.isDelete = isDelete;
        }
    }
}
