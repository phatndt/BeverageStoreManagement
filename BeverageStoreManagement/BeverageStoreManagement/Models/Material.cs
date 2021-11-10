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
        private int quatity;
        private double price;
        private bool status;
        private bool isDelete;

        public int IdMaterial { get => idMaterial; set => idMaterial = value; }
        public string NameMaterial { get => nameMaterial; set => nameMaterial = value; }
        public int Quatity { get => quatity; set => quatity = value; }
        public double Price { get => price; set => price = value; }
        public bool Status { get => status; set => status = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public Material() { }
        public Material(int idMaterial, string nameMaterial, int quatity, double price, bool status, bool isDelete)
        {
            this.idMaterial = idMaterial;
            this.nameMaterial = nameMaterial;
            this.quatity = quatity;
            this.price = price;
            this.status = status;
            this.isDelete = isDelete;
        }

    }
}
