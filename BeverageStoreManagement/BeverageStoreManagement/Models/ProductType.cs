using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class ProductType
    {
        private int idProductType;
        private string nameProductType;
        private double profitPercentage;
        private bool isDelete;

        public int IdProductType { get => idProductType; set => idProductType = value; }
        public string MameProductType { get => nameProductType; set => nameProductType = value; }
        public double ProfitPercentage { get => profitPercentage; set => profitPercentage = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public ProductType() { }

        public ProductType(int idProductType, string nameProductType, double profitPercentage, bool isDelete)
        {
            this.idProductType = idProductType;
            this.nameProductType = nameProductType;
            this.profitPercentage = profitPercentage;
            this.isDelete = isDelete;
        }
    }
}
