using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Supplier
    {
        private int idSupplier;
        private string nameSupplier;
        private string addressSupplier;
        private string phoneSupplier;
        private bool isDelete;

        public int IdSupplier { get => idSupplier; set => idSupplier = value; }
        public string NameSupplier { get => nameSupplier; set => nameSupplier = value; }
        public string AddressSupplier { get => addressSupplier; set => addressSupplier = value; }
        public string PhoneSupplier { get => phoneSupplier; set => phoneSupplier = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public Supplier() { }
        public Supplier(int idSupplier, string nameSupplier, string addressSupplier, string phoneSupplier, bool isDelete)
        {
            this.idSupplier = idSupplier;
            this.nameSupplier = nameSupplier;
            this.addressSupplier = addressSupplier;
            this.phoneSupplier = phoneSupplier;
            this.isDelete = isDelete;
        }
    }
}
