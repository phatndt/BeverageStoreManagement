using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement.Validations
{
    public class EmployeeDALValidation
    {
        public EmployeeDALValidation()
        {

        }
        public bool CompareDateAnDateStartWork(DateTime date, DateTime dateStartWorking)
        {
            DateTime dateTime_18 = date.AddYears(18);
            if (dateTime_18.Date <= dateStartWorking.Date)
            {
                return true;
            }
            return false;
        }

        public bool CompareDateStartWorkingAnDateNow(DateTime dateStartWorking)
        {
            if (dateStartWorking < DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}
