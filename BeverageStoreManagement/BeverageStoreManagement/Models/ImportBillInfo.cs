using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class ImportBillInfo
    {
        private int idImportBillInfo;
        private int idImportBill;
        private int idMaterial;
        private int quantity;
        private string unit;
        private double price;
        private double intoMoney;

        public int IdImportBillInfo { get => idImportBillInfo; set => idImportBillInfo = value; }
        public int IdImportBill { get => idImportBill; set => idImportBill = value; }
        public int IdMaterial { get => idMaterial; set => idMaterial = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public double Price { get => price; set => price = value; }
        public double IntoMoney { get => intoMoney; set => intoMoney = value; }
        public string Unit { get => unit; set => unit = value; }

        public ImportBillInfo() { }
        public ImportBillInfo(int idImportBillInfo, int idImportBill, int idMaterial, int quantity, string unit, double price, double intoMoney) 
        {
            this.idImportBillInfo = idImportBillInfo;
            this.idImportBill = idImportBill;
            this.idMaterial = idMaterial;
            this.quantity = quantity;
            this.unit = unit;
            this.price = price;
            this.intoMoney = intoMoney;
        }
    }
}
