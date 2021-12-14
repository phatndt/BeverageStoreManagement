using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BeverageStoreManagement.Validations;

namespace BeverageStoreManagement.Test
{
    [TestFixture]
    class EmployeeTestDateStartWorkingFunction
    {
        EmployeeDALValidation employeeDALValidation = new EmployeeDALValidation();
        [Test]
        public void TestCase1()
        {
            Assert.AreEqual(true, employeeDALValidation.CompareDateAnDateStartWork(DateTime.ParseExact("20/09/2001", "dd/MM/yyyy", null), DateTime.ParseExact("21/09/2021", "dd/MM/yyyy", null)));
            Assert.AreEqual(true, employeeDALValidation.CompareDateAnDateStartWork(DateTime.ParseExact("01/01/2001", "dd/MM/yyyy", null), DateTime.ParseExact("21/09/2021", "dd/MM/yyyy", null)));
            Assert.AreEqual(true, employeeDALValidation.CompareDateAnDateStartWork(DateTime.ParseExact("31/12/2001", "dd/MM/yyyy", null), DateTime.ParseExact("21/09/2022", "dd/MM/yyyy", null)));
        }
        [Test]
        public void TestCase2()
        {
            Assert.AreEqual(false, employeeDALValidation.CompareDateAnDateStartWork(DateTime.ParseExact("22/09/2021", "dd/MM/yyyy", null), DateTime.ParseExact("21/09/2021", "dd/MM/yyyy", null)));
            Assert.AreEqual(false, employeeDALValidation.CompareDateAnDateStartWork(DateTime.ParseExact("01/01/3000", "dd/MM/yyyy", null), DateTime.ParseExact("21/09/2021", "dd/MM/yyyy", null)));
            Assert.AreEqual(false, employeeDALValidation.CompareDateAnDateStartWork(DateTime.ParseExact("31/12/2020", "dd/MM/yyyy", null), DateTime.ParseExact("21/09/2000", "dd/MM/yyyy", null)));
        }
    }
}
