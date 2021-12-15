using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Product
    {
        private int idProduct;
        private int idProductType;
        private string nameProduct;
        private string description;
        private int price;
        private bool status;
        private byte[] image;
        private bool isDelete;

        public int IdProduct { get => idProduct; set => idProduct = value; }
        public int IdProductType { get => idProductType; set => idProductType = value; }
        public string NameProduct { get => nameProduct; set => nameProduct = value; }
        public string Description { get => description; set => description = value; }
        public int Price { get => price; set => price = value; }
        public bool Status { get => status; set => status = value; }
        public byte[] Image { get => image; set => image = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public Product() { }
        public Product(int idProduct, int idProductType, string nameProduct, string description, int price, bool status, byte[] image, bool isDelete)
        {
            this.idProduct = idProduct;
            this.idProductType = idProductType;
            this.nameProduct = nameProduct;
            this.description = description;
            this.price = price;
            this.status = status;
            this.image = image;
            this.isDelete = isDelete;
        }
    }
}
