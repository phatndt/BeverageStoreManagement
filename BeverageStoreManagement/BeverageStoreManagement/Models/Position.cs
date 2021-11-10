using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Position
    {
        private int idPosition;
        private string namePosition;
        private double salary;
        private double standardWorkDays;
        private bool isDelete;

        public int IdPosition { get => idPosition; set => idPosition = value; }
        public string NamePosition { get => namePosition; set => namePosition = value; }
        public double Salary { get => salary; set => salary = value; }
        public double StandardWorkDays { get => standardWorkDays; set => standardWorkDays = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public Position() { }

        public Position(int idPosition, string namePosition, double salary, double standardWorkDays, bool isDelete)
        {
            this.idPosition = idPosition;
            this.namePosition = namePosition;
            this.salary = salary;
            this.standardWorkDays = standardWorkDays;
            this.isDelete = isDelete;
        }
    }
}
