using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    class Incident : Info
    {
        private int idIncident;
        private string description;
        private bool pay;
        private bool isDelete;

        public int IdIncident { get => idIncident; set => idIncident = value; }
        public int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Description { get => description; set => description = value; }
        public bool Status { get => status; set => status = value; }
        public bool Pay { get => pay; set => pay = value; }
        public double TotalMoney { get => totalMoney; set => totalMoney = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }

        public Incident() { }
        public Incident(int idIncident, int idEmployee, DateTime date, string description, bool status, bool pay, double totalMoney, bool isDelete)
        {
            this.idIncident = idIncident;
            this.idEmployee = idEmployee;
            this.date = date;
            this.description = description;
            this.status = status;
            this.pay = pay;
            this.totalMoney = totalMoney;
            this.isDelete = isDelete;
        }
    }
}
