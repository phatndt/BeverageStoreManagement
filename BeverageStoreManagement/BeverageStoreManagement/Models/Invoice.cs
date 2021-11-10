using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Invoice : Info
    {
        private int idInvoice;
        private double moneyCustomer;
        private int tableNumber;
        private bool isDelete;

        public int IdInvoice { get => idInvoice; set => idInvoice = value; }
        public int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public DateTime Date { get => date; set => date = value; }
        public double TotalMoney { get => totalMoney; set => totalMoney = value; }
        public double MoneyCustomer { get => moneyCustomer; set => moneyCustomer = value; }
        public int TableNumber { get => tableNumber; set => tableNumber = value; }
        public bool Status { get => status; set => status = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public Invoice() { }
        public Invoice(int idInvoice, int idEmployee, DateTime date, double totalMoney, double moneyCustomer, int tableNumber, bool status, bool isDelete) 
        {
            this.idInvoice = idInvoice;
            this.idEmployee = idEmployee;
            this.date = date;
            this.totalMoney = totalMoney;
            this.moneyCustomer = moneyCustomer;
            this.tableNumber = tableNumber;
            this.status = status;
            this.isDelete = isDelete;
        }
    }
}
