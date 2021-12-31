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
    class EmployeeTestCompareDateStartWorkingAnDateNowFunction
    {
        EmployeeDALValidation employeeDALValidation = new EmployeeDALValidation();
        [Test]
        public void TestCase1()
        {
            Assert.AreEqual(true, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.ParseExact("21/09/2021", "dd/MM/yyyy", null)));
            Assert.AreEqual(true, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.ParseExact("10/01/2020", "dd/MM/yyyy", null)));
            Assert.AreEqual(true, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.ParseExact("31/12/2020", "dd/MM/yyyy", null)));
        }
        [Test]
        public void TestCase2()
        {
            Assert.AreEqual(false, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.ParseExact("22/02/2022", "dd/MM/yyyy", null)));
            Assert.AreEqual(false, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.ParseExact("10/01/3000", "dd/MM/yyyy", null)));
            Assert.AreEqual(false, employeeDALValidation.CompareDateStartWorkingAnDateNow(DateTime.ParseExact("31/12/2222", "dd/MM/yyyy", null)));
        }
    }
}
