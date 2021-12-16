using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Material
    {
        private int idMaterial;
        private string nameMaterial;
        private string type;
        private string countUnit;
        private double quantity;
        private double purchasePrice;
        private byte[] image;
        private bool status;
        private bool isDelete;

        public int IdMaterial { get => idMaterial; set => idMaterial = value; }
        public string NameMaterial { get => nameMaterial; set => nameMaterial = value; }
        public string Type { get => type; set => type = value; }
        public string CountUnit { get => countUnit; set => countUnit = value; }
        public double Quantity { get => quantity; set => quantity = value; }
        public double PurchasePrice { get => purchasePrice; set => purchasePrice = value; }
        public byte[] Image { get => image; set => image = value; }
        public bool Status { get => status; set => status = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public Material() { }
        public Material(int idMaterial, string nameMaterial, string type, string countUnit, double quantity, double purchasePrice, byte[] image, bool status, bool isDelete)
        {
            this.idMaterial = idMaterial;
            this.nameMaterial = nameMaterial;
            this.type = type;
            this.countUnit = countUnit;
            this.quantity = quantity;
            this.purchasePrice = purchasePrice;
            this.image = image;
            this.status = status;
            this.isDelete = isDelete;
        }

    }
}
