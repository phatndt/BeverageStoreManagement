using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class ImportBill : Info
    {
        private int idImportBill;
        private int idSupplier;
        private string note;
        private bool isDelete;

        public int IdImportBill { get => idImportBill; set => idImportBill = value; }
        public int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public int IdSupplier { get => idSupplier; set => idSupplier = value; }
        public DateTime Date { get => date; set => date = value; }
        public double TotalMoney { get => totalMoney; set => totalMoney = value; }
        public string Note { get => note; set => note = value; }
        public bool Status { get => status; set => status = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public ImportBill() { }
        public ImportBill(int idImportBill, int idEmployee, int idSupplier, DateTime date, double totalMoney, string note, bool status, bool isDelete)
        {
            this.idImportBill = idImportBill;
            this.idEmployee = idEmployee;
            this.idSupplier = idSupplier;
            this.date = date;
            this.totalMoney = totalMoney;
            this.note = note;
            this.status = status;
            this.isDelete = isDelete;
        }
    }
}
