using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class GoodType
    {
        private int idGoodType;
        private string nameGoodType;
        private double profitPercentage;
        private bool isDelete;

        public int IdGoodType { get => idGoodType; set => idGoodType = value; }
        public string NameGoodType { get => nameGoodType; set => nameGoodType = value; }
        public double ProfitPercentage { get => profitPercentage; set => profitPercentage = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public GoodType() { }

        public GoodType(int idGoodType, string nameGoodType, double profitPercentage, bool isDelete)
        {
            this.idGoodType = idGoodType;
            this.nameGoodType = nameGoodType;
            this.profitPercentage = profitPercentage;
            this.isDelete = isDelete;
        }
    }
}
