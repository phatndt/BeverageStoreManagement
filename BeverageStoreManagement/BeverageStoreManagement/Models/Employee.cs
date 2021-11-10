using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Employee : People
    {
        private int idEmployee;
        private int idPosition;
        private DateTime dateStartWorking;
        private bool isDelete;

        public int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public int IdPosition { get => idPosition; set => idPosition = value; }
        public string Name { get => name; set => name = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public bool Gender { get => gender; set => gender = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public DateTime DateStartWorking { get => dateStartWorking; set => dateStartWorking = value; }

        public Employee() { }

        public Employee(int idEmployee, int idPosition, string name, DateTime dateOfBirth, DateTime dateStartWorking, bool gender, bool isDelete)
        {
            this.idEmployee = idEmployee;
            this.idPosition = idPosition;
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.dateStartWorking = dateStartWorking;
            this.gender = gender;
            this.isDelete = isDelete;
        }

    }
}
