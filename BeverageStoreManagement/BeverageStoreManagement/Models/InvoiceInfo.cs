using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class InvoiceInfo
    {
        private int idInvoiceInfo;
        private int idInvoice;
        private int idGood;
        private int quantity;
        private double price;
        private double intoMoney;

        public int IdInvoiceInfo { get => idInvoiceInfo; set => idInvoiceInfo = value; }
        public int IdInvoice { get => idInvoice; set => idInvoice = value; }
        public int IdGood { get => idGood; set => idGood = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public double Price { get => price; set => price = value; }
        public double IntoMoney { get => intoMoney; set => intoMoney = value; }

        public InvoiceInfo() { }
        public InvoiceInfo(int idInvoiceInfo, int idInvoice, int idGood, int quantity, double price, double intoMoney) 
        {
            this.idInvoiceInfo = idInvoiceInfo;
            this.idInvoice = idInvoice;
            this.idGood = idGood;
            this.quantity = quantity;
            this.price = price;
            this.intoMoney = intoMoney;
        }
    }
}
