using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Models
{
    abstract class Voucher
    {
        protected int idEmployee;
        protected DateTime date;
        protected double totalMoney;
        protected string note;
        protected bool isDelete;
    }
}
